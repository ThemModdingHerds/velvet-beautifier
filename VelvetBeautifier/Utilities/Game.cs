using System.Diagnostics;
using ThemModdingHerds.VelvetBeautifier.GameNews;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Game(string folder,string name)
{
    public const string CLIENT_NAME = "Them's Fightin' Herds";
    public const string SERVER_NAME = "LobbyExe";
    public const string SKULLGIRLS_NAME = "SkullGirls";
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
        return Name == CLIENT_NAME || Name == SKULLGIRLS_NAME;
    }
    public bool IsServer()
    {
        return Name == SERVER_NAME;
    }
    public bool IsSkullGirls()
    {
        return Name == SKULLGIRLS_NAME;
    }
    public string GetTFHResourcesFolder()
    {
        return Path.Combine(Folder,"Scripts","src","Farm","resources");
    }
    public bool ExistsTFHResourcesFolder() => Directory.Exists(GetTFHResourcesFolder());
    public string GetData01Folder()
    {
        return Path.Combine(Folder,"data01");
    }
    public bool ExistsData01Folder() => Directory.Exists(GetData01Folder());
    public List<GameFile> GetData01Files() => IsSkullGirls() ? GameFiles.SkullGirlsData01 : GameFiles.Data01;
    public bool ExistsExecutable() => File.Exists(Executable);
    public GameFile GetExecutable() => IsSkullGirls() ? GameFiles.SkullGirlsExecutable : GameFiles.Executable;
    public void Launch()
    {
        if(!ExistsExecutable() && !GetExecutable().Verify(Executable)) return;
        Process.Start(Executable);
    }
    public string GetGameNewsFile() => Path.Combine(Folder,News.GAMENEWS_FILEPATH);
    public string GetGameNewsImageFolder() => Path.Combine(Folder,News.GAMENEWS_IMAGE_FOLDER);
    public bool ExistsGameNews() => File.Exists(GetGameNewsFile()) && Directory.Exists(GetGameNewsImageFolder());
    public List<News> ReadGameNews() => News.ReadGameNews(GetGameNewsFile());
}