using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModDetails : ScrollView
{
    private readonly Label _title = new()
    {
        X = Pos.Center(),
        Y = 0,
        Width = Dim.Fill(),
        Height = 1
    };
    private readonly Label _description = new()
    {
        X = 1,
        Y = 2,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    public ModDetails()
    {
        Add(_title,_description);
    }
    public void SetMod(ModListView.IModItem mod)
    {
        _title.Text = Velvet.Velvetify($"{mod.Name} by {mod.Author} - v{mod.Version}");
        _description.Text = Velvet.Velvetify(mod.Description);
    }
}