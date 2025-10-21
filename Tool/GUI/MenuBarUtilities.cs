using System.Runtime.InteropServices;
using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public static class MenuBarUtilities
{
    public static MenuBar Create(MainTopLevel mainTopLevel)
    {
        return new([
            new MenuBarItem("_File",[
                new MenuItem("_Refresh","Refresh list",mainTopLevel.modList.Refresh),
                new MenuBarItem("_Show",[
                    new MenuItem("_Local mods","Show local installed mods",() => mainTopLevel.modList.Refresh(ModListView.Mode.Local)),
                    new MenuItem("_Online","Show mods available online",() => mainTopLevel.modList.Refresh(ModListView.Mode.Online))
                ]),
                new MenuItem("_Apply","Apply enabled mods",ActionDialog.Show(ActionDialog.Type.Apply)),
                new MenuItem("_Revert","Remove all modifications from the game",ActionDialog.Show(ActionDialog.Type.Revert)),
                new MenuItem("_Launch","Launch the game with modifications",ActionDialog.Show(ActionDialog.Type.LaunchClient)),
                new MenuBarItem("_Install mods...",[
                    new MenuItem("from files","Install mods from files",InstallDialog.Show(InstallDialog.Type.Files)),
                    new MenuItem("from folders","Install mods from folders",InstallDialog.Show(InstallDialog.Type.Folders)),
                    new MenuItem("from text","Install mods from text input (URL, GameBanana URL/ID)",InstallDialog.Show(InstallDialog.Type.Text))
                ]),
                new MenuItem("E_xit","",() => Application.RequestStop())
            ]),
            new MenuBarItem("_Tools",[
                new MenuItem("_Extract","Extract .gfs/.tfhres files",ActionDialog.Show(ActionDialog.Type.ExtractFiles)),
                new MenuBarItem("_Create...",[
                    new MenuItem("_Mod","Create a new empty mod",CreateDialog.Show(CreateDialog.Type.Mod)),
                    new MenuItem("_Reverge Package","Create a Reverge Package file from a folder",CreateDialog.Show(CreateDialog.Type.RevergePackage)),
                    new MenuItem("empty _TFHResource","Create a empty TFHResource file",CreateDialog.Show(CreateDialog.Type.TFHResource))
                ]),
                new MenuItem("Edit _Configuration",$"Edit the configuration file of {Velvet.NAME}",ConfigDialog.Show),
                new MenuBarItem("_Delete...",[
                    new MenuItem("_backups","",DeleteDialog.Show(DeleteDialog.Type.Backup)),
                    new MenuItem("_mods","",DeleteDialog.Show(DeleteDialog.Type.Mods)),
                    new MenuItem("_everything","",DeleteDialog.Show(DeleteDialog.Type.Everything))
                ]),
                new MenuItem("Install _Menu Shortcut","Create a shortcut in the OS menu",CreateMenuShortcut)
            ]),
            new MenuBarItem("_Help",[
                new MenuItem("_How to use this?","Opens a link to a guide on how to use Velvet Beautifier",() => Url.OpenUrl(GitHubUtilities.GUI_GUIDE_URL)),
                new MenuItem("Report _issue","Report bugs/issues to GitHub",() => Url.OpenUrl(GitHubUtilities.BUG_REPORT_URL)),
                new MenuItem("Request _feature","Suggest a new feature for Velvet Beautifier",() => Url.OpenUrl(GitHubUtilities.FEATURE_REQUEST_URL)),
                new MenuItem("_About","",AboutDialog.Show)
            ])
        ]);
    }
    private static void CreateMenuShortcut()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Win32.InstallMenuShortcut();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            Linux.InstallDesktopEntry();
    }
}