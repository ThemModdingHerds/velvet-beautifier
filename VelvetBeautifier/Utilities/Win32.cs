using System.Runtime.InteropServices;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Win32
{
    public const string Scheme = "velvetbeautifier";
    public static bool CreateURIScheme()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string? path = Dotnet.ExecutablePath;
            if(path == null) return false;
            string regpath = "SOFTWARE\\CLASSES\\" + Scheme;
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regpath,true) ?? Microsoft.Win32.Registry.CurrentUser.CreateSubKey(regpath);
            key.SetValue("","URL:" + Velvet.NAME);
            key.SetValue("URL Protocol",Scheme);
            
            Microsoft.Win32.RegistryKey iconKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("DefaultIcon",true) ?? key.CreateSubKey("DefaultIcon");
            iconKey.SetValue("",'"' + path + "\",1");
            iconKey.Close();

            Microsoft.Win32.RegistryKey openKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"shell\open\command",true) ?? key.CreateSubKey(@"shell\open\command");
            openKey.SetValue("","\"" + path + "\" \"%1\"");
            openKey.Close();

            key.Close();
            return true;
        }
        Velvet.Info("this is only on windows");
        return false;
    }
}