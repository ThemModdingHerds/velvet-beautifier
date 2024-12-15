using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Config
{
    [JsonPropertyName("client_path")]
    public string ClientPath {get; set;} = Game.FindGamePath() ?? "";
    [JsonPropertyName("server_path")]
    public string ServerPath {get; set;} = "";
    [JsonPropertyName("version")]
    public Version Version {get; set;} = Dotnet.LibraryVersion;
    public static Config Read(string path)
    {
        return JsonSerializer.Deserialize<Config>(File.ReadAllText(path)) ?? throw new SerializationException("couldn't read config file");
    }
    public static Config ReadOrCreate(string path)
    {
        if(!File.Exists(path))
            Create(path);
        return Read(path);
    }
    public static void Create(string path)
    {
        Write(path,new());
    }
    public static void Write(string path,Config config)
    {
        StreamWriter file = File.CreateText(path);
        string json = JsonSerializer.Serialize(config);
        file.Write(json);
        file.Close();
    }
    public void Save(string path)
    {
        Write(path,this);
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
        return GetVersion(path) < Dotnet.LibraryVersion;
    }
    public static Version GetVersion(string path)
    {
        JsonDocument json = JsonDocument.Parse(File.ReadAllText(path));
        JsonElement element = json.RootElement;
        JsonElement versionProp = element.GetProperty("version");
        return new(versionProp.GetString() ?? "0.0.0.0");
    }
}