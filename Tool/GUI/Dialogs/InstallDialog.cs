using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;

public static class InstallDialog
{
    public enum Type
    {
        Files,
        Folders,
        Text
    }
    public static Action Show(Type type)
    {
        return type switch
        {
            Type.Files => InstallModFromFiles,
            Type.Folders => InstallModFromFolders,
            Type.Text => InstallTextDialog.Show,
            _ => throw new Exception(),
        };
    }
    private static void InstallModFromFiles()
    {
        OpenDialog dialog = new("Install mods", "Select files to install as mods", null, OpenDialog.OpenMode.File)
        {
            AllowsMultipleSelection = true
        };
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
    private static void InstallModFromFolders()
    {
        OpenDialog dialog = new("Install mods", "Select folders to install as mods", null, OpenDialog.OpenMode.Directory)
        {
            AllowsMultipleSelection = true
        };
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
}
public class InstallTextDialog : Dialog
{
    private class InstallButton : Button
    {
        public InstallButton(Action onInstall) : base("Install", true)
        {
            Clicked += onInstall;
        }
    }
    private class CancelButton : Button
    {
        public CancelButton() : base("Cancel")
        {
            Clicked += () => Application.RequestStop();
        }
    }
    private readonly TextField _input = new()
    {
        X = 1,
        Y = 1,
        Width = Dim.Fill(1),
        Height = 1
    };
    private readonly InstallButton _install;
    public static void Show() => Application.Run(new InstallTextDialog());
    public InstallTextDialog(): base("Install mod from text field",35,6)
    {
        _install = new(OnInstall);
        Add(_input);
        AddButton(_install);
        AddButton(new CancelButton());
    }
    private void OnInstall()
    {
        ModDB.InstallMod(_input.Text.ToString());
        Application.RequestStop();
    }
}