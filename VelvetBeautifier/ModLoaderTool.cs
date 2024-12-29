using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.GFS;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Levels;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Patches;
using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public static class ModLoaderTool
{
    public static Client? Client {get;private set;}
    public static Server? Server {get;private set;}
    public static bool Outdated {get;private set;} = false;
    public static bool IsOutdated()
    {
        GitHubRelease? release = GitHubRelease.Fetch();
        if(release == null) return false;
        return release.Version > Dotnet.LibraryVersion;
    }
    private static bool HasBackups()
    {
        if(Client != null && (!Client.Valid() || !HasBackups(Client)))
            return false;
        if(Server != null && (!Server.Valid() || !HasBackups(Server)))
            return false;
        return true;
    }
    private static bool HasBackups(Game game)
    {
        if(game.HasData01)
        foreach(Checksum checksum in RevergePackageManager.Checksums)
            if(!BackupManager.ExistsBackup(checksum.Name))
                return false;
        if(game.HasResources)
        foreach(Checksum checksum in TFHResourceManager.Checksums)
            if(!BackupManager.ExistsBackup(checksum.Name))
                return false;
        if(GameNewsManager.HasFile(game))
            if(!BackupManager.ExistsBackup(GameNewsManager.GetFilePath(game)))
                return false;
        return true;
    }
    private static bool SetupRequired()
    {
        return !HasBackups() ||
               !Directory.Exists(ModDB.Folder) ||
               !File.Exists(Config.FilePath) ||
               !Directory.Exists(LevelManager.Folder);
    }
    public static void Run()
    {
        // create main directory for storing things
        Directory.CreateDirectory(Velvet.AppDataFolder);
        Migrate();
        // Check if the user is using an old version of VB
        if(Outdated = IsOutdated())
            Velvet.Warn($"You are using an old version of {Velvet.NAME}! Update to the latest release for better support!");
        // some branding things, version output etc :)
        Velvet.Info($"{Velvet.NAME} v{Dotnet.LibraryVersion}\n\nA Mod Loader/Tool for Them's Fightin' Herds");
        // this either creates a config or loads an already existing one
        Config.Init();
        // check if using an old config file
        if(Config.IsOld(Config.FilePath))
            Velvet.Warn("your config file is old! It might not work correctly");
        // create the game instances which we can modify
        if(Config.ClientPath != null)
            Client = new Client(Config.ClientPath);
        if(Config.ServerPath != null)
            Server = new Server(Config.ServerPath);
        // Setup is required for first start or when there are no backups, mods folder, config file or levels folder
        if(SetupRequired())
        {
            Velvet.Info("setting up the environment...");
            // get file information of the client/server
            ChecksumsTFH.Read(true);
            // create the mods folder
            ModDB.Init();
            // create backup of important files (*.gfs, *.tfhres files)
            BackupGameFiles();
            Velvet.Info("finished setup!");
            return;
        }
        // make the managers use those file information
        InitManagers();
        // Process command line
        CommandLine.Process();
    }
    public static void Reset()
    {
        // Yep delete everything as if nothing happened
        if(Directory.Exists(Velvet.AppDataFolder))
            Directory.Delete(Velvet.AppDataFolder,true);
    }
    private static void Migrate()
    {
        // before 2.2.1: move local backup/mods/config to appdata folder
        string backup = Path.Combine(Dotnet.ExecutableFolder,BackupManager.FOLDERNAME);
        if(Directory.Exists(backup))
            Directory.Move(backup,BackupManager.Folder);
        string mods = Path.Combine(Dotnet.ExecutableFolder,ModDB.FOLDERNAME);
        if(Directory.Exists(mods))
            Directory.Move(mods,ModDB.Folder);
        string config = Path.Combine(Dotnet.ExecutableFolder,Config.FILENAME);
        if(File.Exists(config))
            File.Move(config,Config.FilePath);
    }
    private static void InitManagers()
    {
        RevergePackageManager.Init();
        TFHResourceManager.Init();
        GameNewsManager.Init();
    }
    private static void BackupGameFiles()
    {
        if(HasBackups()) return;
        if(Client != null && Client.Valid())
            BackupGameFiles(Client);
        if(Server != null && Server.Valid())
            BackupGameFiles(Server);
    }
    private static void BackupGameFiles(Game game)
    {
        RevergePackageManager.CreateBackup(game);
        LevelManager.Create();
        TFHResourceManager.CreateBackup(game);
        GameNewsManager.CreateBackup(game);
    }
    public static void ApplyMods()
    {
        Velvet.Info($"applying {ModDB.EnabledMods.Count} mods...");
        if(Client != null && Client.Valid())
            ApplyMods(Client);
        if(Server != null && Server.Valid())
            ApplyMods(Server);
        Velvet.Info("mods have been applied!");
    }
    public static void Revert()
    {
        Velvet.Info("reverting files back to vanilla game...");
        if(Client != null && Client.Valid())
            Revert(Client);
        if(Server != null && Server.Valid())
            Revert(Server);
    }
    private static void Revert(Game game)
    {
        Velvet.Info($"reverting game files from {game.ExecutableName}");
        RevergePackageManager.Revert(game);
        TFHResourceManager.Revert(game);
        GameNewsManager.Revert(game);
        PatchManager.Revert(game);
    }
    private static void ApplyMods(Game game)
    {
        Velvet.Info($"applying mods to {game.ExecutableName}...");
        RevergePackageManager.Apply(game);
        LevelManager.Apply(game);
        TFHResourceManager.Apply(game);
        GameNewsManager.Apply(game);
        PatchManager.Apply(game);
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
            Velvet.Info($"extracting TFHResource at {inputPath} to {outputPath}...");
            TFHResource.Utils.Extract(inputPath,outputPath);
            return;
        }
    }
}