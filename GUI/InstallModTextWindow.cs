using Gtk;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class InstallModTextWindow : Window
{
    public Grid Root => (Grid)Children[0];
    public Button InstallButton => (Button)Root.Children[0];
    public Entry InputEntry => (Entry)Root.Children[1];
    public InstallModTextWindow(): this(new Builder("MainWindow.glade")) {}
    private InstallModTextWindow(Builder builder): base(builder.GetRawOwnedObject("InstallModTextWindow"))
    {
        Icon = Utils.VelvetIcon;
        builder.Autoconnect(this);
        InputEntry.Changed += delegate
        {
            InstallButton.Sensitive = Url.IsUrl(InputEntry.Text) || int.TryParse(InputEntry.Text,out int _);
        };
        InstallButton.Clicked += delegate
        {
            ModInstallResult result = ModDB.InstallMod(InputEntry.Text);
            switch(result)
            {
                case ModInstallResult.AlreadyExists:
                case ModInstallResult.Ok:
                    this.ShowMessageBox("Successfully installed mod! Refresh to see installed mod!");
                    break;
                case ModInstallResult.Failed:
                case ModInstallResult.Invalid:
                    this.ShowMessageBox("There was a problem installing the mod. Make sure the URL/GameBanana ID is valid!");
                    break;
            }
            Destroy();
        };
    }
}