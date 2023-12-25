namespace ThemModdingHerds.VelvetBeautifier;
public partial class InstallForm : Form
{
    public InstallForm()
    {
        InitializeComponent();
    }

    private void InstallModUrl_TextChanged(object sender, EventArgs e)
    {
        FetchButton.Enabled = (Utils.IsUrl(InstallModUrl.Text) && InstallModUrl.Text.EndsWith(".zip")) || GameBanana.ValidUrl(InstallModUrl.Text);
    }

    private async void FetchButton_Click(object sender, EventArgs e)
    {
        string url = InstallModUrl.Text;
        if(GameBanana.ValidUrl(url))
        {
            GameBananaMod? gb_mod = await GameBananaMod.Fetch(GameBanana.GetModId(url));
            if(gb_mod == null)
            {
                Velvet.ShowMessageBox("Couldn't fetch " + url);
                Close();
                return;
            }
            new DownloadForm(gb_mod).ShowDialog();
            Close();
            return;
        }
        string unzippedpath = await DownloadManager.GetAndUnzip(url);
        Mod mod = new(unzippedpath);
        new DownloadForm(mod,unzippedpath).ShowDialog();
        Close();
        return;
    }
}
