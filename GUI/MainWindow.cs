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
        Icon = Utils.VelvetIcon;
        builder.Autoconnect(this);
        Utils.JoinThread(ModLoaderTool.Run);
        DeleteEvent += delegate {Application.Quit();};
        MenuBarItems.Init(MenuBar,this);
        ToolbarItems.Init(Toolbar,this);
        RefreshModList();
        Drag.DestSet(this,DestDefaults.All,[new("text/uri-list",TargetFlags.OtherApp,0)],Gdk.DragAction.Copy);
        DragDataReceived += OnWindowDragDataReceived;
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
    private void OnWindowDragDataReceived(object o,DragDataReceivedArgs args)
    {
        IEnumerable<Uri> uris = from uri in args.SelectionData.Uris select new Uri(uri);
        foreach(Uri uri in uris)
        {
            string path = uri.IsFile ? uri.LocalPath : uri.AbsoluteUri;
            ModDB.InstallMod(path);
        }
        RefreshModList();
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
            name.TooltipText = Velvet.Velvetify(mod.Info.Description);
            name.ButtonPressEvent += (o,args) =>
            {
                if(args.Event.Button == 3)
                    CreateContextMenu(mod).Popup();
            };

            ModList.Attach(name,0,row,1,1);
            ModList.Attach(version,1,row,1,1);
            ModList.Attach(author,2,row,1,1);
            row++;
        }
        ModList.ShowAll();
    }
    private Menu CreateContextMenu(Mod mod)
    {
        Menu contextmenu = [];

        MenuItem uninstall = new("_Uninstall");
        uninstall.Activated += delegate
        {
            ModDB.UninstallMod(mod);
            RefreshModList();
        };
        contextmenu.Append(uninstall);
        
        contextmenu.Append(new SeparatorMenuItem());

        MenuItem showFolder = new("_Show folder");
        showFolder.Activated += delegate {FileSystem.OpenFolder(mod.Folder);};
        contextmenu.Append(showFolder);

        contextmenu.ShowAll();
        return contextmenu;
    }
    public void ApplyMods()
    {
        Sensitive = false;
        Utils.JoinThread(ModLoaderTool.ApplyMods);
        this.ShowMessageBox("Enabled mods have been applied, you can now start the game with mods!");
        Sensitive = true;
    }
    public void Revert()
    {
        Sensitive = false;
        Utils.JoinThread(ModLoaderTool.Revert);
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
