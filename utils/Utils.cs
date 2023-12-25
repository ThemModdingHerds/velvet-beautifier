using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Microsoft.Win32;

namespace ThemModdingHerds.VelvetBeautifier;
public static class Utils
{
    public static string Scheme {get;} = "velvetbeautifier";
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
    public static void CopyFilesTo(string input,string output)
    {
        foreach(string dirpath in Directory.GetDirectories(input,"*",SearchOption.AllDirectories))
            Directory.CreateDirectory(dirpath.Replace(input,output));
        foreach(string filepath in Directory.GetFiles(input,"*.*",SearchOption.AllDirectories))
            File.Copy(filepath,filepath.Replace(input,output),true);
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
    public static string CreateTempFile()
    {
        string folder = Directory.CreateTempSubdirectory().FullName;
        string path = Guid.NewGuid().ToString();
        string tempfile = Path.Combine(folder,path);
        File.Create(tempfile).Close();
        Velvet.ConsoleWriteLine("Created temp file from " + path + " to " + tempfile);
        return tempfile;
    }
    public static void CreateURIScheme()
    {
        string name = "Velvet Beautifier";
        string path = Application.ExecutablePath;
        string regpath = "SOFTWARE\\CLASSES\\" + Scheme;

        RegistryKey key = Registry.CurrentUser.OpenSubKey(regpath,true) ?? Registry.CurrentUser.CreateSubKey(regpath);
        key.SetValue("","URL:" + name);
        key.SetValue("URL Protocol",Scheme);
        
        RegistryKey iconKey = Registry.CurrentUser.OpenSubKey("DefaultIcon",true) ?? key.CreateSubKey("DefaultIcon");
        iconKey.SetValue("",'"' + path + "\",1");
        iconKey.Close();

        RegistryKey openKey = Registry.CurrentUser.OpenSubKey(@"shell\open\command",true) ?? key.CreateSubKey(@"shell\open\command");
        openKey.SetValue("","\"" + path + "\" \"%1\"");
        openKey.Close();

        key.Close();
        Velvet.ShowMessageBox("Registered URI Scheme");
    }
    public static bool IsUrl(string url)
    {
        return Uri.TryCreate(url,UriKind.RelativeOrAbsolute,out Uri? _);
    }
    public static void ExtractZip(string path,string output)
    {
        ZipFile.ExtractToDirectory(path,output);
    }
    public static void SavePID()
    {
        Process cur = Process.GetCurrentProcess();
        File.WriteAllText(Path.Combine(Environment.CurrentDirectory,".pid"),cur.Id.ToString());
    }
    public static bool HasPID()
    {
        return File.Exists(Path.Combine(Environment.CurrentDirectory,".pid"));
    }
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AttachConsole(int dwProcessId);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();
}