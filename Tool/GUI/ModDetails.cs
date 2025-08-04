using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModDetails : FrameView
{
    private readonly Label _description = new()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    public ModDetails()
    {
        Title = "Description";
        Add(_description);
    }
    public void SetMod(ModListView.IModItem mod)
    {
        _description.Text = Velvet.Velvetify(mod.Description);
    }
}