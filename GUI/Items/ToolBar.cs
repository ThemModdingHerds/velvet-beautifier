using Gtk;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public static class ToolbarItems
{
    private static ToolButton GetRefreshButton(Toolbar toolbar) => (ToolButton)toolbar.Children[0];
    private static ToolButton GetApplyButton(Toolbar toolbar) => (ToolButton)toolbar.Children[1];
    private static ToolButton GetRevertButton(Toolbar toolbar) => (ToolButton)toolbar.Children[2];
    private static ToolButton GetLaunchButton(Toolbar toolbar) => (ToolButton)toolbar.Children[3];
    public static void Init(Toolbar toolbar,MainWindow window)
    {
        ToolButton refresh = GetRefreshButton(toolbar);
        ToolButton apply = GetApplyButton(toolbar);
        ToolButton revert = GetRevertButton(toolbar);
        ToolButton launch = GetLaunchButton(toolbar);

        refresh.Clicked += delegate {window.RefreshModList();};
        apply.Clicked += delegate {window.ApplyMods();};
        revert.Clicked += delegate {window.Revert();};
        launch.Clicked += delegate {window.Launch();};
    }
}