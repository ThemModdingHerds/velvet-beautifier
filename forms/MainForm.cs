using ThemModdingHerds.VelvetBeautifier.forms;

namespace ThemModdingHerds.VelvetBeautifier;

public partial class MainForm : Form
{
    private readonly ExtractionForm extractionForm = new();
    private readonly AboutForm aboutForm = new();
    private readonly InstallForm installForm = new();
    public MainForm()
    {
        InitializeComponent();
        extractionForm.Owner = aboutForm.Owner = installForm.Owner = this;
        CheckForTFHFolder();
        BackupManager.BackupTFHResources();
        // BackupManager.BackupData01();
        RefreshModList();
#if DEBUG
        Debug();
#endif
        Config.Current.Save();
    }
    private void Debug()
    {
        ToolStripMenuItem debugItem = new()
        {
            Text = "Debug"
        };

        MenuBar.Items.Add(debugItem);

        ToolStripMenuItem gbTestItem = new()
        {
            Text = "Test GameBanana Link"
        };
        gbTestItem.Click += (object? sender, EventArgs e) => GameBanana.HandleCommandLine("https://gamebanana.com/mmdl/1108527,Mod,50765");
        debugItem.DropDownItems.Add(gbTestItem);
    }
    private void CheckForTFHFolder()
    {
        if (!Config.Current.ExistsTFHFolder())
        {
            Velvet.ShowMessageBox("No Them's Fightin' Herds Folder found! Please select the folder", "Didn't find installation folder");
            DialogResult result = FindTFHFolder.ShowDialog();
            if (result != DialogResult.OK)
            {
                Velvet.ShowMessageBox("You need to select a folder, exiting...", "No folder selected");
                Close();
                Environment.Exit(1);
            }
            Config.Current.TfhPath = FindTFHFolder.SelectedPath;
            if(!Config.Current.ExistsTFHFolder())
            {
                Velvet.ShowMessageBox("Thie folder you selected is invalid","Invalid Folder");
                Close();
                Environment.Exit(1);
            }
        }
        if(!Config.Current.ExistsTFHResourcesFolder() || !Config.Current.ExistsData01Folder())
        {
            Velvet.ShowMessageBox("The folder you've specified does not contain Them's Fightin' Herds assets","Invalid TFH folder");
            Config.Current.TfhPath = "";
            Config.Current.Save();
            Close();
            Environment.Exit(1);
        }
    }
    private void MenuFileExit_Click(object sender, EventArgs e) => Close();
    private void MenuToolsExtraction_Click(object sender, EventArgs e) => extractionForm.ShowDialog();
    private void MenuHelpAbout_Click(object sender, EventArgs e) => aboutForm.ShowDialog();
    private void MenuFileInstallMod_Click(object sender, EventArgs e) => installForm.ShowDialog();
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
        {
            int index = ModList.Items.Add(mod.Info.Name);
            if (Config.Current.IsModEnabled(mod))
                ModList.SetItemCheckState(index, CheckState.Checked);
        }
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
        Mod? mod = GetModFromModList(ModList.SelectedIndex);
        if (mod == null)
            return;

        ModNameLabel.Visible = ModAuthorLabel.Visible = ModDescriptionBox.Visible = true;

        ModNameLabel.Text = mod.Info.Name;
        ModAuthorLabel.Text = "by " + mod.Info.Author;
        ModDescriptionBox.Text = mod.Info.Description;
    }
    private Mod? GetModFromModList(int index)
    {
        object? modname_o = ModList.Items[index];
        if (modname_o == null)
            return null;
        string modname = (string)modname_o;
        return ModManager.FindModByName(modname);
    }
    private List<Mod> GetEnabledMods()
    {
        List<Mod> mods = [];

        for (int index = 0; index < ModList.CheckedItems.Count; index++)
        {
            object? modname_o = ModList.CheckedItems[index];
            if (modname_o == null)
                continue;
            string modname = (string)modname_o;
            Mod? mod = ModManager.FindModByName(modname);
            if (mod != null)
                mods.Add(mod);
        }

        return mods;
    }
    private void ApplyButton_Click(object sender, EventArgs e) => ApplyMods();
    private void MenuFileApplyMods_Click(object sender, EventArgs e) => ApplyMods();

    private void ApplyMods()
    {
        List<Mod> mods = GetEnabledMods();
        if (mods.Count == 0)
        {
            ModManager.RevertMods();
            Velvet.ShowMessageBox("Mods have been removed");
            return;
        }

        TFHResourceUtils.ApplyTFHResources(mods);

        Velvet.ShowMessageBox(mods.Count + " mods have been applied! You can start the game with the modifications", "Done");
    }

    private void MenuToolsRegisterScheme_Click(object sender, EventArgs e) => Utils.CreateURIScheme();

    private void ModList_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        Mod? mod = GetModFromModList(e.Index);
        if (mod == null) return;
        switch (e.NewValue)
        {
            case CheckState.Checked:
                Config.Current.EnableMod(mod);
                break;
            default:
                Config.Current.DisableMod(mod);
                break;
        }

        Config.Current.Save();
    }

}
