using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThemModdingHerds.VelvetBeautifier;
public static class GameBanana
{
    public static async void HandleCommandLine(string line)
    {
        string[] args = line.Split(",");
        // 0. url
        // 1. itemtype
        // 2. id
        try
        {
            string url = args[0];
            int id = int.Parse(args[2]);
            GameBananaMod mod = await GameBananaMod.Fetch(id) ?? throw new Exception("Couldn't fetch mod");
            new DownloadForm(mod,url).Show();
        }
        catch(Exception err)
        {
            throw new VelvetException("GameBanana.HandleCommandLine",err.ToString());
        }
    }
    public static bool ValidUrl(string url)
    {
        return url.StartsWith("http://gamebanana.com") ||
                url.StartsWith("https://gamebanana.com") ||
                url.StartsWith("http://www.gamebanana.com") ||
                url.StartsWith("https://www.gamebanana.com");
    }
    public static int GetModId(string url)
    {
        if(!ValidUrl(url) && !url.Contains("/mods/")) return -1;
        try
        {
            string id_s = url[(url.IndexOf("mods/") + 5)..];
            if(id_s.Contains('/'))
                id_s = id_s[id_s.IndexOf('/')..];
            return int.Parse(id_s);
        }
        catch{}
        return -1;
    }
    public static string CreateCoreItemDataRequestUrl(string itemtype,int id,List<string> fields)
    {
        string field = string.Join(',',fields);
        return $"https://api.gamebanana.com/Core/Item/Data?itemtype={itemtype}&itemid={id}&fields={field}&format=json&return_keys=true";
    }
    public static async Task<List<string>> GetDownloads(int id)
    {
        List<string> urls = [];
        string url = CreateCoreItemDataRequestUrl("Mod",id,["Files().aFiles()"]);
        JsonDocument? jsonObject = await DownloadManager.GetJSON<JsonDocument>(url);
        if(jsonObject == null) return [];

        foreach(JsonElement item in jsonObject.RootElement.EnumerateArray())
        foreach(JsonProperty downloadInfo in item.EnumerateObject())
        {
            JsonElement data = downloadInfo.Value;
            JsonElement downloadUrlObject = data.GetProperty("_sDownloadUrl");
            string? downloadUrl = downloadUrlObject.GetString();
            if(downloadUrl == null) continue;
            urls.Add(downloadUrl);
        }

        return urls;
    }
}
public class GameBananaError
{
    [JsonPropertyName("error")]
    public string Error {get;set;} = "";
    [JsonPropertyName("error_code")]
    public string ErrorCode {get;set;} = "";
}
public class GameBananaModFile
{
    [JsonPropertyName("_sFile")]
    public string Filename {get;set;} = "";
    [JsonPropertyName("_nFilesize")]
    public int Filesize {get;set;}
    [JsonPropertyName("_sDownloadUrl")]
    public string DownloadUrl {get;set;} = "";
    [JsonPropertyName("_sDescription")]
    public string Description {get;set;} = "";
    [JsonPropertyName("_tsDateAdded")]
    public int DateAdded {get;set;}
    [JsonPropertyName("_nDownloadCount")]
    public int DownloadCount {get;set;}
}
public class GameBananaMod
{
    [JsonPropertyName("name")]
    public string ModName {get;set;} = "";
    [JsonPropertyName("userid")]
    public int OwnerId {get;set;}
    [JsonPropertyName("Owner().name")]
    public string OwnerName {get;set;} = "";
    [JsonPropertyName("text")]
    public string Body {get;set;} = "";
    [JsonPropertyName("Files().aFiles()")]
    public Dictionary<string,GameBananaModFile> Files {get;set;} = [];
    public static async Task<GameBananaMod?> Fetch(int id)
    {
        string url = GameBanana.CreateCoreItemDataRequestUrl("Mod",id,[
            "name",
            "userid",
            "Owner().name",
            "text",
            "Files().aFiles()"
        ]);
        return await DownloadManager.GetJSON<GameBananaMod>(url);
    }
}