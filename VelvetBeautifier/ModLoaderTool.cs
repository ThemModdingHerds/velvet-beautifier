using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public static class ModLoaderTool
{
    private const string VERSION_FILENAME = "version";
    private static string VersionFilePath => Path.Combine(Velvet.AppDataFolder,VERSION_FILENAME);
    public static Version Version => File.Exists(VersionFilePath) ? new(File.ReadAllText(VersionFilePath)) : new(0,0,0,0);
    public static Client? Client {get;private set;}
    public static Server? Server {get;private set;}
    private static bool SetupRequired()
    {
        return !BackupManager.Exists() ||
               !ModDB.Exists() ||
               !Config.Exists();
    }
    public static void Init()
    {
        // clear cache on startup
        FileSystem.ClearTempFolders();
        // create main directory for storing things
        Directory.CreateDirectory(Velvet.AppDataFolder);
        Migrate();
        // this either creates a config or loads an already existing one
        Config.Init();
        // check versions
        CheckVersion();
        // some branding things, version output etc :)
        Velvet.Info($"{Velvet.NAME} v{Dotnet.LibraryVersion}\n\nA Mod Loader/Tool for Them's Fightin' Herds");
        // reload just creates Client/Server instances
        Reload();
        // setup is required for first start or when there are no backups, mods folder, config file or levels folder
        if(SetupRequired())
            Setup();
        // verify that the backup files are not modified
        if(BackupManager.Invalid)
            Velvet.Warn("some of your backup files have been modified! Redownload the game files and reset Velvet Beautifier!");
    }
    private static void Setup()
    {
        Velvet.Info("setting up the environment...");
        // get file information of the client/server
        GameFiles.Init();
        // create the mods folder
        ModDB.Init();
        // create backup of important files (*.gfs, *.tfhres files)
        BackupManager.Init();
        Velvet.Info("finished setup!");
    }
    private static void CheckVersion()
    {
        // check if using an old config file
        if(Config.IsOld)
            Velvet.Warn("your config file is old! It might not work correctly");
        // check if the user is using an old version of VB
        if(GitHubRelease.Outdated)
            Velvet.Warn($"You are using an old version of {Velvet.NAME}! Update to the latest release for better support!");
        // check if tool has been updated
        if(Version < Dotnet.LibraryVersion)
        {
            Velvet.Info("detected new version!");
            Setup();
            if(File.Exists(VersionFilePath))
                File.Delete(VersionFilePath);
            File.WriteAllText(VersionFilePath,Dotnet.LibraryVersion.ToString());
        }
    }
    public static void Reload()
    {
        // create the game instances which we can modify
        if(Config.ClientPath != null)
            Client = new Client(Config.ClientPath);
        if(Config.ServerPath != null)
            Server = new Server(Config.ServerPath);
    }
    public static void DeleteEverything()
    {
        // Yep delete everything as if nothing happened
        if(Directory.Exists(Velvet.AppDataFolder))
            Directory.Delete(Velvet.AppDataFolder,true);
    }
    private static void Migrate()
    {
        // before 2.2.1: move local backup/mods/config to appdata folder
        string exeFolder = Dotnet.ExecutableFolder;

        string backup = Path.Combine(exeFolder,BackupManager.FOLDERNAME);
        if(Directory.Exists(backup))
            Directory.Move(backup,BackupManager.Folder);

        string mods = Path.Combine(exeFolder,ModDB.FOLDERNAME);
        if(Directory.Exists(mods))
            Directory.Move(mods,ModDB.Folder);

        string config = Path.Combine(exeFolder,Config.FILENAME);
        if(File.Exists(config))
            File.Move(config,Config.FilePath);
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
