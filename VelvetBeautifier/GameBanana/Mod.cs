using System.Text.Json.Serialization;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GameBanana;

public class Mod
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
    public Dictionary<string,ModFile> Files {get;set;} = [];
    public static Mod? Fetch(int id)
    {
        string url = Utils.CreateCoreItemDataRequestUrl("Mod",id,[
            "name",
            "userid",
            "Owner().name",
            "text",
            "Files().aFiles()"
        ]);
        return DownloadManager.GetJSON<Mod>(url);
    }
    public ModFile GetLatestUpdate()
    {
        ModFile newest = Files.Values.ToArray().Last();
        foreach(ModFile file in Files.Values)
        {
            if(DateTime.Compare(newest.DateAdded,file.DateAdded) < 0)
                newest = file;
        }
        return newest;
    }
    public string DownloadLatestUpdate()
    {
        return DownloadManager.GetTemp(GetLatestUpdate().DownloadUrl);
    }
}