using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.GFS;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Levels;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public static class ModLoaderTool
{
    public static Config Config {get;} = Config.Init();
    public static Game? Client {get;private set;}
    public static Game? Server {get;private set;}
    public static bool IsOutdated()
    {
        GitHubRelease? release = GitHubRelease.FetchSync();
        if(release == null) return false;
        return release.Version > Dotnet.LibraryVersion;
    }
    private static bool HasBackups()
    {
        if(Client != null && Client.Valid() && !HasBackups(Client))
            return false;
        if(Server != null && Server.Valid() && !HasBackups(Server))
            return false;
        return true;
    }
    private static bool HasBackups(Game game)
    {
        if(RevergePackageManager.HasData01(game))
        foreach(Checksum checksum in RevergePackageManager.Checksums)
            if(!BackupManager.ExistsBackup(checksum.Name))
                return false;
        if(TFHResourceManager.HasResources(game))
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
        Dotnet.ConsoleTitle = Velvet.NAME;
        if(IsOutdated()) Velvet.Warn($"You are using an old version of {Velvet.NAME}! Update to the latest release for better support!");
        Velvet.Info($"{Velvet.NAME} v{Dotnet.LibraryVersion}\n\nA Mod Loader/Tool for Them's Fightin' Herds");

        if(Config.IsOld(Config.FilePath))
            Velvet.Warn("your config file is old! It might not work correctly");
        if(Config.ClientPath != null && Config.ExistsGameFolder())
            Client = new Game(Config.ClientPath,Game.GetClientName());
        if(Config.ServerPath != null && Config.ExistsServerFolder())
            Server = new Game(Config.ServerPath,Game.GetServerName());
        if(SetupRequired())
        {    
            Velvet.Info("setting up the environment...");
            ChecksumsTFH.Read(true);
            ModDB.Init();
            BackupGameFiles();
            Velvet.Info("finished setup!");
        }
        CommandLine.Process();
    }
    public static void Reset()
    {
        BackupManager.Clear();
        ModDB.Clear();
        if(File.Exists(Config.FilePath))
            File.Delete(Config.FilePath);
        if(Directory.Exists(LevelManager.Folder))
            Directory.Delete(LevelManager.Folder,true);
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
        Velvet.Info($"reverting game files from {game.Name}");
        RevergePackageManager.Revert(game);
        TFHResourceManager.Revert(game);
        GameNewsManager.Revert(game);
    }
    private static void ApplyMods(Game game)
    {
        Velvet.Info($"applying mods to {game.Name}...");
        RevergePackageManager.Apply(game);
        LevelManager.Apply(game);
        TFHResourceManager.Apply(game);
        GameNewsManager.Apply(game);
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
}