using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Forms;
public partial class MainForm : Form
{
    private readonly AboutForm aboutForm = new();
    private readonly InstallForm installForm = new();
    public List<string> Argv = [];
    private Mod? SelectedMod { get => GetModFromModList(ModList.SelectedIndex); }
    public MainForm()
    {
        InitializeComponent();
        aboutForm.Owner = installForm.Owner = this;
        ModList.ContextMenuStrip = ModListContextMenu;
        CheckForCorrectFolders();
        // BackupManager.BackupTFHResources();
        // BackupManager.BackupData01();
        RefreshModList();
#if DEBUG
        Debug();
#endif
        Application.Instance.Config.Save();
    }
    private void MenuFileExit_Click(object sender, EventArgs e) => Close();
    private void MenuToolsExtraction_Click(object sender, EventArgs e) => ExtractSelectedFiles();
    private void MenuHelpAbout_Click(object sender, EventArgs e) => aboutForm.ShowDialog(this);
    private void ContextInstallFromURL_Click(object sender, EventArgs e) => installForm.ShowDialog(this);
    private void ContextInstallFromFolder_Click(object sender, EventArgs e) => InstallFromFolder();
    private void MenuInstallFromURL_Click(object sender, EventArgs e) => installForm.ShowDialog(this);
    private void MenuInstallFromFolder_Click(object sender, EventArgs e) => InstallFromFolder();
    private void MenuHelpFI_Click(object sender, EventArgs e) => Url.OpenUrl("https://github.com/ThemModdingHerds/velvet-beautifier/issues/new/choose");
    private void MenuToolsConfigureTFHFolder_Click(object sender, EventArgs e) => SelectGameFolder();
    private void MenuFileRefreshMods_Click(object sender, EventArgs e) => RefreshModList();
    private void ApplyButton_Click(object sender, EventArgs e) => Application.Instance.ModDB.Apply();
    private void MenuFileApplyMods_Click(object sender, EventArgs e) => Application.Instance.ModDB.Apply();
    private void MenuToolsRegisterScheme_Click(object sender, EventArgs e) => Win32.CreateURIScheme();
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
        gbTestItem.Click += async (object? sender, EventArgs e) => await GameBanana.Utils.ParseCommandLine("https://gamebanana.com/mmdl/1108527,Mod,50765");
        debugItem.DropDownItems.Add(gbTestItem);
    }
    private void CheckForCorrectFolders()
    {
        if (!Application.Instance.Config.ExistsGameFolder())
        {
            Velvet.ShowMessageBox("No Them's Fightin' Herds Folder found! Please select the folder", "Didn't find installation folder");
            SelectGameFolder();
        }
        if (Application.Instance.Config.ExistsServerFolder())
        {
            if (!Application.Instance.Server.IsServer())
            {
                Velvet.ShowMessageBox("the server folder is invalid, no mods for the local server");
                Application.Instance.Config.ServerPath = "";
                Application.Instance.Config.Save();
            }
        }
    }
    private void SelectGameFolder()
    {
        SelectFolder.Description = Velvet.Velvetify("Select Installation Folder of Them's Fightin' Herds");
        DialogResult result = SelectFolder.ShowDialog();
        if (result != DialogResult.OK)
        {
            Velvet.ShowMessageBox("You need to select a folder, exiting...", "No folder selected");
            Close();
            Environment.Exit(1);
        }
        Application.Instance.Config.ClientPath = SelectFolder.SelectedPath;
        if (!Application.Instance.Config.ExistsGameFolder() || !Application.Instance.Client.IsClient())
        {
            Velvet.ShowMessageBox("This folder you selected is invalid", "Invalid Folder");
            Application.Instance.Config.ClientPath = "";
            Application.Instance.Config.Save();
            Close();
            Environment.Exit(1);
        }
        if (!Application.Instance.Client.ExistsTFHResourcesFolder() || !Application.Instance.Client.ExistsData01Folder())
        {
            Velvet.ShowMessageBox("The folder you've specified does not contain Them's Fightin' Herds assets", "Invalid TFH folder");
            Application.Instance.Config.ClientPath = "";
            Application.Instance.Config.Save();
            Close();
            Environment.Exit(1);
        }
        Application.Instance.Config.Save();
    }
    public void RefreshModList()
    {
        Velvet.ConsoleWriteLine("Refreshing modlist...");
        Application.Instance.ModDB.Refresh();
        ModList.Items.Clear();
        foreach (Mod mod in Application.Instance.ModDB.Mods)
        {
            int index = ModList.Items.Add(mod.Info.Name);
            if (mod.Enabled)
                ModList.SetItemCheckState(index, CheckState.Checked);
        }
    }
    private void ModList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SelectedMod == null)
            return;

        ModNameLabel.Visible = ModAuthorLabel.Visible = ModDescriptionBox.Visible = true;

        ModNameLabel.Text = SelectedMod.Info.Name;
        ModAuthorLabel.Text = "by " + SelectedMod.Info.Author;
        ModDescriptionBox.Text = SelectedMod.Info.Description;
    }
    private Mod? GetModFromModList(int index)
    {
        if (index < 0 || index >= ModList.Items.Count) return null;
        object? modname_o = ModList.Items[index];
        if (modname_o == null)
            return null;
        string modname = (string)modname_o;
        return Application.Instance.ModDB.FindModByName(modname);
    }
    private void ModList_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        if (SelectedMod == null) return;
        switch (e.NewValue)
        {
            case CheckState.Checked:
                SelectedMod.Enable();
                break;
            default:
                SelectedMod.Disable();
                break;
        }
    }
    private void ExtractSelectedFiles()
    {
        DialogResult result = ExtractionSelection.ShowDialog(Owner);
        if (result != DialogResult.OK) return;
        foreach (string file in ExtractionSelection.FileNames)
        {
            string? dirpath = Path.GetDirectoryName(file);
            if (dirpath == null) return;
            string output = Path.Combine(dirpath, Path.GetFileNameWithoutExtension(file));
            if (file.EndsWith(".tfhres"))
                TFHResourceUtils.Extract(file, output);
            if (file.EndsWith(".gfs"))
                GFS.Utils.Extract(file, output);
        }
    }
    private void InstallFromFolder()
    {
        SelectFolder.Description = Velvet.Velvetify("Select Mod Folder");
        DialogResult result = SelectFolder.ShowDialog();
        if (result != DialogResult.OK) return;
        if (!Mod.IsMod(SelectFolder.SelectedPath))
        {
            Velvet.ShowMessageBox("This folder is not a valid mod", "Invalid Mod Folder");
            return;
        }
        new DownloadForm(SelectFolder.SelectedPath).ShowDialog(Owner);
    }
    private void UninstallMod_Click(object sender, EventArgs e)
    {
        if (SelectedMod == null)
        {
            Velvet.ShowMessageBox("You need to select a mod first");
            return;
        }
        DialogResult result = Velvet.ShowMessageBox("Are you sure you want to uninstall " + SelectedMod.Info.Name + "?", "Uninstall " + SelectedMod.Info.Name + "?", MessageBoxButtons.YesNo);
        if (result != DialogResult.Yes) return;
        Directory.Delete(SelectedMod.Folder, true);
        Velvet.ShowMessageBox("Uninstalled mod!");
        RefreshModList();
    }
    private void VisitModPage_Click(object sender, EventArgs e)
    {
        if (SelectedMod == null)
        {
            Velvet.ShowMessageBox("You need to select a mod first");
            return;
        }
        string? url = SelectedMod.Info.Url;
        if (url == null)
        {
            Velvet.ShowMessageBox("This mod has no link assigned!");
            return;
        }
        if (!Url.IsUrl(url))
        {
            Velvet.ShowMessageBox("The url assigned to this mod is invalid. You should contact the mod creator to fix it!");
            return;
        }
        Url.OpenUrl(url);
    }
}
