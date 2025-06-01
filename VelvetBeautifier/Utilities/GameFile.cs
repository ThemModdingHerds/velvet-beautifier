using System.Text.Json;
using System.Text.Json.Serialization;
using ThemModdingHerds.VelvetBeautifier.GitHub;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// This class contains information about a game file (name, hash and if they can be modified)
/// </summary>
public class GameFile
{
    /// <summary>
    /// The filename of the game file
    /// </summary>
    [JsonPropertyName("name")]
    public string Name {get;set;} = string.Empty;
    /// <summary>
    /// The hash of the file for verifications (SHA256)
    /// </summary>
    [JsonPropertyName("hash")]
    public string Hash {get;set;} = string.Empty;
    /// <summary>
    /// Can the file be modified?
    /// </summary>
    [JsonPropertyName("canModify")]
    public bool CanModify {get;set;}
    /// <summary>
    /// Verify if <c>filepath</c> is this game file?
    /// </summary>
    /// <param name="filepath">A valid file path</param>
    /// <returns>true if the file matches, otherwise false</returns>
    public bool Verify(string filepath) => Crypto.Checksum(filepath, Hash);
    public override string ToString() => Name;
}
/// <summary>
/// This class contains game files of the game
/// </summary>
public class GameFiles
{
    /// <summary>
    /// The filename of the local game files information
    /// </summary>
    public const string FILENAME = "checksums.json";
    /// <summary>
    /// The URL to fetch the game file information
    /// </summary>
    public const string FILEURL = $"https://raw.githubusercontent.com/{GitHubUtilities.OWNER}/{GitHubUtilities.CHECKSUM_REPO}/refs/heads/main/tfh.json";
    /// <summary>
    /// The full filepath of the local game files information file
    /// </summary>
    public static string FilePath => Path.Combine(Velvet.AppDataFolder,FILENAME);
    /// <summary>
    /// Information about the executable of the client
    /// </summary>
    [JsonPropertyName("game")]
    public GameFile Game {get;set;} = new();
    /// <summary>
    /// Information about the files in the <c>data01</c> folder
    /// </summary>
    [JsonPropertyName("data01")]
    public List<GameFile> Data01 {get;set;} = [];
    /// <summary>
    /// Information about the files in the <c>resources</c> folder
    /// </summary>
    [JsonPropertyName("tfhres")]
    public List<GameFile> TFHResources {get;set;} = [];
    /// <summary>
    /// Information about the game news file
    /// </summary>
    [JsonPropertyName("gamenews")]
    public GameFile GameNews {get;set;} = new();
    /// <summary>
    /// Does the local game files information file exists?
    /// </summary>
    /// <returns>true if the file exists, otherwise false</returns>
    private static bool Exists() => File.Exists(FilePath);
    /// <summary>
    /// Fetch the game files information file from the internet
    /// </summary>
    /// <returns>Information about the game files or null if the fetch process failed</returns>
    public static GameFiles? Fetch() => DownloadManager.GetJSON<GameFiles>(FILEURL);
    /// <summary>
    /// Initialize the game files information file
    /// </summary>
    public static void Init() => Read(true);
    /// <summary>
    /// Read the local game files information file
    /// </summary>
    /// <param name="redownload">Should the file be (re)downloaded?</param>
    /// <returns>Information about the game files or null if the fetch process fails or JSON parsing fails</returns>
    public static GameFiles? Read(bool redownload = false)
    {
        // check if the file exists or we need to download the file again
        if(redownload || !Exists())
        {
            GameFiles? gamefiles = Fetch();
            if(gamefiles == null) return null;
            // delete already existing one to overwrite
            if(File.Exists(FilePath))
                File.Delete(FilePath);
            File.WriteAllText(FilePath,JsonSerializer.Serialize(gamefiles));
            return gamefiles;
        }
        return JsonSerializer.Deserialize<GameFiles>(File.ReadAllText(FilePath));
    }
}