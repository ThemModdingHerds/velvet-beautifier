using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.Levels;
using ThemModdingHerds.TFHResource;
using ThemModdingHerds.TFHResource.Data;
using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public class ModLoaderTool
{
    public const string CONFIG_NAME = "config.json";
    public const string MODS_NAME = "mods";
    public const string BACKUP_NAME = "backup";
    public const string LEVELS_NAME = "levels";
    private readonly string levelsPath;
    private readonly string gamenewsPath;
    public CommandLine CommandLine {get;}
    public Config Config {get;}
    private readonly string configPath;
    public ModDB ModDB {get;}
    public BackupManager BackupManager {get;}
    public Game Client {get;}
    public Game Server {get;}
    public ModLoaderTool(string[] argv)
    {
        CommandLine = new(this,argv);
        Dotnet.ConsoleTitle = Velvet.NAME;
        Velvet.Info($"{Velvet.NAME} v{Dotnet.LibraryVersion}\n\nA Mod Loader/Tool for Z-Engine games");

        levelsPath = Path.Combine(Dotnet.ExecutableFolder,LEVELS_NAME);

        configPath = Path.Combine(Dotnet.ExecutableFolder,CONFIG_NAME);
        Config = Config.ReadOrCreate(configPath);

        string mods = Path.Combine(Dotnet.ExecutableFolder,MODS_NAME);
        ModDB = new(mods);

        string backup = Path.Combine(Dotnet.ExecutableFolder,BACKUP_NAME);
        BackupManager = new(backup);
        gamenewsPath = Path.Combine(Dotnet.ExecutableFolder,BACKUP_NAME,BackupManager.GetBackupName(News.GAMENEWS_FILEPATH));

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
    private bool SetupNotRequired()
    {
        return Directory.Exists(BackupManager.Folder) &&
               Directory.Exists(ModDB.Folder) &&
               File.Exists(configPath) &&
               Directory.Exists(levelsPath) &&
               File.Exists(gamenewsPath);
    }
    public SetupResult Setup()
    {
        if(Config.IsOld(CONFIG_NAME))
        {
            Velvet.Error("your config file is old! Delete it and let it create a new one");
            return SetupResult.OldConfig;
        }
        if(SetupNotRequired())
            return SetupResult.NotRequired;
        Velvet.Info("setting up for first lanuch...");
        if(!BackupGameFiles())
        {
            BackupManager.Clear();
            Velvet.Error("some of your game files have been tampered with! Make sure they're the original files!");
            return SetupResult.BackupFail;
        }
        Velvet.Info("creating level pack from game...");
        CreateLevelPack();
        Velvet.Info("finished setup!");
        return SetupResult.Success;
    }
    public void Reset()
    {
        BackupManager.Clear();
        ModDB.Clear();
        if(File.Exists(CONFIG_NAME))
            File.Delete(CONFIG_NAME);
        if(Directory.Exists(LEVELS_NAME))
            Directory.Delete(LEVELS_NAME);
    }
    private void CreateLevelPack()
    {
        string levelsgfs = BackupManager.GetBackupPath("levels.gfs");
        string levels = GFS.Utils.Extract(levelsgfs);
        LevelPack pack = LevelPack.Read(Path.Combine(levels,"temp","levels"));
        pack.Save(levelsPath);
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
        if(game.ExistsGameNews() && game.IsClient())
            BackupManager.MakeBackup(game.GetGameNewsFile());
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
    private void ApplyLevels(Game game)
    {
        string temp = FileSystem.CreateTempFolder();
        string tempLevels = Path.Combine(temp,"temp","levels");
        Directory.CreateDirectory(tempLevels);
        List<LevelPack> modpacks = [];
        foreach(Mod mod in ModDB.EnabledMods)
        {
            LevelPack? modpack = mod.GetLevelPack();
            if(modpack == null) continue;
            modpacks.Add(modpack);
        }
        if(modpacks.Count == 0) return;
        LevelPack.Combine(tempLevels,[GetLevelPack(),..modpacks]).Save();
        string levelsgfs = Path.Combine(game.GetData01Folder(),"levels.gfs");
        RevergePackage levels = RevergePackage.Create(temp);
        Writer writer = new(levelsgfs);
        writer.Write(levels);
    }
    private void ApplyMods(Game game)
    {
        if(game.ExistsGameNews())
        {
            List<News> gamenews = [
                new(
                    1,
                    Velvet.Velvetify($"{ModDB.EnabledMods.Count} mods loaded"),
                    Velvet.GAMENEWS_MODLIST_FILENAME,
                    Velvet.Velvetify(Velvet.NAME),
                    Velvet.Velvetify("this game has been modified, you may experience unstable/broken sessions"),
                    Velvet.GITHUB_PROJECT_URL
                ),
                ..GetGameNews()
            ];
            News.WriteGameNews(Client.GetGameNewsFile(),gamenews);
            string gamenews_modlist_path = Path.Combine(Client.GetGameNewsImageFolder(),Velvet.GAMENEWS_MODLIST_FILENAME);
            if(File.Exists(gamenews_modlist_path))
                File.Delete(gamenews_modlist_path);
            FileStream gamenews_modlist = File.OpenWrite(gamenews_modlist_path);
            Dotnet.GetGameNewsModsListResource().CopyTo(gamenews_modlist);
            gamenews_modlist.Close();
        }
        if(game.ExistsData01Folder())
        {
            ApplyLevels(game);
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
                if(File.Exists(file))
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
    public ModInstallResult InstallStream(Stream stream)
    {
        Velvet.Info("installing Mod from stream...");
        return InstallFolder(ArchiveUtils.ExtractArchive(stream));
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
    public LevelPack GetLevelPack()
    {
        return LevelPack.Read(levelsPath);
    }
    public List<News> GetGameNews()
    {
        return News.ReadGameNews(gamenewsPath);
    }
}