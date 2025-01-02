using Gtk;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ToolbarItems
{
    readonly ToolButton refresh;
    readonly ToolButton apply;
    readonly ToolButton revert;
    readonly ToolButton launch;
    public ToolbarItems(Builder builder,MainWindow window)
    {
        refresh = new(builder.GetRawOwnedObject("ToolbarRefresh"));
        apply = new(builder.GetRawOwnedObject("ToolbarApply"));
        revert = new(builder.GetRawOwnedObject("ToolbarRevert"));
        launch = new(builder.GetRawOwnedObject("ToolbarLaunch"));

        refresh.Clicked += delegate {window.RefreshModList();};
        apply.Clicked += delegate {window.ApplyMods();};
        revert.Clicked += delegate {window.Revert();};
        launch.Clicked += delegate {window.Launch();};
    }
}