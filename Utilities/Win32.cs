using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Win32
{
    public static string Scheme {get;} = "velvetbeautifier";
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AttachConsole(int dwProcessId);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();
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
}