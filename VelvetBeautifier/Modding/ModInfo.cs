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
    public Version Version {get; set;} = new();
    [JsonPropertyName("url")]
    public string? Url {get; set;}
    public static ModInfo? Read(string filepath)
    {
        return FileSystem.ReadJson<ModInfo?>(filepath);
    }
    public void Write(string filepath)
    {
        FileSystem.WriteJson(filepath,this);
    }
    public override string ToString()
    {
        return $"{Name} [{Id}] by {Author} v{Version}";
    }
}