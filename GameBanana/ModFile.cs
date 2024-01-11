using System.Text.Json.Serialization;

namespace ThemModdingHerds.VelvetBeautifier.GameBanana;

public class ModFile
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
