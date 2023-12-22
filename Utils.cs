using Microsoft.Win32;

namespace ThemModdingHerds.VelvetBeautifier;
public static class Utils
{
    public static string GetSteamPath()
    {
        string x32 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam";
        string x64 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
        string defaultPath = "C:\\Program Files (x86)\\Steam";
        string regPath = Environment.Is64BitOperatingSystem ? x64 : x32;
        string? path = (string?)Registry.GetValue(regPath,"InstallPath",defaultPath);
        if(path == null) return defaultPath;
        return path;
    }
    public static string GetDefaultTFHPath()
    {
        return Path.Combine(GetSteamPath(),"steamapps","common","Them's Fightin' Herds");
    }
}