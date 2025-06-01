using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;
public static class MenuBar
{
    public static Terminal.Gui.MenuBar Create(MainTopLevel window)
    {
        return new([
            new MenuBarItem("_File",[
                new MenuItem("_Refresh","Refresh list",window.RefreshModList),
                new MenuItem("_Apply","Apply enabled mods",window.ApplyMods),
                new MenuItem("_Revert","Remove all modifications from the game",window.Revert),
                new MenuItem("_Launch","Launch the game with modifications",window.LaunchGame),
                new MenuBarItem("_Install mods...",[
                    new MenuItem("from files","Install mods from files",window.InstallModFromFiles),
                    new MenuItem("from folders","Install mods from folders",window.InstallModFromFolders),
                    new MenuItem("from text","Install mods from text input (URL, GameBanana URL/ID)",window.InstallModFromText)
                ]),
                new MenuItem("E_xit","",() => Application.RequestStop())
            ]),
            new MenuBarItem("_Tools",[
                new MenuItem("_Extract","Extract .gfs/.tfhres files",window.ExtractFiles),
                new MenuBarItem("_Create...",[
                    new MenuItem("_Reverge Package","Create a Reverge Package file from a folder",window.CreateGFS),
                    new MenuItem("empty _TFHResource","Create a empty TFHResource file",window.CreateTFHRES),
                    new MenuItem("_Mod","Create a new empty mod",window.CreateMod)
                ]),
                new MenuItem("Edit _Configuration",$"Edit the configuration file of {Velvet.NAME}",window.EditConfig),
                new MenuBarItem("_Delete...",[
                    new MenuItem("_backups","",window.DeleteBackups),
                    new MenuItem("_mods","",window.DeleteMods),
                    new MenuItem("_everything","",window.DeleteEverything)
                ])
            ]),
            new MenuBarItem("_Help",[
                new MenuItem("_How to use this?","Opens a link to a guide on how to use Velvet Beautifier",() => Url.OpenUrl(GitHubUtilities.GUI_GUIDE_URL)),
                new MenuItem("Report _issue","Report bugs/issues to GitHub",() => Url.OpenUrl(GitHubUtilities.BUG_REPORT_URL)),
                new MenuItem("Request _feature","Suggest a new feature for Velvet Beautifier",() => Url.OpenUrl(GitHubUtilities.FEATURE_REQUEST_URL)),
                new MenuItem("_About","",AboutDialog.Show)
            ])
        ]);
    }
}