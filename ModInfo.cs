using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThemModdingHerds.VelvetBeautifier;
public class ModInfo
{
    [JsonPropertyName("id")]
    public string Id {get; set;} = "com.example.mod";
    [JsonPropertyName("name")]
    public string Name {get; set;} = "Example Mod";
    [JsonPropertyName("author")]
    public string Author {get; set;} = Environment.UserName;
}