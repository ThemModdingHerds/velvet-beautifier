using System.Text.Json;
using System.Text.Json.Serialization;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Config
{
    public const string FILENAME = "config.json";
    public static string FilePath => Path.Combine(Dotnet.ExecutableFolder,FILENAME);
    // Increase if changes made to this class
    public const int VERSION = 1;
    [JsonPropertyName("client_path")]
    public string ClientPath {get; set;} = Game.FindGamePath() ?? "";
    [JsonPropertyName("server_path")]
    public string ServerPath {get; set;} = "";
    [JsonPropertyName("version")]
    public int Version {get; set;} = VERSION;
    public static Config? Read(string path)
    {
        return JsonSerializer.Deserialize<Config?>(File.ReadAllText(path));
    }
    public static Config ReadOrCreate(string path)
    {
        if(!File.Exists(path))
            return Create(path);
        return Read(path) ?? throw new Exception("impossible");
    }
    public static Config Create(string path)
    {
        Config config = new();
        config.Write(path);
        return config;
    }
    public static Config Init() => ReadOrCreate(FilePath);
    public void Write(string path)
    {
        StreamWriter file = File.CreateText(path);
        string json = JsonSerializer.Serialize(this);
        file.Write(json);
        file.Close();
    }
    public bool ExistsGameFolder()
    {
        return Directory.Exists(ClientPath);
    }
    public bool ExistsServerFolder()
    {
        return Directory.Exists(ServerPath);
    }
    public bool Valid()
    {
        return ExistsGameFolder() || ExistsServerFolder();
    }
    public static bool IsOld(string path)
    {
        return GetVersion(path) < VERSION;
    }
    public static int GetVersion(string path)
    {
        try
        {
            JsonDocument json = JsonDocument.Parse(File.ReadAllText(path));
            JsonElement root = json.RootElement;
            root.TryGetProperty("version",out JsonElement versionProp);
            versionProp.TryGetInt32(out int version);
            return version;
        }
        catch(Exception)
        {
            return -1;
        }
    }
}