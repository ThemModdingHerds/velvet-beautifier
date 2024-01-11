namespace ThemModdingHerds.VelvetBeautifier.Patches;
public class PatchPropertySegment(string path) : IPatchSegment
{
    public PatchSegmentType Type {get => PatchSegmentType.Property;}
    public string Path {get => path;}
}