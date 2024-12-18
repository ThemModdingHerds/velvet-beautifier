using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.Levels;
using ThemModdingHerds.TFHResource;
using ThemModdingHerds.TFHResource.Data;
using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.GitHub;
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
    public Game? Client {get;}
    public Game? Server {get;}
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

        if(Config.ExistsGameFolder())
            Client = new Game(Config.ClientPath,Game.GetServerName());
        if(Config.ExistsServerFolder())
            Server = new Game(Config.ServerPath,Game.GetServerName());
        Setup();
        CommandLine.Process();
    }
    public static bool IsOutdated()
    {
        Task<GitHubRelease?> task = GitHubRelease.Fetch();
        task.Wait();
        GitHubRelease? release = task.Result;
        if(release == null) return false;
        return release.Version > Dotnet.LibraryVersion;
    }
    private bool HasBackups()
    {
        if(Client != null && Client.Valid() && !HasBackups(Client))
            return false;
        if(Server != null && Server.Valid() && !HasBackups(Server))
            return false;
        return true;
    }
    private bool HasBackups(Game game)
    {
        if(game.ExistsData01Folder())
        foreach(Checksum gameFile in game.GetData01Checksums())
            if(!BackupManager.ExistsBackup(gameFile.Name))
                return false;
        if(game.ExistsTFHResourcesFolder())
        foreach(Checksum gameFile in game.GetTFHResourcesChecksums())
            if(!BackupManager.ExistsBackup(gameFile.Name))
                return false;
        if(game.ExistsGameNews())
            if(!BackupManager.ExistsBackup(game.GetGameNewsFile()))
                return false;
        return true;
    }
    private bool SetupNotRequired()
    {
        return HasBackups() &&
               Directory.Exists(ModDB.Folder) &&
               File.Exists(configPath) &&
               Directory.Exists(levelsPath) &&
               File.Exists(gamenewsPath);
    }
    public void Setup()
    {
        if(Config.IsOld(CONFIG_NAME))
            Velvet.Error("your config file is old! It might not work correctly");
        if(SetupNotRequired())
            return;
        Velvet.Info("setting up the environment...");
        BackupGameFiles();
        Velvet.Info("creating level pack from game...");
        CreateLevelPack();
        Velvet.Info("finished setup!");
    }
    public void Reset()
    {
        BackupManager.Clear();
        ModDB.Clear();
        if(File.Exists(CONFIG_NAME))
            File.Delete(CONFIG_NAME);
        if(Directory.Exists(LEVELS_NAME))
            Directory.Delete(LEVELS_NAME,true);
    }
    private void CreateLevelPack()
    {
        string levelsgfs = BackupManager.GetBackupPath("levels.gfs");
        if(!File.Exists(levelsgfs))
        {
            Velvet.Error("couldn't create level pack from level.gfs because there's no backup of it");
            return;
        }
        string levels = GFS.Utils.Extract(levelsgfs);
        LevelPack pack = LevelPack.Read(Path.Combine(levels,"temp","levels"));
        pack.Save(levelsPath);
    }
    private void BackupGameFiles()
    {
        if(HasBackups()) return;
        if(Client != null && Client.Valid())
            BackupGameFiles(Client);
        if(Server != null && Server.Valid())
            BackupGameFiles(Server);
    }
    private void BackupGameFiles(Game game)
    {
        void BackupFile(string file,Checksum? gameFile)
        {
            if(gameFile == null) return;
            if(!gameFile.Verify(file))
                Velvet.Error($"{gameFile.Name} has been tampered with! It might cause problems");
            if(!BackupManager.ExistsBackup(file))
                Velvet.Info($"creating backup of {gameFile.Name}");
            BackupManager.MakeBackup(file);
        }
        void Backup(string folder,IEnumerable<Checksum> gameFiles)
        {
            foreach(Checksum gameFile in gameFiles)
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupFile(file,gameFile);
            }
        }
        if(game.ExistsData01Folder())
            Backup(game.GetData01Folder(),game.GetData01Checksums());
        if(game.ExistsTFHResourcesFolder())
            Backup(game.GetTFHResourcesFolder(),game.GetTFHResourcesChecksums());
        if(game.ExistsGameNews())
            BackupFile(game.GetGameNewsFile(),game.GetGameNewsChecksum());
    }
    public void ApplyMods()
    {
        Velvet.Info($"applying {ModDB.EnabledMods.Count} mods...");
        if(Client != null && Client.Valid())
            ApplyMods(Client);
        if(Server != null && Server.Valid())
            ApplyMods(Server);
        Velvet.Info("mods have been applied!");
    }
    public void Revert()
    {
        Velvet.Info("reverting files back to vanilla game...");
        if(Client != null && Client.Valid())
            Revert(Client);
        if(Server != null && Server.Valid())
            Revert(Server);
    }
    private void Revert(Game game)
    {
        Velvet.Info($"reverting game files from {game.Name}");
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(Checksum gameFile in game.GetData01Checksums())
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(Checksum gameFile in game.GetTFHResourcesChecksums())
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
            }
        }
        if(game.ExistsGameNews())
        {
            string file = game.GetGameNewsFile();
            BackupManager.Revert(file);
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
        int count = (from modpack in modpacks select modpack.Worlds.Entries.Count).Aggregate((a,b) => a + b);
        Velvet.Info($"applying {count} levels...");
        LevelPack vanilla = ReadVanillaLevelPack();
        vanilla.Add([..modpacks]);
        vanilla.Save(tempLevels);
        string levelsgfs = Path.Combine(game.GetData01Folder(),"levels.gfs");
        RevergePackage levels = RevergePackage.Create(temp);
        RevergePackage modded = RevergePackage.Open(levelsgfs);
        levels.Merge(modded);
        if(File.Exists(levelsgfs))
            File.Delete(levelsgfs);
        Writer writer = new(levelsgfs);
        writer.Write(levels);
    }
    private void ApplyMods(Game game)
    {
        Velvet.Info($"applying mods to {game.Name}...");
        if(game.ExistsGameNews())
        {
            string file = game.GetGameNewsFile();
            BackupManager.Revert(file);
            News modStats = new(
                1,
                Velvet.Velvetify($"{ModDB.EnabledMods.Count} mods loaded"),
                Velvet.GAMENEWS_MODLIST_FILENAME,
                Velvet.Velvetify(Velvet.NAME),
                Velvet.Velvetify("this game has been modified, you may experience unstable/broken sessions"),
                Velvet.GITHUB_PROJECT_URL
            );
            List<News> gamenews = [];
            gamenews.Add(modStats);
            gamenews.AddRange(GetGameNews());
            News.WriteGameNews(file,gamenews);
            string gamenews_modlist_path = Path.Combine(game.GetGameNewsImageFolder(),Velvet.GAMENEWS_MODLIST_FILENAME);
            if(File.Exists(gamenews_modlist_path))
                File.Delete(gamenews_modlist_path);
            FileStream gamenews_modlist = File.OpenWrite(gamenews_modlist_path);
            Dotnet.GetGameNewsModsListResource().CopyTo(gamenews_modlist);
            gamenews_modlist.Close();
        }
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(Checksum gameFile in game.GetData01Checksums())
            {
                if(!gameFile.CanModify)
                    Velvet.Error($"WARN: {gameFile.Name} cannot be modified! Expect issues launching!");
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
            ApplyLevels(game);
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(Checksum gameFile in game.GetTFHResourcesChecksums())
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
    public async Task<ModInstallResult> InstallGameBananaMod(GameBanana.Argument argument) => await InstallGameBananaMod(argument.GetId());
    public async Task<ModInstallResult> InstallGameBananaMod(string url) => await InstallGameBananaMod(GameBanana.Utils.GetModId(url));
    public async Task<ModInstallResult> InstallMod(string? str)
    {
        if(str == null) return ModInstallResult.Invalid;
        if(int.TryParse(str,out int id))
        {
            Velvet.Info($"installing GameBanana Mod with id '{id}'...");
            return await InstallGameBananaMod(id);
        }
        if(GameBanana.Utils.ValidUrl(str))
        {
            Velvet.Info($"installing GameBanana Mod with url '{str}'");
            return await InstallGameBananaMod(str);
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
        Mod? mod = ModDB.FindModById(id);
        if(mod == null)
        {
            Velvet.Error($"no Mod with id '{id}' exists!");
            return;
        }
        Velvet.Info($"disabling Mod id '{id}'...");
        mod.Disable();
    }
    public LevelPack ReadVanillaLevelPack()
    {
        return LevelPack.Read(levelsPath);
    }
    public List<News> GetGameNews()
    {
        return News.ReadGameNews(gamenewsPath);
    }
}