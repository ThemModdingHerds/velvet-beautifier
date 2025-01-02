using Gtk;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class MenuBarItems
{
    public MenuBarItems(Builder builder,MainWindow window)
    {
        InitFile(builder,window);
        InitTools(builder,window);
        InitHelp(builder);
    }
    MenuItem? FileRefresh;
    MenuItem? FileApply;
    MenuItem? FileRevert;
    MenuItem? FileLaunch;
    MenuItem? FileExit;
    private void InitFile(Builder builder,MainWindow window)
    {
        FileRefresh = new(builder.GetRawOwnedObject("MenuFileRefresh"));
        FileApply = new(builder.GetRawOwnedObject("MenuFileApply"));
        FileRevert = new(builder.GetRawOwnedObject("MenuFileRevert"));
        FileLaunch = new(builder.GetRawOwnedObject("MenuFileLaunch"));
        FileExit = new(builder.GetRawOwnedObject("MenuFileExit"));

        FileRefresh.Activated += delegate {window.RefreshModList();};
        FileApply.Activated += delegate {window.ApplyMods();};
        FileRevert.Activated += delegate {window.Revert();};
        FileLaunch.Activated += delegate {window.Launch();};
        InitFileInstall(builder,window);
        FileExit.Activated += delegate {Application.Quit();};
    }
    MenuItem? FileInstallFile;
    MenuItem? FileInstallFolders;
    MenuItem? FileInstallText;
    private void InitFileInstall(Builder builder,MainWindow window)
    {
        FileInstallFile = new(builder.GetRawOwnedObject("MenuFileInstallFile"));
        FileInstallFolders = new(builder.GetRawOwnedObject("MenuFileInstallFolders"));
        FileInstallText = new(builder.GetRawOwnedObject("MenuFileInstallText"));

        FileInstallFile.Activated += delegate
        {
            string[] files = window.OpenFileDialog("Select files to install",[Utils.GFSFilter,Utils.ZipFilter,Utils.RarFilter,Utils.SevenZipFilter,Utils.TarFilter,Utils.GZipFilter],true);
            if(files.Length == 0) return;
            int count = 0;
            foreach(string file in files)
            {
                ModInstallResult result = ModDB.InstallMod(file);
                switch(result)
                {
                    case ModInstallResult.AlreadyExists:
                    case ModInstallResult.Ok:
                        count++;
                        break;
                }
            }
            window.ShowMessageBox($"Installed {count}/{files.Length} mods");
            window.RefreshModList();
        };
        FileInstallFolders.Activated += delegate
        {
            string[] folders = window.OpenFolderDialog("Select folders to install",true);
            if(folders.Length == 0) return;
            int count = 0;
            foreach(string folder in folders)
            {
                ModInstallResult result = ModDB.InstallMod(folder);
                switch(result)
                {
                    case ModInstallResult.AlreadyExists:
                    case ModInstallResult.Ok:
                        count++;
                        break;
                }
            }
            window.ShowMessageBox($"Installed {count}/{folders.Length} mods");
            window.RefreshModList();
        };
        FileInstallText.Activated += delegate
        {
            InstallModTextWindow installModTextWindow = new();
            installModTextWindow.Show();
        };
    }
    MenuItem? ToolsExtract;
    MenuItem? ToolsCreateGFS;
    MenuItem? ToolsCreateTFHRES;
    MenuItem? ToolsCreateMod;
    MenuItem? ToolsEditConfig;
    MenuItem? ToolsDesktopShortcut;
    MenuItem? ToolsMenuShortcut;
    private void InitTools(Builder builder,MainWindow window)
    {
        ToolsExtract = new(builder.GetRawOwnedObject("MenuToolsExtract"));
        ToolsCreateGFS = new(builder.GetRawOwnedObject("MenuToolsCreateGFS"));
        ToolsCreateTFHRES = new(builder.GetRawOwnedObject("MenuToolsCreateTFHRES"));
        ToolsCreateMod = new(builder.GetRawOwnedObject("MenuToolsCreateMod"));
        ToolsEditConfig = new(builder.GetRawOwnedObject("MenuToolsEditConfig"));
        InitDelete(builder,window);
        ToolsDesktopShortcut = new(builder.GetRawOwnedObject("MenuToolsDesktopShortcut"));
        ToolsMenuShortcut = new(builder.GetRawOwnedObject("MenuToolsMenuShortcut"));

        ToolsExtract.Activated += delegate
        {
            string[] files = window.OpenFileDialog(Velvet.Velvetify("Select file to extract"),[Utils.GFSFilter,Utils.TFHRESFilter]);
            if(files.Length != 1) return;
            string file = files[0];
            string[] folders = window.OpenFolderDialog(Velvet.Velvetify("Select the folder to output"));
            if(folders.Length != 1) return;
            string output = folders[0];
            ModLoaderTool.Extract(file,output);
            ResponseType result = window.ShowMessageBox("Finished extracting! Do you want to open the output folder?",ButtonsType.YesNo,MessageType.Question);
            if(result == ResponseType.Yes)
                FileSystem.OpenFolder(output);
        };
        ToolsCreateGFS.Activated += delegate
        {
            string[] folders = window.OpenFolderDialog(Velvet.Velvetify("Select a folder to turn into a Reverge Package file"));
            if(folders.Length != 1) return;
            string input = folders[0];
            string? output = window.SaveFileDialog(Velvet.Velvetify("Save Reverge Package file"),[Utils.GFSFilter]);
            bool result = GFS.Utils.Create(input,output);
            if(!result)
                window.ShowMessageBox("Failed to create Reverge Package file! Report this!",MessageType.Error);
        };
        ToolsCreateTFHRES.Activated += delegate
        {
            string? input = window.SaveFileDialog(Velvet.Velvetify("Save empty TFHResource file"),[Utils.TFHRESFilter]);
            if(input == null) return;
            bool result = TFHResource.Utils.CreateEmpty(input);
            if(!result)
                window.ShowMessageBox("Failed to create TFHResource file! Report this!",MessageType.Error);
        };
        ToolsCreateMod.Activated += delegate
        {
            CreateModWindow createModWindow = new();
            createModWindow.Show();
        };
        ToolsEditConfig.Activated += delegate
        {
            EditConfigurationWindow editConfigurationWindow = new();
            editConfigurationWindow.Show();
        };
        ToolsDesktopShortcut.Activated += delegate {Utils.CreateDesktopShortcut();};
        ToolsMenuShortcut.Activated += delegate {Utils.CreateMenuShortcut();};
        // remove if impl
        ToolsDesktopShortcut.Destroy();
        ToolsMenuShortcut.Destroy();
    }
    MenuItem? ToolsDeleteBackups;
    MenuItem? ToolsDeleteMods;
    MenuItem? ToolsDeleteEverything;
    private void InitDelete(Builder builder,MainWindow window)
    {
        static bool DoIt(Window window) => window.ShowMessageBox("Do you want to Proceed?",ButtonsType.YesNo,MessageType.Warning) == ResponseType.Yes;

        ToolsDeleteBackups = new(builder.GetRawOwnedObject("MenuToolsDeleteBackups"));
        ToolsDeleteMods = new(builder.GetRawOwnedObject("MenuToolsDeleteMods"));
        ToolsDeleteEverything = new(builder.GetRawOwnedObject("MenuToolsDeleteEverything"));

        ToolsDeleteBackups.Activated += delegate
        {
            if(!DoIt(window)) return;
            BackupManager.Clear();
            window.ShowMessageBox("Deleted all backup files! The software will close after this window!");
            Application.Quit();
        };
        ToolsDeleteMods.Activated += delegate
        {
            if(!DoIt(window)) return;
            BackupManager.Revert();
            ModDB.Clear();
            window.ShowMessageBox("Deleted all mods!");
            window.RefreshModList();
        };
        ToolsDeleteEverything.Activated += delegate
        {
            if(!DoIt(window)) return;
            BackupManager.Revert();
            ModLoaderTool.DeleteEverything();
            window.ShowMessageBox("Deleted everything! The software will close after this window!");
            Application.Quit();
        };
    }
    MenuItem? HelpGuide;
    MenuItem? HelpReportIssue;
    MenuItem? HelpRequestFeature;
    MenuItem? HelpAbout;
    private void InitHelp(Builder builder)
    {
        HelpGuide = new(builder.GetRawOwnedObject("MenuHelpGuide"));
        HelpReportIssue = new(builder.GetRawOwnedObject("MenuHelpReportIssue"));
        HelpRequestFeature = new(builder.GetRawOwnedObject("MenuHelpRequestFeature"));
        HelpAbout = new(builder.GetRawOwnedObject("MenuHelpAbout"));

        HelpGuide.Activated += delegate {Url.OpenUrl(GitHub.GitHub.GUI_GUIDE_URL);};
        HelpReportIssue.Activated += delegate {Url.OpenUrl(GitHub.GitHub.BUG_REPORT_URL);};
        HelpRequestFeature.Activated += delegate {Url.OpenUrl(GitHub.GitHub.FEATURE_REQUEST_URL);};
        HelpAbout.Activated += delegate
        {
            AboutDialog dialog = VelvetGtk.CreateAboutDialog();
            dialog.Run();
            dialog.Destroy();
        };
    }
}