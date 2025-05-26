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
    /// The full filepath of the configuration file
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
    /// Is the current config file a old config file?
    /// </summary>
    public static bool IsOld => GetVersion() < VERSION;
    /// <summary>
    /// Read the configuration file at <c>path</c>
    /// </summary>
    /// <param name="filepath">A filepath to a configuration file</param>
    public static void Read(string filepath)
    {
        JSON? config = JSON.Read(filepath);
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
    /// <summary>
    /// Save changes to the config file
    /// </summary>
    public static void Write()
    {
        JSON config = new()
        {
            ClientPath = ClientPath,
            ServerPath = ServerPath,
            Version = Version
        };
        config.Write(FilePath);
    }
    /// <summary>
    /// Check if <see cref="ClientPath"/> and <see cref="ServerPath"/> are valid
    /// </summary>
    /// <returns></returns>
    public static bool IsValid() => (ClientPath != null && Directory.Exists(ClientPath)) || (ServerPath != null && Directory.Exists(ServerPath));
    /// <summary>
    /// Get the version of the current config file
    /// </summary>
    /// <returns>A number as the version of the config file</returns>
    public static int GetVersion()
    {
        try
        {
            JsonDocument json = JsonDocument.Parse(File.ReadAllText(FilePath));
            JsonElement root = json.RootElement;
            root.TryGetProperty("version", out JsonElement versionProp);
            versionProp.TryGetInt32(out int version);
            return version;
        }
        catch (Exception)
        {
            return INVALID_VERSION;
        }
    }
    /// <summary>
    /// Check if the config file exists
    /// </summary>
    /// <returns>true if the config file exists, otherwise false</returns>
    public static bool Exists() => File.Exists(FilePath);
    /// <summary>
    /// The JSON structure of the config file
    /// </summary>
    public class JSON
    {
        /// <summary>
        /// The full path of the client's installation folder
        /// </summary>
        [JsonPropertyName("client_path")]
        public string? ClientPath { get; set; }
        /// <summary>
        /// The full path of the server's installation folder
        /// </summary>
        [JsonPropertyName("server_path")]
        public string? ServerPath { get; set; }
        /// <summary>
        /// The version of the config file
        /// </summary>
        [JsonPropertyName("version")]
        public int Version { get; set; } = INVALID_VERSION;
        /// <summary>
        /// Read the config file at <c>filepath</c>
        /// </summary>
        /// <param name="filepath">A valid filepath</param>
        /// <returns>The config file, can be null</returns>
        public static JSON? Read(string filepath)
        {
            return JsonSerializer.Deserialize<JSON>(File.ReadAllText(filepath));
        }
        /// <summary>
        /// Create a config file at <c>filepath</c>
        /// </summary>
        /// <param name="filepath">A valid filepath</param>
        /// <returns>The newly created config file</returns>
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
        /// <summary>
        /// Write the config file at <c>filepath</c>
        /// </summary>
        /// <param name="filepath">A valid filepath</param>
        public void Write(string filepath) => File.WriteAllText(filepath, JsonSerializer.Serialize(this));
    }
}