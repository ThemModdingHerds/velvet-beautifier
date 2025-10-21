using Terminal.Gui;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class MainTopLevel : Toplevel
{
    public static readonly MainTopLevel Instance = new();
    private const int HorSepDistance = 25;
    public readonly ModListView modList = new()
    {
        X = 0,
        Y = 1,
        Width = HorSepDistance,
        Height = Dim.Fill(1)
    };
    public readonly ModDetails modDetails = new()
    {
        X = HorSepDistance,
        Y = 1,
        Width = Dim.Fill(),
        Height = Dim.Fill(1)
    };
    private MainTopLevel()
    {
        Add(MenuBar = MenuBarUtilities.Create(this));

        Add(modList, modDetails);

        modList.OnModSelect += modDetails.SetMod;
        modList.OnModSelect += OnModSelect;
        modList.OnModeChange += OnModeChange;

        ResetStatusBar();
    }
    private void OnModSelect(ModListView.IModItem item)
    {
        if (StatusBar != null)
            Remove(StatusBar);
        Add(StatusBar = StatusBarUtilities.Create(item));
    }
    private void OnModeChange(ModListView.Mode mode) => ResetStatusBar();
    private void ResetStatusBar()
    {
        if (StatusBar != null)
            Remove(StatusBar);
        Add(StatusBar = new());
    }
}