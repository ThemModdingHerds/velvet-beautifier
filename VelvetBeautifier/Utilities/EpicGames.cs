using System.Runtime.InteropServices;
using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class EpicGames
{
    public const string DEFAULT_PATH = "C:\\ProgramData\\Epic\\EpicGamesLauncher\\Data\\Manifests";
    public const string KEYPATH = "HKEY_CURRENT_USER\\Software\\Epic Games\\EOS";
    public static string? GetManifestFolder()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return (string?)Microsoft.Win32.Registry.GetValue(KEYPATH,"ModSdkMetadataDir",DEFAULT_PATH) ?? DEFAULT_PATH;
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
    public static string? GetGamePath()
    {
        List<string> gamepaths = GetGames();
        foreach(string gamepath in gamepaths)
        {
            if(gamepath.EndsWith(Game.GetClientName()))
                return gamepath;
        }
        return null;
    }
}