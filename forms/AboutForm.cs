namespace ThemModdingHerds.VelvetBeautifier
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void GitHubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utils.OpenLink("https://github.com/ThemModdingHerds/velvet-beautifier");

        private void GameBananaLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utils.OpenLink("https://gamebanana.com/tools/15674");

        private void NightTheFoxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utils.OpenLink("https://github.com/N1ghtTheF0x");
    }
}
