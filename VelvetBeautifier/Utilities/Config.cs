using System.Text.Json;
using System.Text.Json.Serialization;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods and fields for reading/writing configuration files
/// </summary>
public static class Config
{
    /// <summary>
    /// Represents a invalid version, aka default value of version
    /// </summary>
    public const int INVALID_VERSION = -1;
    /// <summary>
    /// The current Version of the configuration file, increase these values if there are important changes to the config file
    /// </summary>
    public const int VERSION = 1;
    /// <summary>
    /// The filename of the configuration file
    /// </summary>
    public const string FILENAME = "config.json";
    /// <summary>
    /// The full filepath of the configuration file, should be next to the executable
    /// </summary>
    public static string FilePath => Path.Combine(Velvet.AppDataFolder,FILENAME);
    /// <summary>
    /// The full path of the client's installation folder
    /// </summary>
    public static string? ClientPath {get; set;}
    /// <summary>
    /// The full path of the server's installation folder
    /// </summary>
    public static string? ServerPath {get; set;}
    /// <summary>
    /// The version of this configuration file
    /// </summary>
    public static int Version {get; set;} = VERSION;
    /// <summary>
    /// Read the configuration file at <c>path</c>
    /// </summary>
    /// <param name="path">A filepath to a configuration file</param>
    public static void Read(string path)
    {
        JSON? config = JSON.Read(path);
        ClientPath = config?.ClientPath;
        ServerPath = config?.ServerPath;
        Version = config?.Version ?? INVALID_VERSION;
    }
    /// <summary>
    /// Upon launch, this method is execute which creates a config file if it doesn't exist
    /// </summary>
    public static void Init()
    {
        if(File.Exists(FilePath))
        {
            Read(FilePath);
            return;
        }
        JSON config = JSON.Create(FilePath);
        config.Write(FilePath);
        Read(FilePath);
    }
    public static void Write(string path)
    {
        JSON config = new()
        {
            ClientPath = ClientPath,
            ServerPath = ServerPath,
            Version = Version
        };
        config.Write(path);
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
            return INVALID_VERSION;
        }
    }
    public class JSON
    {
        [JsonPropertyName("client_path")]
        public string? ClientPath {get; set;}
        [JsonPropertyName("server_path")]
        public string? ServerPath {get; set;}
        [JsonPropertyName("version")]
        public int Version {get; set;} = INVALID_VERSION;
        public static JSON? Read(string filepath)
        {
            return JsonSerializer.Deserialize<JSON>(File.ReadAllText(filepath));
        }
        public static JSON Create(string filepath)
        {
            JSON config = new()
            {
                ClientPath = Client.FoundPath,
                Version = VERSION
            };
            config.Write(filepath);
            return config;
        }
        public void Write(string filepath) => File.WriteAllText(filepath,JsonSerializer.Serialize(this));
    }
}