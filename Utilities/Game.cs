namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Game(string folder,string name)
{
    public static readonly string CLIENT_NAME = "Them's Fightin' Herds";
    public static readonly string SERVER_NAME = "LobbyExe";
    public string Folder {get;} = folder;
    public string Name {get;} = name;
    public string Executable {get;} = Path.Combine(folder,name);
    public static string? FindGamePath()
    {
        string path;
        List<string> games;
        games = Steam.GetGames();
        path = games.Where((gpath) => gpath.EndsWith(CLIENT_NAME)).ToArray()[0];
        if(Directory.Exists(path))
            return path;

        games = EpicGames.GetGames();
        path = games.Where((gpath) => gpath.EndsWith(CLIENT_NAME)).ToArray()[0];
        if(Directory.Exists(path))
            return path;
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
        return (!IsClient() || ExistsData01Folder()) && ExistsExecutable() && ExistsTFHResourcesFolder();
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
    public bool VerifyTFHResource(string resource,string hash)
    {
        if(!GameFiles.TFHResources.Contains(resource)) return false;
        string path = Path.Combine(GetTFHResourcesFolder(),resource);
        if(!File.Exists(path)) return false;
        return Crypto.Checksum(path,hash);
    }
    public bool ExistsTFHResourcesFolder() => Directory.Exists(GetTFHResourcesFolder());
    public string GetData01Folder()
    {
        return Path.Combine(Folder,"data01");
    }
    public bool VerifyData01(string gfsfile,string hash)
    {
        if(!GameFiles.Data01.Contains(gfsfile)) return false;
        string path = Path.Combine(GetData01Folder(),gfsfile);
        if(!File.Exists(path)) return false;
        return Crypto.Checksum(path,hash);
    }
    public bool ExistsData01Folder() => Directory.Exists(GetData01Folder());
    public bool ExistsExecutable() => File.Exists(Executable);
}