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
            string regpath = $"SOFTWARE\\CLASSES\\{Scheme}";
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regpath,true) ?? Microsoft.Win32.Registry.CurrentUser.CreateSubKey(regpath);
            key.SetValue("",$"URL:{Velvet.NAME}");
            key.SetValue("URL Protocol","");

            Microsoft.Win32.RegistryKey iconKey = key.OpenSubKey("DefaultIcon",true) ?? key.CreateSubKey("DefaultIcon");
            iconKey.SetValue("",$"\"{path}\",1");
            iconKey.Close();

            Microsoft.Win32.RegistryKey openKey = key.OpenSubKey("shell\\open\\command",true) ?? key.CreateSubKey("shell\\open\\command");
            openKey.SetValue("",$"\"{path}\" \"%1\"");
            openKey.Close();

            key.Close();
            Velvet.Info("created URI scheme for this executable");
            return true;
        }
        Velvet.Warn("not supported on this platform");
        return false;
    }
    public static bool DeleteURIScheme()
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string regpath = $"SOFTWARE\\CLASSES\\{Scheme}";
            bool exists = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regpath) != null;
            if(exists)
            {
                try
                {
                    Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(regpath,true);
                }
                catch(Exception)
                {
                    return false;
                }
                return true;
            }
        }
        Velvet.Warn("not supported on this platform");
        return false;
    }
}