using System.Diagnostics;
using System.Runtime.InteropServices;
using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.GFS;
using ThemModdingHerds.VelvetBeautifier.TFHResource;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Game(string folder,string name)
{
    public string Folder {get;} = folder;
    public string Name {get;} = name;
    public string Executable {get;} = Path.Combine(folder,name);
    public static string? FindGamePath()
    {
        string? path = Steam.GetGamePath();
        if(path != null)
            return path;
        path = EpicGames.GetGamePath();
        if(path != null)
            return path;
        return null;
    }
    public static string GetClientName()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "Them's Fightin' Herds.exe";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "ThemsFightinHerds.Linux.x64";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return "Them's Fighting Herds.app/Contents/MacOS/Them's Fighting Herds";
        return string.Empty;
    }
    public static string GetServerName()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "LobbyExe.exe";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "LobbyServer.Linux.x64";
        return string.Empty;
    }
    public bool Valid()
    {
        bool gfs = RevergePackageManager.HasData01(this);
        bool tfhres = TFHResourceManager.HasResources(this);
        if(!ExistsExecutable())
            return false;
        if(IsClient() && !(gfs && tfhres))
            return false;
        if(IsServer() && !tfhres)
            return false;
        return true;
    }
    public bool IsClient()
    {
        return Name == GetClientName();
    }
    public bool IsServer()
    {
        return Name == GetServerName();
    }
    public string GetScripts()
    {
        return Path.Combine(Folder,"Scripts");
    }
    private bool ExistsExecutable() => File.Exists(Executable);
    private Checksum? GetExecutableChecksum() => ExistsExecutable() ? ChecksumsTFH.FetchSync()?.Game : null;
    public void Launch()
    {
        if(!ExistsExecutable() && (!GetExecutableChecksum()?.Verify(Executable) ?? false)) return;
        Process.Start(new ProcessStartInfo()
        {
            FileName = Executable,
            Verb = "open",
            WorkingDirectory = Path.GetDirectoryName(Executable)
        });
    }
}