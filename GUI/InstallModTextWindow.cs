using Gtk;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class InstallModTextWindow : Window
{
    public const string ID = "InstallModTextWindow";
    public Button InstallButton {get;}
    public Entry InputEntry {get;}
    public InstallModTextWindow(): this(new Builder(VelvetGtk.GLADEFILE)) {}
    private InstallModTextWindow(Builder builder): base(builder.GetRawOwnedObject(ID))
    {
        Icon = Utils.VelvetIcon;
        builder.Autoconnect(this);

        InstallButton = new(builder.GetRawOwnedObject("InstallTextButton"));
        InputEntry = new(builder.GetRawOwnedObject("InstallTextEntry"));

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