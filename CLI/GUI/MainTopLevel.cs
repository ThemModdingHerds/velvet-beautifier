using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.CLI.GUI;

public class MainTopLevel : Toplevel
{
    private readonly ModListView modList = new();
    private readonly FrameView left = new("Mods")
    {
        X = 0,
        Y = 1,
        Width = 25,
        Height = Dim.Fill()
    };
    private readonly FrameView right = new("Description")
    {
        X = 25,
        Y = 1,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    public MainTopLevel()
    {
        MenuBar = GUI.MenuBar.Create(this);
        Add(MenuBar);

        left.Add(modList);
        Add(left);

        Add(right);
    }
    public void RefreshModList()
    {
        modList.Refresh();
    }
    public void ApplyMods()
    {
        ModDB.Apply();
        MessageBox.Query("Applied mods", "Mods have been applied! You can now launch the game", "Ok");
    }
    public void Revert()
    {
        BackupManager.Revert();
        MessageBox.Query("Reverted", "Reverted back to orignal game, no mods applied", "Ok");
    }
    public void LaunchGame()
    {
        if (ModLoaderTool.Client == null)
        {
            MessageBox.ErrorQuery("No client", "No client has been provided in your configuration! Check if they are available/correct!", "Ok");
            return;
        }
        ModLoaderTool.Client.Launch();
    }
    public void InstallModFromFiles()
    {
        OpenDialog dialog = new("Install mods", "Select files to install as mods", null, OpenDialog.OpenMode.File);
        Application.Run(dialog);
        if (dialog.FilePaths.Count == 0) return;
        int installed = 0;
        foreach (string filepath in dialog.FilePaths)
        {
            ModInstallResult result = ModDB.InstallMod(filepath);
            if (result != ModInstallResult.Ok)
                continue;
            installed++;
        }
        MessageBox.Query("Installed mods", $"{installed} mods have been installed!", "Ok");
    }
    public void InstallModFromFolders()
    {
        OpenDialog dialog = new("Install mods", "Select folders to install as mods", null, OpenDialog.OpenMode.Directory);
        Application.Run(dialog);
        if (dialog.FilePaths.Count == 0) return;
        int installed = 0;
        foreach (string filepath in dialog.FilePaths)
        {
            ModInstallResult result = ModDB.InstallMod(filepath);
            if (result != ModInstallResult.Ok)
                continue;
            installed++;
        }
        MessageBox.Query("Installed mods", $"{installed} mods have been installed!", "Ok");
    }
    public void InstallModFromText()
    {

    }
    public void ExtractFiles()
    {

    }
    public void CreateGFS()
    {

    }
    public void CreateTFHRES()
    {

    }
    public void CreateMod()
    {

    }
    public void EditConfig()
    {

    }
    public void DeleteBackups()
    {
        int result = MessageBox.Query("Delete backups?", "Are you sure you want to delete the backup files?", "Yes", "No");
        if (result == 0)
        {
            BackupManager.Clear();
            MessageBox.Query("Deleted backups", "The backup files have been deleted", "Ok");
        }
    }
    public void DeleteMods()
    {
        int result = MessageBox.Query("Delete mods?", "Are you sure you want to delete all mods?", "Yes", "No");
        if (result == 0)
        {
            BackupManager.Revert();
            ModDB.Clear();
            MessageBox.Query("Deleted mods", "The mods have been deleted", "Ok");
        }
    }
    public void DeleteEverything()
    {
        int result = MessageBox.Query("Delete everything?", "Are you sure you want to delete everything?", "Yes", "No");
        if (result == 0)
        {
            BackupManager.Revert();
            ModLoaderTool.DeleteEverything();
            MessageBox.Query("Deleted everything", "Everything has been deleted! The software will close after this window!", "Ok");
            Application.RequestStop();
        }
    }
}