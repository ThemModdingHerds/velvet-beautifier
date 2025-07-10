using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class MainTopLevel : Toplevel
{
    private const int HorSepDistance = 25;
    public readonly ModListView modList = new()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    public readonly ModDetails modDetails = new()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill(1)
    };
    private readonly FrameView left = new("Mods")
    {
        X = 0,
        Y = 1,
        Width = HorSepDistance,
        Height = Dim.Fill()
    };
    private readonly FrameView right = new("Description")
    {
        X = HorSepDistance,
        Y = 1,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    public MainTopLevel()
    {
        Add(MenuBar = GUI.MenuBar.Create(this));

        left.Add(modList);
        right.Add(modDetails);
        
        Add(left,right);

        modList.OnModSelect += modDetails.SetMod;
    }
}