using System.Text.Json.Serialization;

namespace ThemModdingHerds.VelvetBeautifier.GameBanana;

public class Error
{
    [JsonPropertyName("error")]
    public string Description {get;set;} = "";
    [JsonPropertyName("error_code")]
    public string Code {get;set;} = "";
}
