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
        List<string> steam = Steam.GetGames();
        if(steam.Count > 0)
        {
            string steampath = steam.Where((gpath) => gpath.EndsWith(CLIENT_NAME)).ToArray()[0];
            if(Directory.Exists(steampath))
                return steampath;
        }

        List<string> epicgames = EpicGames.GetGames();
        if(epicgames.Count > 0)
        {
            string epicpath = epicgames.Where((gpath) => gpath.EndsWith(CLIENT_NAME)).ToArray()[0];
            if(Directory.Exists(epicpath))
                return epicpath;
        }
        return null;
    }
    public static Game? FindClient()
    {
        string? path = FindGamePath();
        if(path == null) return null;
        return new(path,CLIENT_NAME);
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
    public bool ExistsExecutable() => File.Exists(Executable);
}