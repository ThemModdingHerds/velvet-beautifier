using Gtk;
using ThemModdingHerds.VelvetBeautifier.GUI.Items;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class MainWindow : Window
{
    public Box Root => (Box)Children[0];
    public MenuBar MenuBar => (MenuBar)Root.Children[0];
    public Toolbar Toolbar => (Toolbar)Root.Children[1];
    public Grid ModList => (Grid)Root.Children[2];
    public MainWindow(): this(new Builder("MainWindow.glade")) { }
    private MainWindow(Builder builder): base(builder.GetRawOwnedObject("MainWindow"))
    {
        builder.Autoconnect(this);
        Utils.JoinThread(ModLoaderTool.Run);
        DeleteEvent += delegate {Application.Quit();};
        MenuBarItems.Init(MenuBar,this);
        ToolbarItems.Init(Toolbar,this);
        RefreshModList();
    }
    private void ResetModList()
    {
        foreach(Widget child in ModList.Children)
            ModList.Remove(child);
        Label name = new("Name");
        Label version = new("Version");
        Label author = new("Author");
        ModList.Attach(name,0,0,1,1);
        ModList.Attach(version,1,0,1,1);
        ModList.Attach(author,2,0,1,1);
        ModList.ShowAll();
    }
    public void RefreshModList()
    {
        ResetModList();
        int row = 1;
        foreach(Mod mod in ModDB.Mods)
        {
            CheckButton name = new(Velvet.Velvetify(mod.Info.Name));
            Label version = new(mod.Info.Version.ToString());
            Label author = new(Velvet.Velvetify(mod.Info.Author));

            name.Active = mod.Enabled;
            name.Toggled += delegate
            {
                mod.SetEnabled(name.Active);
            };
            name.TooltipText = version.TooltipText = author.TooltipText = Velvet.Velvetify(mod.Info.Description);

            ModList.Attach(name,0,row,1,1);
            ModList.Attach(version,1,row,1,1);
            ModList.Attach(author,2,row,1,1);
            row++;
        }
        ModList.ShowAll();
    }
    public void ApplyMods()
    {
        Sensitive = false;
        ModLoaderTool.ApplyMods();
        this.ShowMessageBox("Enabled mods have been applied, you can now start the game with mods!");
        Sensitive = true;
    }
    public void Revert()
    {
        Sensitive = false;
        ModLoaderTool.Revert();
        this.ShowMessageBox("Reverted all the game files back to their orignal!");
        Sensitive = true;
    }
    public void Launch()
    {
        if(ModLoaderTool.Client == null)
        {
            this.ShowMessageBox("No client was found. Make sure the path to the client is set correctly!");
            return;
        }
        ModLoaderTool.Client.Launch();
    }
}
