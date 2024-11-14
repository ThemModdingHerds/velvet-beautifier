using System.Runtime.InteropServices;
using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class EpicGames
{
    public static string? GetManifestFolder()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string defaultPath = "C:\\ProgramData\\Epic\\EpicGamesLauncher\\Data\\Manifests";
            string keypath = "HKEY_CURRENT_USER\\Software\\Epic Games\\EOS";
            string path = (string?)Microsoft.Win32.Registry.GetValue(keypath,"ModSdkMetadataDir",defaultPath) ?? defaultPath;
            if(!Directory.Exists(path)) throw new VelvetException("EpicGames.GetManifestFolder","Invalid path: " + path);
            return path;
        }
        return null;
    }
    public static List<string> GetManifests()
    {
        string? manifest = GetManifestFolder();
        if(manifest == null) return [];
        return FileSystem.GetAllFiles(manifest);
    }
    public static List<string> GetGames()
    {
        List<string> games = [];
        List<string> manifest_paths = GetManifests();
        foreach(string manifest_path in manifest_paths)
        {
            JsonDocument? document = JsonSerializer.Deserialize<JsonDocument>(manifest_path);
            if(document == null) continue;
            JsonElement root = document.RootElement;
            string? name = root.GetProperty("MainGameAppName").GetString();
            if(name != null)
                games.Add(name);
        }
        return games;
    }
}