using System.Text.Json;
using System.Text.Json.Serialization;
using ThemModdingHerds.VelvetBeautifier.Utilities;
namespace ThemModdingHerds.VelvetBeautifier.Modding;
public class ModInfo
{
    [JsonPropertyName("id")]
    public string Id {get; set;} = "com.example.mod";
    [JsonPropertyName("name")]
    public string Name {get; set;} = "Example Mod";
    [JsonPropertyName("author")]
    public string Author {get; set;} = Environment.UserName;
    [JsonPropertyName("desc")]
    public string Description {get; set;} = "This is an Example Mod";
    [JsonPropertyName("version")]
    public string Version {get; set;} = "legacy";
    [JsonPropertyName("url")]
    public string? Url {get; set;}
    public static ModInfo Read(string filepath)
    {
        return JsonSerializer.Deserialize<ModInfo>(File.ReadAllText(filepath)) ?? throw new VelvetException("new Mod","couldn't read mod entry at " + filepath);
    }
    public void Write(string filepath)
    {
        File.WriteAllText(filepath,JsonSerializer.Serialize(this));
    }
}