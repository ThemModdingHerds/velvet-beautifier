using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class Checksum
{
    [JsonPropertyName("name")]
    public string Name {get;set;} = string.Empty;
    [JsonPropertyName("hash")]
    public string Hash {get;set;} = string.Empty;
    [JsonPropertyName("canModify")]
    public bool CanModify {get;set;}
    public bool Verify(string file)
    {
        return Crypto.Checksum(file,Hash);
    }
    public override string ToString()
    {
        return Name;
    }
}
public class ChecksumsBase
{
    [JsonPropertyName("game")]
    public Checksum Game {get;set;} = new();
}
public class ChecksumsZEngine : ChecksumsBase
{
    [JsonPropertyName("data01")]
    public List<Checksum> Data01 {get;set;} = [];
}
public class ChecksumsTFH : ChecksumsZEngine
{
    public const string FILENAME = "checksums.json";
    public static string FilePath => Path.Combine(Velvet.AppDataFolder,FILENAME);
    [JsonPropertyName("tfhres")]
    public List<Checksum> TFHResources {get;set;} = [];
    [JsonPropertyName("gamenews")]
    public Checksum GameNews {get;set;} = new();
    private static bool HasCached()
    {
        return File.Exists(FilePath);
    }
    public static ChecksumsTFH? Fetch()
    {
        return DownloadManager.GetJSON<ChecksumsTFH>(Velvet.GITHUB_CHECKSUMS_TFH_FILE_URL);
    }
    public static ChecksumsTFH? Read(bool redownload = false)
    {
        if(redownload || !HasCached())
        {
            ChecksumsTFH? checksums = Fetch();
            File.WriteAllText(FilePath,JsonSerializer.Serialize(checksums));
            return checksums;
        }
        return JsonSerializer.Deserialize<ChecksumsTFH>(File.ReadAllText(FilePath));
    }
}