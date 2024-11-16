using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.TFHResource;
using ThemModdingHerds.TFHResource.Data;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public class Application
{
    public const string CONFIG_NAME = "config.json";
    public const string MODS_NAME = "mods";
    public const string BACKUP_NAME = "backup";
    public CommandLine CommandLine {get;}
    public Config Config {get;}
    public ModDB ModDB {get;}
    public BackupManager BackupManager {get;}
    public Game Client {get;}
    public Game Server {get;}
    public Application(string[] argv)
    {
        CommandLine = new(this,argv);
        Console.Title = Velvet.NAME;
        Velvet.Info($"{Velvet.NAME} v{Dotnet.LibraryVersion}\n\nA Mod Loader/Tool for Z-Engine games");

        string config = Path.Combine(Dotnet.ExecutableFolder,CONFIG_NAME);
        Config = Config.ReadOrCreate(config);

        string mods = Path.Combine(Dotnet.ExecutableFolder,MODS_NAME);
        ModDB = new(mods);

        string backup = Path.Combine(Dotnet.ExecutableFolder,BACKUP_NAME);
        BackupManager = new(backup);

        Client = new Game(Config.ClientPath,Game.CLIENT_NAME);
        Server = new Game(Config.ServerPath,Game.SERVER_NAME);
    }
    public async void HandleArguments(string[] argv)
    {
        if(argv.Length == 1)
        {
            if(Uri.TryCreate(argv[0],UriKind.Absolute,out Uri? uri))
            {
                string content = uri.AbsolutePath;
                ModInstallResult result = ModInstallResult.Invalid;
                if(Url.IsUrl(content) || File.Exists(content) || Directory.Exists(content))
                {
                    result = await InstallMod(content);
                }
                else if(GameBanana.Argument.TryParse(content,out GameBanana.Argument? argument))
                {
                    result = await InstallMod(argument.Link);
                }
                Velvet.Info("you can close this window now...");
                Console.ReadLine();
                Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
            }
        }
    }
    public SetupResult Setup()
    {
        if(Directory.Exists(BackupManager.Folder) && Directory.Exists(ModDB.Folder) && File.Exists(CONFIG_NAME)) return SetupResult.NotRequired;
        Velvet.Info("setting up for first lanuch...");
        if(Config.IsOld(CONFIG_NAME))
        {
            Velvet.Error("your config file is old! Delete it and let it create a new one");
            return SetupResult.OldConfig;
        }
        if(!BackupGameFiles())
        {
            BackupManager.Clear();
            Velvet.Error("some of your game files have been tampered with! Make sure they're the original files!");
            return SetupResult.BackupFail;
        }
        Velvet.Info("finished setup!");
        return SetupResult.Success;
    }
    public void Reset()
    {
        BackupManager.Clear();
        ModDB.Clear();
        if(File.Exists(CONFIG_NAME))
            File.Delete(CONFIG_NAME);
    }
    private bool BackupGameFiles()
    {
        if(Directory.Exists(BackupManager.Folder)) return true;
        bool noTampering = true;
        if(Client.Valid())
            noTampering = BackupGameFiles(Client);
        if(Server.Valid())
            noTampering = BackupGameFiles(Server);
        return noTampering;
    }
    private bool BackupGameFiles(Game game)
    {
        bool Backup(string folder,IEnumerable<GameFile> gameFiles)
        {
            foreach(GameFile gameFile in gameFiles)
            {
                string file = Path.Combine(folder,gameFile.Name);
                if(!gameFile.Verify(file))
                {
                    Velvet.Error($"{gameFile.Name} has been tampered with! Make sure it's the original file");
                    return false;
                }
                if(!BackupManager.ExistsBackup(file))
                    Velvet.Info($"creating backup of {gameFile.Name}...");
                BackupManager.MakeBackup(file);
                continue;
            }
            return true;
        }
        bool noTampering = true;
        if(game.ExistsData01Folder())
            noTampering = Backup(game.GetData01Folder(),GameFiles.Data01);
        if(game.ExistsTFHResourcesFolder())
            noTampering = Backup(game.GetTFHResourcesFolder(),GameFiles.TFHResources);
        return noTampering;
    }
    public void ApplyMods()
    {
        ModDB.Refresh();
        Velvet.Info($"applying {ModDB.EnabledMods.Count} mods...");
        if(Client.Valid())
        {
            Velvet.Info("applying mods to client...");
            ApplyMods(Client);
        }
        if(Server.Valid())
        {
            Velvet.Info("applying mods to server...");
            ApplyMods(Server);
        }
    }
    public void Revert()
    {
        Velvet.Info("reverting files back to vanilla game...");
        if(Client.Valid())
        {
            Velvet.Info("reverting files from client...");
            Revert(Client);
        }
        if(Server.Valid())
        {
            Velvet.Info("reverting files from server...");
            Revert(Server);
        }
    }
    private void Revert(Game game)
    {
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(GameFile gameFile in GameFiles.Data01)
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(GameFile gameFile in GameFiles.TFHResources)
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
            }
        }
    }
    private void ApplyMods(Game game)
    {
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(GameFile gameFile in game.GetData01Files())
            {
                if(!gameFile.CanModify)
                    Velvet.Error($"WARN: {gameFile.Name} cannot be modified! Expect the game not lanuching!");
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
                Reader reader = new(file);
                RevergePackage target = reader.ReadRevergePackage();
                reader.Close();
                int modCount = 0;
                RevergePackage modded = new(target.Header,target,target.Metadata);
                foreach(Mod mod in ModDB.EnabledMods)
                {
                    RevergePackage? gfsmod = mod.GetRevergePackage(gameFile.Name);
                    if(gfsmod == null) continue;
                    modded.Metadata.Add(gfsmod.Metadata);
                    foreach(var pair in gfsmod)
                        modded.Add(pair.Value);
                    modCount++;
                }
                if(modCount == 0) continue;
                Velvet.Info($"applying {modCount} to {gameFile.Name}");
                File.Delete(file);
                Writer writer = new(file);
                writer.Write(modded);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(GameFile gameFile in GameFiles.TFHResources)
            {
                if(!gameFile.CanModify)
                    Velvet.Error($"WARN: {gameFile.Name} cannot be modified! Expect the game not lanuching!");
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
                Database target = Database.Open(file);
                Database modded = target.Clone();
                List<Database> mods = [];
                foreach(Mod mod in ModDB.EnabledMods)
                {
                    Database? db = mod.GetTFHResource(gameFile.Name);
                    if(db == null) continue;
                    mods.Add(db);
                }
                if(mods.Count == 0) continue;
                Velvet.Info($"applying {mods.Count} to {gameFile.Name}");
                modded.Upsert([..mods]);
                foreach(Database db in mods)
                    db.Close();
                target.Close();
                modded.Save(file,true);
                modded.Close();
            }
        }
    }
    private ModInstallResult InstallFolder(string? modFolder) => ModDB.InstallMod(modFolder);
    public async Task<ModInstallResult> InstallGameBananaMod(int id)
    {
        GameBanana.Mod? mod = await GameBanana.Mod.Fetch(id);
        if(mod == null) return ModInstallResult.Invalid;
        string file = await mod.DownloadLatestUpdate();
        return await InstallMod(file);
    }
    public async Task InstallGameBananaMod(GameBanana.Argument argument) => await InstallGameBananaMod(argument.GetId());
    public async Task<ModInstallResult> InstallMod(string? str)
    {
        if(str == null) return ModInstallResult.Invalid;
        if(int.TryParse(str,out int id))
        {
            Velvet.Info($"installing GameBanana Mod with id '{id}'...");
            return await InstallGameBananaMod(id);
        }
        if(Url.IsUrl(str))
        {
            Velvet.Info($"installing Mod from {str}...");
            string temp = await DownloadManager.GetTemp(str);
            return await InstallMod(temp);
        }
        string path = Path.Combine(Environment.CurrentDirectory,str);
        if(Directory.Exists(path))
        {
            Velvet.Info($"installing Mod folder from {path}...");
            return InstallFolder(path);
        }
        if(File.Exists(path))
        {
            Velvet.Info($"installing Mod file from {path}...");
            return InstallFolder(ArchiveUtils.ExtractArchive(path));
        }
        Velvet.Error($"couldn't install Mod from {str}");
        return ModInstallResult.Invalid;
    }
    public bool UninstallMod(string? id)
    {
        if(id == null) return false;
        Velvet.Info($"uninstalling Mod id '{id}'...");
        return ModDB.UninstallMod(id);
    }
    public void CreateMod(string? id)
    {
        if(id == null) return;
        ModInfo info = new()
        {
            Id = id
        };
        string path = Path.Combine(ModDB.Folder,id);
        Directory.CreateDirectory(path);
        info.Write(Path.Combine(path,Mod.MODINFO_NAME));
        Velvet.Info($"created Mod '{id}' at {path}");
    }
    public static void CreateRevergePackage(string? input,string? output)
    {
        if(input == null || output == null) return;
        string inputPath = Path.Combine(Environment.CurrentDirectory,input);
        string outputPath = Path.Combine(Environment.CurrentDirectory,output.EndsWith(".gfs") ? output : $"{output}.gfs");
        RevergePackage gfs = RevergePackage.Create(inputPath);
        Writer writer = new(outputPath);
        writer.Write(gfs);
        Velvet.Info($"created GFS at {outputPath}");
    }
    public static void CreateTFHResource(string? output)
    {
        if(output == null) return;
        string outputPath = Path.Combine(Environment.CurrentDirectory,output.EndsWith(".tfhres") ? output : $"{output}.tfhres");
        Database database = new();
        database.Save(outputPath);
        Velvet.Info($"created TFHRES at {outputPath}");
    }
    public static void Extract(string? input,string? output)
    {
        if(input == null || output == null) return;
        string inputPath = Path.Combine(Environment.CurrentDirectory,input);
        string outputPath = Path.Combine(Environment.CurrentDirectory,output);
        if(inputPath.EndsWith(".gfs"))
        {
            Velvet.Info($"extracting GFS at {inputPath} to {outputPath}...");
            GFS.Utils.Extract(inputPath,outputPath);
            return;
        }
        if(inputPath.EndsWith(".tfhres"))
        {
            Velvet.Info($"extracting GFS at {inputPath} to {outputPath}...");
            TFHResource.Utils.Extract(inputPath,outputPath);
            return;
        }
    }
    public void EnableMod(string? id)
    {
        if(id == null) return;
        ModDB.Refresh();
        Mod? mod = ModDB.FindModById(id);
        if(mod == null)
        {
            Velvet.Error($"no Mod with id '{id}' exists!");
            return;
        }
        Velvet.Info($"enabling Mod id '{id}'...");
        mod.Enable();
    }
    public void DisableMod(string? id)
    {
        if(id == null) return;
        ModDB.Refresh();
        Mod? mod = ModDB.FindModById(id);
        if(mod == null)
        {
            Velvet.Error($"no Mod with id '{id}' exists!");
            return;
        }
        Velvet.Info($"disabling Mod id '{id}'...");
        mod.Disable();
    }
}