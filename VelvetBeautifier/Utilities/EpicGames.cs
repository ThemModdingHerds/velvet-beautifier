using System.Runtime.InteropServices;
using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods for Epic Shit, aka Epic Games (this only works on Windows)
/// </summary>
public static class EpicGames
{
    /// <summary>
    /// The default folder path to the manifests, there are JSON files with installed game metadata
    /// </summary>
    public const string DEFAULT_MANIFESTS_FOLDERPATH = "C:\\ProgramData\\Epic\\EpicGamesLauncher\\Data\\Manifests";
    /// <summary>
    /// The Registry key path of the manifests folder location
    /// </summary>
    public const string MANIFEST_KEYPATH = "HKEY_CURRENT_USER\\Software\\Epic Games\\EOS";
    /// <summary>
    /// Get the manifest folder path if it exists
    /// </summary>
    /// <returns>The full folderpath of the manifest folder</returns>
    public static string? GetManifestFolder()
    {
        // only on windows
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return null;
        string folder = (string?)Microsoft.Win32.Registry.GetValue(MANIFEST_KEYPATH,"ModSdkMetadataDir",DEFAULT_MANIFESTS_FOLDERPATH) ?? DEFAULT_MANIFESTS_FOLDERPATH;
        if(!Directory.Exists(folder)) return null;
        return folder;
    }
    /// <summary>
    /// Get the file paths of all manifests
    /// </summary>
    /// <returns>A list of file paths of manifests</returns>
    public static List<string> GetManifests()
    {
        string? manifest = GetManifestFolder();
        if(manifest == null) return [];
        return FileSystem.GetAllFiles(manifest);
    }
    /// <summary>
    /// Get the names of the games installed on Epic Games
    /// </summary>
    /// <returns>A list of game names</returns>
    public static List<string> GetGames()
    {
        List<string> games = [];
        List<string> manifest_paths = GetManifests();
        foreach(string manifest_path in manifest_paths)
        {
            JsonDocument? document = JsonSerializer.Deserialize<JsonDocument>(manifest_path);
            if(document == null) continue;
            JsonElement root = document.RootElement;
            // "MainGameAppName" is the string property containing the game's name
            string? name = root.GetProperty("MainGameAppName").GetString();
            if(name != null)
                games.Add(name);
        }
        return games;
    }
    /// <summary>
    /// Get the folderpath of Them's Fightin' Herds if it exists
    /// </summary>
    /// <returns>The folderpath of the game installed on Epic Games</returns>
    public static string? GetGamePath()
    {
        List<string> gamepaths = GetGames();
        foreach(string gamepath in gamepaths)
        {
            if(gamepath.EndsWith(Game.NAME))
                return gamepath;
        }
        return null;
    }
}