namespace ThemModdingHerds.VelvetBeautifier;

public partial class MainForm : Form
{
    private readonly ExtractionWindow extractionWindow = new();
    private readonly AboutWindow aboutWindow = new();
    public MainForm()
    {
        InitializeComponent();
        extractionWindow.Owner = aboutWindow.Owner = this;
        CheckForTFHFolder();
        BackupManager.BackupTFHResources();
        RefreshModList();
    }
    private void CheckForTFHFolder()
    {
        if (!Config.Current.ExistsTFHFolder())
        {
            Velvet.ShowMessageBox("No Them's Fightin' Herds Folder found! Please select the folder", "Didn't find installation folder");
            DialogResult result = FindTFHFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                Config.Current.TfhPath = FindTFHFolder.SelectedPath;
                Config.Current.Save();
                return;
            }

            Velvet.ShowMessageBox("You need to select a folder, exiting...", "No folder selected");
            Environment.Exit(1);
        }
    }
    private void MenuFileExit_Click(object sender, EventArgs e) => Environment.Exit(0);

    private void MenuToolsExtraction_Click(object sender, EventArgs e) => extractionWindow.ShowDialog();

    private void MenuHelpAbout_Click(object sender, EventArgs e) => aboutWindow.ShowDialog();

    private void MenuHelpFI_Click(object sender, EventArgs e) => Utils.OpenLink("https://github.com/ThemModdingHerds/velvet-beautifier/issues/new/choose");

    private void MenuToolsConfigureTFHFolder_Click(object sender, EventArgs e)
    {
        DialogResult result = FindTFHFolder.ShowDialog();
        if (result == DialogResult.OK)
        {
            Config.Current.TfhPath = FindTFHFolder.SelectedPath;
            Config.Current.Save();
        }
    }
    private void FillModList(List<Mod> mods)
    {
        foreach (Mod mod in mods)
            ModList.Items.Add(mod.Info.Name);
    }
    private void RefreshModList()
    {
        ModManager.RefreshMods();
        FillModList([.. ModManager.Mods.Values]);
    }

    private void MenuFileRefreshMods_Click(object sender, EventArgs e) => RefreshModList();

    private void ModList_SelectedIndexChanged(object sender, EventArgs e)
    {
        object? modname = ModList.Items[ModList.SelectedIndex];
    }

    private void ApplyButton_Click(object sender, EventArgs e)
    {

    }
}
