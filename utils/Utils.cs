using System.Diagnostics;
using System.Runtime.InteropServices;
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
    public static void OpenLink(string link)
    {
        Process.Start(new ProcessStartInfo(link){UseShellExecute = true});
    }
    public static List<string> GetAllFiles(string folder)
    {
        if(!Directory.Exists(folder))
            return [];
        return Directory.EnumerateFiles(folder,"*.*",SearchOption.AllDirectories).ToList();
    }
    public static string CreateTempFile(string path)
    {
        if(!File.Exists(path))
            throw new VelvetException("Utils.CreateTempFile",path + " does not exist");
        string folder = Directory.CreateTempSubdirectory().FullName;
        string filename = Path.GetFileName(path);
        string tempfile = Path.Combine(folder,filename);
        File.Copy(path,tempfile,true);
        Velvet.ConsoleWriteLine("Created temp file from " + path + " to " + tempfile);
        return tempfile;
    }
    [DllImport("kernel32.dll",SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AllocConsole();
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AttachConsole(int dwProcessId);
}