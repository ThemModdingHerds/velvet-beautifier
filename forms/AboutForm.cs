using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Forms;
public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        VersionLabel.Text = "v" + Dotnet.ExeVersion.ToString();
    }
    private void GitHubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Url.OpenUrl("https://github.com/ThemModdingHerds/velvet-beautifier");
    private void GameBananaLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Url.OpenUrl("https://gamebanana.com/tools/15674");
    private void NightTheFoxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Url.OpenUrl("https://github.com/N1ghtTheF0x");
}
