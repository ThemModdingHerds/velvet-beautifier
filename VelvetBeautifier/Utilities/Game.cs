using System.Diagnostics;
using System.Runtime.InteropServices;
using ThemModdingHerds.VelvetBeautifier.GameNews;

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
        throw new Exception("not supported");
    }
    public static string GetServerName()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return "LobbyExe.exe";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return "LobbyServer.Linux.x64";
        throw new Exception("not supported");
    }
    public bool Valid()
    {
        if(!ExistsExecutable())
            return false;
        if(IsClient() && !(ExistsData01Folder() && ExistsTFHResourcesFolder()))
            return false;
        if(IsServer() && !ExistsTFHResourcesFolder())
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
    // TFHResource
    public string GetTFHResourcesFolder()
    {
        return Path.Combine(Folder,"Scripts","src","Farm","resources");
    }
    public bool ExistsTFHResourcesFolder() => Directory.Exists(GetTFHResourcesFolder());
    public List<Checksum> GetTFHResourcesChecksums() => ExistsTFHResourcesFolder() ? ChecksumsTFH.FetchSync()?.TFHResources ?? [] : [];
    // Data01
    public string GetData01Folder()
    {
        return Path.Combine(Folder,"data01");
    }
    public bool ExistsData01Folder() => Directory.Exists(GetData01Folder());
    public List<Checksum> GetData01Checksums() => ExistsData01Folder() ? ChecksumsTFH.FetchSync()?.Data01 ?? [] : [];
    // Executable
    public bool ExistsExecutable() => File.Exists(Executable);
    public Checksum? GetExecutableChecksum() => ExistsExecutable() ? ChecksumsTFH.FetchSync()?.Game : null;
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
    // GameNews
    public string GetGameNewsFile() => Path.Combine(Folder,News.GAMENEWS_FILEPATH);
    public Checksum? GetGameNewsChecksum() => ExistsGameNews() ? ChecksumsTFH.FetchSync()?.GameNews : null;
    public string GetGameNewsImageFolder() => Path.Combine(Folder,News.GAMENEWS_IMAGE_FOLDER);
    public bool ExistsGameNews() => File.Exists(GetGameNewsFile()) && Directory.Exists(GetGameNewsImageFolder());
    public List<News> ReadGameNews() => News.ReadGameNews(GetGameNewsFile());
}