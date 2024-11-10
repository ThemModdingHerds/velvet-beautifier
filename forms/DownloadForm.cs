using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Forms;
public partial class DownloadForm : Form
{
    private MainForm MainForm {get => (MainForm?)ParentForm ?? throw new VelvetException("DownloadForm.MainForm","MainForm is null");}
    private string? DownloadLink { get; }
    private string? FolderPath { get; }
    private string ModName {get;}
    private string ModAuthor {get;}
    private string Description {get;}
    public DownloadForm(GameBanana.Mod mod, string url)
    {
        ModName = mod.ModName;
        ModAuthor = mod.OwnerName;
        Description = mod.Body;
        InitializeComponent();
        DownloadLink = url;
        Text = "Install " + mod.ModName + '?';
        ModNameLabel.Text = mod.ModName;
        ModAuthorLabel.Text = "by " + mod.OwnerName;
        ModDescriptionBox.Text = mod.Body;
    }
    public DownloadForm(Mod mod, string mod_path)
    {
        ModName = mod.Info.Name;
        ModAuthor = mod.Info.Author;
        Description = mod.Info.Description;
        InitializeComponent();
        FolderPath = mod_path;
        Text = "Install " + mod.Info.Name + '?';
        ModNameLabel.Text = mod.Info.Name;
        ModAuthorLabel.Text = "by " + mod.Info.Author;
        ModDescriptionBox.Text = mod.Info.Description;
    }
    public DownloadForm(string path): this(new Mod(path),path)
    {

    }
    public DownloadForm(GameBanana.Mod mod) : this(mod, mod.GetLatestUpdate().DownloadUrl)
    {

    }

    private void InstallButton_Click(object sender, EventArgs e)
    {
        if(DownloadLink != null)
            InstallFromLink();
        else if(FolderPath != null)
            InstallFromFolder();
        Close();
    }
    private static bool ModCheck(string folder)
    {
        if(Mod.IsMod(folder)) return true;
        VelvetForms.ShowMessageBox("The mod you downloaded is not a valid mod","Invalid mod");
        Directory.Delete(folder,true);
        return false;
    }
    private void InstallFromFolder(string folder)
    {
        if(!ModCheck(folder)) return;
        ModInfo info = ModInfo.Read(Path.Combine(folder,Mod.MODINFO_NAME));
        string destFolder = Path.Combine(MainForm.App.ModDB.Folder,info.Id);
        Utilities.FileSystem.CopyFolder(folder,destFolder);
        VelvetForms.ShowMessageBox("Mod has been installed!","Installed Mod");
        MainForm.RefreshModList();
    }
    private void InstallFromFolder()
    {
        if(FolderPath == null) return;
        InstallFromFolder(FolderPath);
    }
    private async void InstallFromLink()
    {
        if(DownloadLink == null) return;
        string unzippedpath = await DownloadManager.GetAndUnzip(DownloadLink);
        InstallFromFolder(unzippedpath);
    }
}