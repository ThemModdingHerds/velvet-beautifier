namespace ThemModdingHerds.VelvetBeautifier
{
    public partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void GitHubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utils.OpenLink("https://github.com/ThemModdingHerds/velvet-beautifier");
    }
}
