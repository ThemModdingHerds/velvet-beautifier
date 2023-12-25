namespace ThemModdingHerds.VelvetBeautifier;
public partial class DownloadForm : Form
{
    private string? GameBananaUrl { get; }
    private string? UnzippedPath { get; }
    private DownloadType DType {get;}
    private string ModName {get;}
    private string ModAuthor {get;}
    private string Description {get;}
    public DownloadForm(GameBananaMod mod, string url)
    {
        ModName = mod.ModName;
        ModAuthor = mod.OwnerName;
        Description = mod.Body;
        InitializeComponent();
        DType = DownloadType.GameBanana;
        GameBananaUrl = url;
        Text = "Install " + mod.ModName + '?';
        ModNameLabel.Text = mod.ModName;
        ModAuthorLabel.Text = "by " + mod.OwnerName;
        ModDescriptionBox.Text = mod.Body;
    }
    public DownloadForm(Mod mod, string unzippedpath)
    {
        ModName = mod.Info.Name;
        ModAuthor = mod.Info.Author;
        Description = mod.Info.Description;
        InitializeComponent();
        DType = DownloadType.ZipFile;
        UnzippedPath = unzippedpath;
        Text = "Install " + mod.Info.Name + '?';
        ModNameLabel.Text = mod.Info.Name;
        ModAuthorLabel.Text = "by " + mod.Info.Author;
        ModDescriptionBox.Text = mod.Info.Description;
    }
    public DownloadForm(GameBananaMod mod) : this(mod, mod.GetLatestUpdate().DownloadUrl)
    {

    }

    private void InstallButton_Click(object sender, EventArgs e)
    {
        switch(DType)
        {
            case DownloadType.GameBanana:
                InstallGameBanana();
                break;
            case DownloadType.ZipFile:
                InstallZipfile();
                break;
        }
        Close();
    }
    private static bool ModCheck(string folder)
    {
        if(Mod.IsMod(folder)) return true;
        Velvet.ShowMessageBox("The mod you downloaded is not a valid mod","Invalid mod");
        Directory.Delete(folder,true);
        return false;
    }
    private void InstallZipfile()
    {
        string destFolder = Path.Combine(Config.Current.ModsFolder,ModName);
        if(UnzippedPath == null) return;
        if(!ModCheck(UnzippedPath)) return;
        Utils.CopyFilesTo(UnzippedPath,destFolder);
        Velvet.ShowMessageBox("Zip Mod has been downloaded and installed. You might have to refresh the mods list","Installed Zip Mod");
    }
    private async void InstallGameBanana()
    {
        if(GameBananaUrl == null) return;
        string destFolder = Path.Combine(Config.Current.ModsFolder,ModName);
        string unzippedpath = await DownloadManager.GetAndUnzip(GameBananaUrl);
        if(!ModCheck(unzippedpath)) return;
        Utils.CopyFilesTo(unzippedpath,destFolder);
        Velvet.ShowMessageBox("GameBanana Mod has been downloaded and installed. You might have to refresh the mods list","Installed GameBanana Mod");
    }
}
public enum DownloadType
{
    GameBanana,
    ZipFile
}