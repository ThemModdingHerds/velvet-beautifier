using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Patches;
[JsonDerivedType(typeof(TextPatch),"text")]
[JsonDerivedType(typeof(BinaryPatch),"binary")]
public class Patch
{
    [JsonPropertyName("target")]
    public string Target {get;set;} = string.Empty;
    public string GetFullTargetPath(Game game) => Path.GetFullPath(Path.Combine(game.Folder,Target));
    public bool ExistsTarget(Game game) => File.Exists(GetFullTargetPath(game));
}
public class TextPatch : Patch
{
    [JsonPropertyName("match")]
    public string Match {get;set;} = string.Empty;
    [JsonPropertyName("value")]
    public string Value {get;set;} = string.Empty;
}
public class BinaryPatch : Patch
{
    [JsonPropertyName("offset")]
    public long? Offset {get;set;}
    [JsonPropertyName("filepath")]
    public string FilePath {get;set;} = string.Empty;
    public bool IsFullReplace => Offset == null;
}