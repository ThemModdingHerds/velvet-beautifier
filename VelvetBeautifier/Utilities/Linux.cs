namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods for Linux related stuff
/// </summary>
public static class Linux
{
    public const string DESKTOP_GENERIC_NAME = "Mod Loader/Tool";
    public static string[] DESKTOP_CATEGORIES => ["Utility","Compression","GTK"];
    public static string[] DESKTOP_KEYWORDS => [];
    public static string HOME => Environment.GetEnvironmentVariable("HOME") ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static string? CreateDesktopEntry()
    {
        string? exePath = Dotnet.ExecutablePath;
        if(exePath == null) return null;
        string exeFolder = Dotnet.ExecutableFolder;
        return string.Join('\n',[
            "[Desktop Entry]",
            "Type=Application",
            $"Name={Velvet.NAME}",
            $"GenericName={DESKTOP_GENERIC_NAME}",
            $"Comment={Velvet.DESCRIPTION}",
            $"Exec={exePath} --gui",
            $"Path={exeFolder}",
            "Terminal=true",
            $"Categories={string.Join(';',DESKTOP_CATEGORIES)};",
            $"Keywords={string.Join(';',DESKTOP_KEYWORDS)};",
            "SingleMainWindow=true"
        ]);
    }
    public static void InstallDesktopEntry()
    {
        string applicationsFolder = Path.Combine(HOME,".local","share","applications");
        string filename = $"{Velvet.NAME}.desktop";
        string filepath = Path.Combine(applicationsFolder,filename);
        string? desktop = CreateDesktopEntry();
        if(desktop == null) return;
        if(File.Exists(filepath))
            File.Delete(filepath);
        File.WriteAllText(filepath,desktop);
    }
}