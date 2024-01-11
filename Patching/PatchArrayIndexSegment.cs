namespace ThemModdingHerds.VelvetBeautifier.Patches;
public class PatchArrayIndexSegment(int index) : IPatchSegment
{
    public PatchSegmentType Type {get => PatchSegmentType.ArrayIndex;}
    public int Index {get => index;}
}