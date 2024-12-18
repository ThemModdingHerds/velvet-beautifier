using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GitHub;
public class GitHubRelease
{
    public const string API_URL = $"https://api.github.com/repos/{Velvet.GITHUB_OWNER}/{Velvet.GITHUB_REPO}/releases";
    public const string API_LATEST_URL = $"{API_URL}/latest";
    [JsonPropertyName("tag_name")]
    public string TagName {get;set;} = string.Empty;
    [JsonIgnore]
    public string? BuildType => TagName[(TagName.IndexOf('-')+1)..];
    [JsonIgnore]
    public Version Version => new(TagName[..TagName.IndexOf('-')]);
    [JsonPropertyName("assets")]
    public List<GitHubReleaseAsset> Assets {get;set;} = [];
    public static async Task<GitHubRelease?> Fetch()
    {
        return await DownloadManager.GetJSON<GitHubRelease>(API_LATEST_URL);
    }
    public bool IsDevBuild()
    {
        return BuildType == "dev";
    }
    private GitHubReleaseAsset GetAsset(string type)
    {
        string name = $"VelvetBeautifier.{type}.{TagName}.";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            name += "Windows";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            name += "Linux";
        if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            name += "MacOS";
        foreach(GitHubReleaseAsset asset in Assets)
        {
            if(asset.Name == name)
                return asset;
        }
        throw new FileNotFoundException($"No asset for ${name}");
    }
    public GitHubReleaseAsset GetCLIAsset() => GetAsset("CLI");
    public GitHubReleaseAsset GetGUIAsset() => GetAsset("GUI");
}
public class GitHubReleaseAsset
{
    [JsonPropertyName("name")]
    public string Name {get;set;} = string.Empty;
    [JsonPropertyName("browser_download_url")]
    public string BrowserDownloadUrl {get;set;} = string.Empty;
}