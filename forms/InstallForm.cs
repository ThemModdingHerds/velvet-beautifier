using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Forms;
public partial class InstallForm : Form
{
    private MainForm MainForm {get => (MainForm?)Owner ?? throw new VelvetException("InstallForm.MainForm","MainForm is null");}
    public InstallForm()
    {
        InitializeComponent();
    }

    private void InstallModUrl_TextChanged(object sender, EventArgs e)
    {
        FetchButton.Enabled = (Url.IsUrl(InstallModUrl.Text) && InstallModUrl.Text.EndsWith(".zip")) || GameBanana.Utils.ValidUrl(InstallModUrl.Text);
    }

    private async void FetchButton_Click(object sender, EventArgs e)
    {
        string url = InstallModUrl.Text;
        DownloadForm download;
        if(GameBanana.Utils.ValidUrl(url))
        {
            GameBanana.Mod? gb_mod = await GameBanana.Mod.Fetch(GameBanana.Utils.GetModId(url));
            if(gb_mod == null)
            {
                Velvet.ShowMessageBox("Couldn't fetch " + url);
                Close();
                return;
            }
            download = new(gb_mod)
            {
                Owner = MainForm
            };
            download.ShowDialog();
            Close();
            return;
        }
        string unzippedpath = await DownloadManager.GetAndUnzip(url);
        Mod mod = new(unzippedpath);
        mod.Info.Url = url;
        download = new(mod,unzippedpath)
        {
            Owner = MainForm
        };
        download.ShowDialog();
        Close();
        return;
    }
}
