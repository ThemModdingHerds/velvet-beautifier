using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using Microsoft.Win32;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Steam
{
    public static string GetInstallPath()
    {
        string x32 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam";
        string x64 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
        string defaultPath = "C:\\Program Files (x86)\\Steam";
        string regPath = Environment.Is64BitOperatingSystem ? x64 : x32;
        string path = (string?)Registry.GetValue(regPath,"InstallPath",defaultPath) ?? defaultPath;
        if(!Directory.Exists(path)) throw new VelvetException("Steam.GetInstallPath","Invalid folder: " + path);
        return path;
    }
    public static string GetLibraryFoldersConfigPath()
    {
        string path = Path.Combine(GetInstallPath(),"config","libraryfolders.vdf");
        if(!File.Exists(path)) throw new VelvetException("Steam.GetLibraryFoldersConfigPath","Invalid file: " + path);
        return path;
    }
    public static List<string> GetLibraryFolders()
    {
        List<string> folders = [];
        VProperty vdf = VdfConvert.Deserialize(File.ReadAllText(GetLibraryFoldersConfigPath())) ?? throw new VelvetException("Steam.GetLibraryFolders","Couldn't get \"ibraryfolders.vdf\" from Steam folder");
        VToken libraryfolders = vdf.Value;
        foreach(VProperty property in libraryfolders.Cast<VProperty>())
        {
            VToken entry = property.Value;
            string path = entry.Value<string>("path");
            if(Directory.Exists(path)) 
                folders.Add(path);
        }
        return folders;
    }
    public static string GetCommonSteamApps(string library)
    {
        if(!Directory.Exists(library)) throw new VelvetException("Steam.GetCommonSteamApps","Invalid folder: " + library);
        string path = Path.Combine(library,"steamapps","common");
        if(!Directory.Exists(path)) throw new VelvetException("Steam.GetCommonSteamApps","Invalid folder: " + path);
        return path;
    }
    public static List<string> GetGames(string library)
    {
        return [..Directory.EnumerateDirectories(GetCommonSteamApps(library))];
    }
    public static List<string> GetGames()
    {
        List<string> games = [];
        foreach(string library in GetLibraryFolders())
        {
            List<string> library_games = GetGames(library);
            games.AddRange(library_games);
        }
        return games;
    }
}