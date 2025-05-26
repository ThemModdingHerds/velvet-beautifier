using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Various methods for retrieving information from the Steam client
/// </summary>
public static class Steam
{
    /// <summary>
    /// The path to the Registry key of 32-bit Installation (Windows Only)
    /// </summary>
    public const string REGISTRY_KEYPATH32 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam";
    /// <summary>
    /// The path to the Registry key of 64-bit Installation (Windows Only)
    /// </summary>
    public const string REGISTRY_KEYPATH64 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
    /// <summary>
    /// The default path of Steam on Windows
    /// </summary>
    public const string WINDOWS_DEFAULT_FOLDERPATH = "C:\\Program Files (x86)\\Steam";
    public const string LINUX_DEFAULT_FOLDERNAME = ".steam/steam";
    public const string LINUX_CLASSIC_FOLDERNAME = "Steam";
    public const string LINUX_BETA_POSTFIX = "Beta";
    /// <summary>
    /// Get the path of the steam folder. On Windows it uses Registry Keys and falls back to the default path
    /// </summary>
    /// <returns>The path to the Steam installation if it exists</returns>
    public static string? GetInstallPath()
    {
        // On windows use Registry keys or default path as fallback
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // get the correct Registry path depending on architecture
            string regPath = Environment.Is64BitOperatingSystem ? REGISTRY_KEYPATH64 : REGISTRY_KEYPATH32;
            string installPath = (string?)Microsoft.Win32.Registry.GetValue(regPath,"InstallPath",WINDOWS_DEFAULT_FOLDERPATH) ?? WINDOWS_DEFAULT_FOLDERPATH;
            // must be a folder
            if(!Directory.Exists(installPath)) return null;
            return installPath;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // literally from bin_steam.sh
            string steamDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME") ?? Linux.HOME;
            string defaultPath = Path.Combine(steamDataHome, LINUX_DEFAULT_FOLDERNAME);
            string classic = Path.Combine(steamDataHome, LINUX_CLASSIC_FOLDERNAME);
            string betaClassic = classic + LINUX_BETA_POSTFIX;
            if (Directory.Exists(defaultPath))
                return defaultPath;
            if (Directory.Exists(classic))
                return classic;
            if (Directory.Exists(betaClassic))
                return betaClassic;
        }
        // TODO: how to find on MacOS?
        // return nothing if found
        return null;
    }
    /// <summary>
    /// Get the path of the <c>libraryfolders.vdf</c>, it contains the libary folders the user uses for storing Steam games
    /// </summary>
    /// <returns>The path to the <c>libraryfolders.vdf</c> if it exists</returns>
    public static string? GetLibraryFoldersConfigPath()
    {
        string? install = GetInstallPath();
        if(install == null) return null;
        string libraryfolders = Path.Combine(install,"config","libraryfolders.vdf");
        if(!File.Exists(libraryfolders)) return null;
        return libraryfolders;
    }
    /// <summary>
    /// Get all the library folders of Steam
    /// </summary>
    /// <returns>A list of folder paths, may be empty</returns>
    public static List<string> GetLibraryFolders()
    {
        List<string> folders = [];
        // we need this file
        string? libraryfoldersconfig = GetLibraryFoldersConfigPath();
        if(libraryfoldersconfig == null) return folders;
        // read the file
        VProperty vdf = VdfConvert.Deserialize(File.ReadAllText(libraryfoldersconfig));
        VToken libraryfolders = vdf.Value;
        // this file has a array of objects which have a "path" property, the library folder's path
        foreach(VProperty property in libraryfolders.Cast<VProperty>())
        {
            VToken entry = property.Value;
            string libraryfolder = entry.Value<string>("path");
            if(Directory.Exists(libraryfolder)) 
                folders.Add(libraryfolder);
        }
        return folders;
    }
    /// <summary>
    /// Get the folder of <c>folderpath</c> where all Steam games are installed
    /// </summary>
    /// <param name="folderpath">The path to a folder, should be a library</param>
    /// <returns>The folder with subfolders that contain games if it exists</returns>
    public static string? GetCommonSteamApps(string folderpath)
    {
        // must be a folder
        if(!Directory.Exists(folderpath)) return null;
        // here are the games located
        string commonSteamApps = Path.Combine(folderpath,"steamapps","common");
        // must be valid of course
        if(!Directory.Exists(commonSteamApps)) return null;
        return commonSteamApps;
    }
    /// <summary>
    /// Get the paths of all Steam games in <c>folderpath</c>
    /// </summary>
    /// <param name="folderpath">The path to a folder, should be a library</param>
    /// <returns>The full paths to the games in that folder, may be empty</returns>
    public static List<string> GetGames(string folderpath)
    {
        // Get the games folder path
        string? commonapps = GetCommonSteamApps(folderpath);
        if(commonapps == null) return [];
        // Get the subfolders of this folder
        return [..Directory.EnumerateDirectories(commonapps)];
    }
    /// <summary>
    /// Get the paths of all Steam games, this will get them all
    /// </summary>
    /// <returns>The full paths to the games, may be empty</returns>
    public static List<string> GetGames()
    {
        List<string> games = [];
        // go through each library and get their game folders
        foreach(string library in GetLibraryFolders())
        {
            List<string> library_games = GetGames(library);
            games.AddRange(library_games);
        }
        return games;
    }
    /// <summary>
    /// Find the path to Them's Fightin' Herds installation
    /// </summary>
    /// <returns>The installation folder of Them's Fightin' Herds if it exists</returns>
    public static string? GetGamePath()
    {
        // Get all steam games
        List<string> gamepaths = GetGames();
        foreach(string gamepath in gamepaths)
        {
            // if the game is Them's Fightin' Herds
            if(gamepath.EndsWith(Game.NAME))
                // found it
                return gamepath;
        }
        return null;
    }
}