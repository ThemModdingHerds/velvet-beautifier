using System.Text.Json;
using System.Text.Json.Serialization;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Config
{
    public static readonly string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory,"config.json");
    [JsonPropertyName("client_path")]
    public string ClientPath {get; set;} = Game.FindGamePath() ?? "";
    [JsonPropertyName("server_path")]
    public string ServerPath {get; set;} = "";
    [JsonPropertyName("version")]
    public Version Version {get; set;} = Dotnet.ExeVersion;
    public static Config Read(string path)
    {
        if(!File.Exists(path))
            Create(path);
        return JsonSerializer.Deserialize<Config>(File.ReadAllText(path)) ?? throw new VelvetException("Config.Read","couldn't read config file");
    }
    public static Config Read()
    {
        return Read(CONFIG_PATH);
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
    public void Save()
    {
        Save(CONFIG_PATH);
    }
    public bool ExistsGameFolder()
    {
        return Directory.Exists(ClientPath);
    }
    public bool ExistsServerFolder()
    {
        return Directory.Exists(ServerPath);
    }
}