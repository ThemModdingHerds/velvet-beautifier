using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModDetails
{
    private readonly Label _title = new()
    {
        X = Pos.Center(),
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    private readonly Label _description = new()
    {
        X = 0,
        Y = 2,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    public ModDetails(View view)
    {
        view.Add(_title);
        view.Add(_description);
    }
    public void SetMod(Mod mod)
    {
        _title.Text = mod.Info.ToString();
        _description.Text = mod.Info.Description;
    }
}