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
    public static string FilePath => Path.Combine(Dotnet.ExecutableFolder,FILENAME);
    [JsonPropertyName("tfhres")]
    public List<Checksum> TFHResources {get;set;} = [];
    [JsonPropertyName("gamenews")]
    public Checksum GameNews {get;set;} = new();
    private static bool HasCached()
    {
        return File.Exists(FilePath);
    }
    public static async Task<ChecksumsTFH?> Fetch(bool force = false)
    {
        if(HasCached() && !force)
        {
            return JsonSerializer.Deserialize<ChecksumsTFH>(File.ReadAllText(FilePath));
        }
        ChecksumsTFH? checksums = await DownloadManager.GetJSON<ChecksumsTFH>(Velvet.GITHUB_CHECKSUMS_TFH_FILE_URL);
        if(HasCached())
            File.Delete(FilePath);
        File.WriteAllText(FilePath,JsonSerializer.Serialize(checksums));
        return checksums;
    }
    public static ChecksumsTFH? FetchSync(bool force = false)
    {
        Task<ChecksumsTFH?> task = Fetch(force);
        task.Wait();
        return task.Result;
    }
}