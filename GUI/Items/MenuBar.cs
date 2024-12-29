using Gtk;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public static class MenuBarItems
{

    private static Menu GetFileMenu(MenuBar menuBar) => (Menu)((MenuItem)menuBar.Children[0]).Submenu;
    private static Menu GetToolsMenu(MenuBar menuBar) => (Menu)((MenuItem)menuBar.Children[1]).Submenu;
    private static Menu GetHelpMenu(MenuBar menuBar) => (Menu)((MenuItem)menuBar.Children[2]).Submenu;
    public static void Init(MenuBar menuBar,MainWindow window)
    {
        InitFile(GetFileMenu(menuBar),window);
        InitTools(GetToolsMenu(menuBar),window);
        InitHelp(GetHelpMenu(menuBar),window);
    }
    private static void InitFile(Menu menu,MainWindow window)
    {
        MenuItem refresh = (MenuItem)menu.Children[0];
        MenuItem apply = (MenuItem)menu.Children[1];
        MenuItem revert = (MenuItem)menu.Children[2];
        MenuItem launch = (MenuItem) menu.Children[3];
        Menu install = (Menu)((MenuItem)menu.Children[4]).Submenu;
        MenuItem exit = (MenuItem)menu.Children[6];

        refresh.Activated += delegate {window.RefreshModList();};
        apply.Activated += delegate {window.ApplyMods();};
        revert.Activated += delegate {window.Revert();};
        launch.Activated += delegate {window.Launch();};
        InitFileInstall(install,window);
        exit.Activated += delegate {Application.Quit();};
    }
    private static void InitFileInstall(Menu menu,MainWindow window)
    {
        MenuItem file = (MenuItem)menu.Children[0];
        MenuItem folders = (MenuItem)menu.Children[1];
        MenuItem text = (MenuItem)menu.Children[2];

        file.Activated += delegate
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
        folders.Activated += delegate
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
        text.Activated += delegate
        {
            InstallModTextWindow installModTextWindow = [];
            installModTextWindow.Show();
        };
    }
    private static void InitTools(Menu menu,MainWindow window)
    {
        MenuItem extract = (MenuItem)menu.Children[0];
        MenuItem createGFS = (MenuItem)menu.Children[1];
        MenuItem createTFHRES = (MenuItem)menu.Children[2];
        MenuItem createMod = (MenuItem)menu.Children[3];

        extract.Activated += delegate
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
        createGFS.Activated += delegate
        {
            string[] folders = window.OpenFolderDialog(Velvet.Velvetify("Select a folder to turn into a Reverge Package file"));
            if(folders.Length != 1) return;
            string input = folders[0];
            string? output = window.SaveFileDialog(Velvet.Velvetify("Save Reverge Package file"),[Utils.GFSFilter]);
            bool result = GFS.Utils.Create(input,output);
            if(!result)
                window.ShowMessageBox("Failed to create Reverge Package file! Report this!",MessageType.Error);
        };
        createTFHRES.Activated += delegate
        {
            string? input = window.SaveFileDialog(Velvet.Velvetify("Save empty TFHResource file"),[Utils.TFHRESFilter]);
            bool result = TFHResource.Utils.CreateEmpty(input);
            if(!result)
                window.ShowMessageBox("Failed to create TFHResource file! Report this!",MessageType.Error);
        };
        createMod.Activated += delegate
        {
            CreateModWindow createModWindow = [];
            createModWindow.Show();
        };
    }
    private static void InitHelp(Menu menu,MainWindow window)
    {
        MenuItem guide = (MenuItem)menu.Children[0];
        MenuItem reportIssue = (MenuItem)menu.Children[2];
        MenuItem requestFeature = (MenuItem)menu.Children[3];
        MenuItem about = (MenuItem) menu.Children[5];

        guide.Activated += delegate {Url.OpenUrl(Velvet.GITHUB_REPO_GUI_GUIDE_URL);};
        reportIssue.Activated += delegate {Url.OpenUrl(Velvet.GITHUB_REPO_BUG_REPORT_URL);};
        requestFeature.Activated += delegate {Url.OpenUrl(Velvet.GITHUB_REPO_FEATURE_REQUEST_URL);};
        about.Activated += delegate
        {
            AboutDialog dialog = VelvetGtk.CreateAboutDialog();
            dialog.Run();
            dialog.Destroy();
        };
    }
}