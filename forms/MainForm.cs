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
        ModList.Items.Clear();
        foreach (Mod mod in mods)
            ModList.Items.Add(mod.Info.Name);
    }
    private void RefreshModList()
    {
        Velvet.ConsoleWriteLine("Refreshing modlist...");
        ModManager.RefreshMods();
        FillModList([.. ModManager.Mods.Values]);
    }

    private void MenuFileRefreshMods_Click(object sender, EventArgs e) => RefreshModList();

    private void ModList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Mod? mod =GetModFromModList(ModList.SelectedIndex);
        if(mod == null)
            return;

        ModNameLabel.Visible = ModAuthorLabel.Visible = ModDescriptionBox.Visible = true;

        ModNameLabel.Text = mod.Info.Name;
        ModAuthorLabel.Text = "by " + mod.Info.Author;
        ModDescriptionBox.Text = mod.Info.Description;
    }
    private Mod? GetModFromModList(int index)
    {
        object? modname_o = ModList.Items[index];
        if(modname_o == null)
            return null;
        string modname = (string)modname_o;
        return ModManager.FindModByName(modname);
    }
    private List<Mod> GetEnabledMods()
    {
        List<Mod> mods = [];

        for(int index = 0;index < ModList.CheckedItems.Count;index++)
        {
            object? modname_o = ModList.CheckedItems[index];
            if(modname_o == null)
                continue;
            string modname = (string)modname_o;
            Mod? mod = ModManager.FindModByName(modname);
            if(mod != null)
                mods.Add(mod);
        }

        return mods;
    }
    private void ApplyButton_Click(object sender, EventArgs e)
    {
        List<Mod> mods = GetEnabledMods();
        if(mods.Count == 0)
            return;

        Dictionary<string,List<TFHResourceMod>> tfhres_mods = [];

        foreach(Mod mod in mods)
        {
            foreach(TFHResourceMod resourceMod in mod.TFHResourceMods)
            {
                if(!tfhres_mods.ContainsKey(resourceMod.Resource))
                    tfhres_mods.Add(resourceMod.Resource,[]);
                List<TFHResourceMod> ts = tfhres_mods[resourceMod.Resource];
                ts.Add(resourceMod);
            }
        }

        foreach((string resource,List<TFHResourceMod> tfhres) in tfhres_mods)
        {
            string target_path = Config.Current.GetTFHResourcesFolder(resource);
            TFHResourceMod.ModIt(target_path,tfhres);
        }

        Velvet.ShowMessageBox(mods.Count + " mods have been applied! You can start the game with the modifications","Done");
    }
}
