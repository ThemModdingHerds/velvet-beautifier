using System.Runtime.InteropServices;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Steam
{
    public const string REGISTRY_KEY32 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam";
    public const string REGISTRY_KEY64 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
    public const string WINDOWS_DEFAULT_PATH = "C:\\Program Files (x86)\\Steam";
    public static string? GetInstallPath()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string regPath = Environment.Is64BitOperatingSystem ? REGISTRY_KEY64 : REGISTRY_KEY32;
            string path = (string?)Microsoft.Win32.Registry.GetValue(regPath,"InstallPath",WINDOWS_DEFAULT_PATH) ?? WINDOWS_DEFAULT_PATH;
            if(!Directory.Exists(path)) return null;
            return path;
        }
        return null;
    }
    public static string? GetLibraryFoldersConfigPath()
    {
        string? install = GetInstallPath();
        if(install == null) return null;
        string path = Path.Combine(install,"config","libraryfolders.vdf");
        if(!File.Exists(path)) return null;
        return path;
    }
    public static List<string> GetLibraryFolders()
    {
        List<string> folders = [];
        string? libraryfoldersconfig = GetLibraryFoldersConfigPath();
        if(libraryfoldersconfig == null) return folders;
        VProperty vdf = VdfConvert.Deserialize(File.ReadAllText(libraryfoldersconfig));
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
    public static string? GetCommonSteamApps(string library)
    {
        if(!Directory.Exists(library)) return null;
        string path = Path.Combine(library,"steamapps","common");
        if(!Directory.Exists(path)) return null;
        return path;
    }
    public static List<string> GetGames(string library)
    {
        string? commonapps = GetCommonSteamApps(library);
        if(commonapps == null) return [];
        return [..Directory.EnumerateDirectories(commonapps)];
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
    public static string? GetGamePath()
    {
        List<string> gamepaths = GetGames();
        string? path = null;
        foreach(string gamepath in gamepaths)
        {
            if(gamepath.EndsWith(Game.CLIENT_NAME))
                return gamepath;
            if(gamepath.EndsWith(Game.SKULLGIRLS_NAME))
                path = gamepath;
        }
        return path;
    }
}