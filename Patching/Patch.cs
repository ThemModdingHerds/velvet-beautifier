using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Patches;
public class Patch
{
    [JsonPropertyName("type")]
    public PatchType Type {get; set;}
    [JsonPropertyName("value_type")]
    public PatchValue ValueType {get; set;}
    [JsonPropertyName("path")]
    public string Path {get; set;} = "";
    [JsonPropertyName("property")]
    public string Property {get; set;} = "";
    [JsonPropertyName("value")]
    public object? Value {get; set;}
    [JsonIgnore]
    public List<IPatchSegment> PropertySegments {get => PatchUtils.GetSegments(Property);}
    public void Apply(XmlDocument document)
    {
        XmlNode node = document;
        List<IPatchSegment> segments = PropertySegments;
        while(segments.Count != 0)
        {
            IPatchSegment segment = segments.First();
            segments.RemoveAt(0);
            if(segment.Type == PatchSegmentType.Property)
            {
                PatchPropertySegment propertySegment = (PatchPropertySegment)segment;
                node = node[propertySegment.Path] ?? throw new VelvetException("Patch.Apply","Invalid property at object");
            }
            if(segment.Type == PatchSegmentType.ArrayIndex)
            {
                PatchArrayIndexSegment arrayIndexSegment = (PatchArrayIndexSegment)segment;
                node = node.ChildNodes.Item(arrayIndexSegment.Index) ?? throw new VelvetException("Patch.Apply","Invalid index at array");
            }
        }
        XmlNode parent = node.ParentNode ?? node;
    }
    public void Apply(JsonNode json)
    {
        JsonNode node = json.Root;
        List<IPatchSegment> segments = PropertySegments;
        while(segments.Count != 0)
        {
            IPatchSegment segment = segments.First();
            segments.RemoveAt(0);
            if(segment.Type == PatchSegmentType.Property)
            {
                PatchPropertySegment propertySegment = (PatchPropertySegment)segment;
                JsonObject obj = node.AsObject();
                node = obj[propertySegment.Path] ?? throw new VelvetException("Patch.Apply","Invalid property at object");
            }
            if(segment.Type == PatchSegmentType.ArrayIndex)
            {
                PatchArrayIndexSegment arrayIndexSegment = (PatchArrayIndexSegment)segment;
                JsonArray array = node.AsArray();
                node = array[arrayIndexSegment.Index] ?? throw new VelvetException("Patch.Apply","Invalid index at array");
            }
        }
        JsonNode parent = node.Parent ?? node;
        switch(Type)
        {
            case PatchType.Delete:
                if(parent == node) throw new VelvetException("Patch.Apply","Can't delete root node");
                parent[node.GetPropertyName()] = null;
                break;
            case PatchType.Insert:

                break;
            case PatchType.Update:

                break;
        }
    }
}