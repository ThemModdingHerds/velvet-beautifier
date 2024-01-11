using System.Text.Json;
using Microsoft.Win32;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class EpicGames
{
    public static string GetManifestFolder()
    {
        string defaultPath = "C:\\ProgramData\\Epic\\EpicGamesLauncher\\Data\\Manifests";
        string keypath = "HKEY_CURRENT_USER\\Software\\Epic Games\\EOS";
        string path = (string?)Registry.GetValue(keypath,"ModSdkMetadataDir",defaultPath) ?? defaultPath;
        if(!Directory.Exists(path)) throw new VelvetException("EpicGames.GetManifestFolder","Invalid path: " + path);
        return path;
    }
    public static List<string> GetManifests()
    {
        return IO.GetAllFiles(GetManifestFolder());
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