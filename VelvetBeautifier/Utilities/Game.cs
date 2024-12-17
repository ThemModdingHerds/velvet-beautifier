using System.Diagnostics;
using ThemModdingHerds.VelvetBeautifier.GameNews;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Game(string folder,string name)
{
    public const string CLIENT_NAME = "Them's Fightin' Herds";
    public const string SERVER_NAME = "LobbyExe";
    public string Folder {get;} = folder;
    public string Name {get;} = name;
    public string Executable {get;} = Path.Combine(folder,name + FileSystem.ExecutableExtension);
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
    public bool Valid()
    {
        bool flag = ExistsExecutable();
        if(IsClient())
            flag = ExistsData01Folder() && ExistsTFHResourcesFolder();
        if(IsServer())
            flag = ExistsTFHResourcesFolder();
        return flag;
    }
    public bool IsClient()
    {
        return Name == CLIENT_NAME;
    }
    public bool IsServer()
    {
        return Name == SERVER_NAME;
    }
    // TFHResource
    public string GetTFHResourcesFolder()
    {
        return Path.Combine(Folder,"Scripts","src","Farm","resources");
    }
    public bool ExistsTFHResourcesFolder() => Directory.Exists(GetTFHResourcesFolder());
    public List<Checksum> GetTFHResourcesChecksums() => ExistsTFHResourcesFolder() ? [] : ChecksumsTFH.FetchSync()?.TFHResources ?? [];
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
    public string GetGameNewsFile() => Path.Combine(Folder,News.GAMENEWS_FILEPATH);
    public Checksum? GetGameNewsChecksum() => ExistsGameNews() ? ChecksumsTFH.FetchSync()?.GameNews : null;
    public string GetGameNewsImageFolder() => Path.Combine(Folder,News.GAMENEWS_IMAGE_FOLDER);
    public bool ExistsGameNews() => File.Exists(GetGameNewsFile()) && Directory.Exists(GetGameNewsImageFolder());
    public List<News> ReadGameNews() => News.ReadGameNews(GetGameNewsFile());
}