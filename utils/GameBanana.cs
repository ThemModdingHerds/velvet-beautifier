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
    static readonly DateTime epoch = new(1970,1,1);
    [JsonPropertyName("_sFile")]
    public string Filename {get;set;} = "";
    [JsonPropertyName("_nFilesize")]
    public int Filesize {get;set;}
    [JsonPropertyName("_sDownloadUrl")]
    public string DownloadUrl {get;set;} = "";
    [JsonPropertyName("_sDescription")]
    public string Description {get;set;} = "";
    [JsonPropertyName("_tsDateAdded")]
    public int DateAddedInt {get;set;}
    [JsonIgnore]
    public DateTime DateAdded {get => epoch.AddSeconds(DateAddedInt);}
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
    public GameBananaModFile GetLatestUpdate()
    {
        GameBananaModFile newest = Files.Values.ToArray().Last();
        foreach(GameBananaModFile file in Files.Values)
        {
            if(DateTime.Compare(newest.DateAdded,file.DateAdded) < 0)
                newest = file;
        }
        return newest;
    }
}