using Gtk;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class EditConfigurationWindow : Window
{
    public const string ID = "EditConfigurationWindow";
    public Entry ClientPathEntry {get;}
    public Entry ServerPathEntry {get;}
    public Button ApplyButton {get;}
    public Button ClientPickButton {get;}
    public Button ServerPickButton {get;}
    public EditConfigurationWindow(): this(new Builder(VelvetGtk.GLADEFILE)) {}
    private EditConfigurationWindow(Builder builder): base(builder.GetRawOwnedObject(ID))
    {
        builder.Autoconnect(this);

        ClientPathEntry = new(builder.GetRawOwnedObject("ConfigClientPathEntry"));
        ServerPathEntry = new(builder.GetRawOwnedObject("ConfigServerPathEntry"));
        ApplyButton = new(builder.GetRawOwnedObject("ConfigApplyButton"));
        ClientPickButton = new(builder.GetRawOwnedObject("ConfigClientPickButton"));
        ServerPickButton = new(builder.GetRawOwnedObject("ConfigServerPickButton"));

        ClientPathEntry.Text = Config.ClientPath ?? string.Empty;
        ServerPathEntry.Text = Config.ServerPath ?? string.Empty;

        ClientPathEntry.Changed += OnTextChange;
        ServerPathEntry.Changed += OnTextChange;

        ClientPickButton.Clicked += delegate {ClientPathEntry.Text = FolderPick() ?? ClientPathEntry.Text;};
        ServerPickButton.Clicked += delegate {ServerPathEntry.Text = FolderPick() ?? ServerPathEntry.Text;};

        ApplyButton.Clicked += OnApply;
    }
    private string? FolderPick()
    {
        string[] folders = this.OpenFolderDialog("Select installation folder");
        if(folders.Length != 1) return null;
        return folders[0];
    }
    private bool Valid()
    {
        bool valid = false;

        if(ClientPathEntry.Text != string.Empty)
            valid = Client.Valid(ClientPathEntry.Text);
        if(ServerPathEntry.Text != string.Empty)
            valid = Server.Valid(ServerPathEntry.Text);

        return valid;
    }
    private void OnTextChange(object? sender,EventArgs e)
    {
        ApplyButton.Sensitive = Valid();
    }
    private void OnApply(object? sender,EventArgs e)
    {
        if(ClientPathEntry.Text != string.Empty)
            Config.ClientPath = ClientPathEntry.Text;
        if(ServerPathEntry.Text != string.Empty)
            Config.ServerPath = ServerPathEntry.Text;
        Config.Write();
        this.ShowMessageBox("Updated configuration file! Reloading...");
        ModLoaderTool.Reload();
        Destroy();
    }
}