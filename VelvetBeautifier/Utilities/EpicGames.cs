using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

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
    public class Manifest
    {
        [JsonPropertyName("DisplayName")]
        public string DisplayName {get;set;} = string.Empty;
        [JsonPropertyName("InstallLocation")]
        public string InstallLocation {get;set;} = string.Empty;
    }
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
    public static List<Manifest> GetManifests()
    {
        string? manifestFolder = GetManifestFolder();
        if(manifestFolder == null) return [];
        List<Manifest> manifests = [];
        foreach(string manifestPath in FileSystem.GetAllFiles(manifestFolder,"*.item"))
        {
            Manifest? manifest = FileSystem.ReadJson<Manifest>(manifestPath);
            if(manifest == null) continue;
            manifests.Add(manifest);
        }
        return manifests;
    }
    /// <summary>
    /// Get the folderpath of Them's Fightin' Herds if it exists
    /// </summary>
    /// <returns>The folderpath of the game installed on Epic Games</returns>
    public static string? GetGamePath()
    {
        List<Manifest> manifests = GetManifests();
        foreach(Manifest manifest in manifests)
        {
            if(manifest.DisplayName == Game.NAME)
                return manifest.InstallLocation;
        }
        return null;
    }
}