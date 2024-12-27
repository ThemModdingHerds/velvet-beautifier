using System.Runtime.InteropServices;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods for the Win32 API (will not work on other operating systems)
/// </summary>
public static class Win32
{
    /// <summary>
    /// Register the URI scheme in the Windows Registry to support 1-Click GameBanana Installing
    /// </summary>
    /// <returns>true if registration was successful, false not</returns>
    public static bool CreateURIScheme()
    {
        // Only windows
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Velvet.Warn("not supported on this platform");
            return false;
        }
        // we need the executable
        string? path = Dotnet.ExecutablePath;
        if(path == null) return false;
        // this is the path where URI schemes are registered in windows
        string regpath = $"SOFTWARE\\CLASSES\\{Velvet.URI_SCHEME}";
        using Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regpath,true) ?? Microsoft.Win32.Registry.CurrentUser.CreateSubKey(regpath);
        // set name of URI scheme
        key.SetValue("",$"URL:{Velvet.NAME}");
        key.SetValue("URL Protocol","");

        // set the icon of the URI scheme (this uses the executable icon)
        using Microsoft.Win32.RegistryKey iconKey = key.OpenSubKey("DefaultIcon",true) ?? key.CreateSubKey("DefaultIcon");
        iconKey.SetValue("",$"\"{path}\",1");

        // set the logic of the URI scheme (when someone click on the link)
        using Microsoft.Win32.RegistryKey openKey = key.OpenSubKey("shell\\open\\command",true) ?? key.CreateSubKey("shell\\open\\command");
        openKey.SetValue("",$"\"{path}\" \"%1\"");

        Velvet.Info("created URI scheme for this executable");
        return true;
    }
    /// <summary>
    /// Delete the registered URI scheme if it exists
    /// </summary>
    /// <returns>true if it was deleted, false not</returns>
    public static bool DeleteURIScheme()
    {
        // only on windows
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Velvet.Warn("not supported on this platform");
            return false;
        }
        string regpath = $"SOFTWARE\\CLASSES\\{Velvet.URI_SCHEME}";
        // check if key exists
        bool exists = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regpath) != null;
        if(!exists) return false;
        try
        {
            // try to delete it
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(regpath,true);
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}