using System.Runtime.InteropServices;
using System.Text;

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
    // http://www.vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp
    [ComImport()]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IShellLinkW
    {
        //[helpstring("Retrieves the path and filename of
        // a shell link object")]
        void GetPath(
        [Out(), MarshalAs(UnmanagedType.LPWStr)]
            StringBuilder pszFile,
        int cchMaxPath,
        ref _WIN32_FIND_DATAW pfd,
        uint fFlags);

        //[helpstring("Retrieves the list of shell link
        // item identifiers")]
        void GetIDList(out IntPtr ppidl);

        //[helpstring("Sets the list of shell link
        // item identifiers")]
        void SetIDList(IntPtr pidl);

        //[helpstring("Retrieves the shell link
        // description string")]
        void GetDescription(
        [Out(), MarshalAs(UnmanagedType.LPWStr)]
            StringBuilder pszFile,
        int cchMaxName);

        //[helpstring("Sets the shell link description string")]
        void SetDescription(
        [MarshalAs(UnmanagedType.LPWStr)] string pszName);

        //[helpstring("Retrieves the name of the shell link
        // working directory")]
        void GetWorkingDirectory(
        [Out(), MarshalAs(UnmanagedType.LPWStr)]
            StringBuilder pszDir,
        int cchMaxPath);

        //[helpstring("Sets the name of the shell link
        // working directory")]
        void SetWorkingDirectory(
        [MarshalAs(UnmanagedType.LPWStr)] string pszDir);

        //[helpstring("Retrieves the shell link
        // command-line arguments")]
        void GetArguments(
        [Out(), MarshalAs(UnmanagedType.LPWStr)]
            StringBuilder pszArgs,
        int cchMaxPath);

        //[helpstring("Sets the shell link command-line
        // arguments")]
        void SetArguments(
        [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

        //[propget, helpstring("Retrieves or sets the
        // shell link hot key")]
        void GetHotkey(out short pwHotkey);
        //[propput, helpstring("Retrieves or sets the
        // shell link hot key")]
        void SetHotkey(short pwHotkey);

        //[propget, helpstring("Retrieves or sets the shell
        // link show command")]
        void GetShowCmd(out uint piShowCmd);
        //[propput, helpstring("Retrieves or sets the shell 
        // link show command")]
        void SetShowCmd(uint piShowCmd);

        //[helpstring("Retrieves the location (path and index) 
        // of the shell link icon")]
        void GetIconLocation(
        [Out(), MarshalAs(UnmanagedType.LPWStr)] 
            StringBuilder pszIconPath,
        int cchIconPath,
        out int piIcon);

        //[helpstring("Sets the location (path and index) 
        // of the shell link icon")]
        void SetIconLocation(
        [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath,
        int iIcon);

        //[helpstring("Sets the shell link relative path")]
        void SetRelativePath(
        [MarshalAs(UnmanagedType.LPWStr)] 
            string pszPathRel,
        uint dwReserved);

        //[helpstring("Resolves a shell link. The system
        // searches for the shell link object and updates 
        // the shell link path and its list of 
        // identifiers (if necessary)")]
        void Resolve(
        IntPtr hWnd,
        uint fFlags);

        //[helpstring("Sets the shell link path and filename")]
        void SetPath(
        [MarshalAs(UnmanagedType.LPWStr)]
            string pszFile);
    }
    [Guid("00021401-0000-0000-C000-000000000046")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport()]
    private class CShellLink{}
    [StructLayout(LayoutKind.Sequential,
          Pack=4, Size=0, CharSet=CharSet.Unicode)]
    private struct _WIN32_FIND_DATAW
    {
        public uint dwFileAttributes;
        public _FILETIME ftCreationTime;
        public _FILETIME ftLastAccessTime;
        public _FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr , SizeConst = 260)]
        public string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
    }
    [StructLayout(LayoutKind.Sequential,
        Pack=4, Size=0)]
    private struct _FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }
    [ComImport()]
    [Guid("0000010B-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IPersistFile
    {
        // can't get this to go if I extend IPersist, 
        // so put it here:
        [PreserveSig]
        int GetClassID(out Guid pClassID);

        //[helpstring("Checks for changes since
        // last file write")]
        [PreserveSig]
        int IsDirty();

        //[helpstring("Opens the specified file and
        // initializes the object from its contents")]
        void Load(
        [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
        uint dwMode);

        //[helpstring("Saves the object into 
        // the specified file")]
        void Save(
        [MarshalAs(UnmanagedType.LPWStr)] 
        string pszFileName,
        [MarshalAs(UnmanagedType.Bool)] 
        bool fRemember);

        //[helpstring("Notifies the object that save
        // is completed")]
        void SaveCompleted(
        [MarshalAs(UnmanagedType.LPWStr)]
        string pszFileName);

        //[helpstring("Gets the current name of the 
        // file associated with the object")]
        void GetCurFile(
        [MarshalAs(UnmanagedType.LPWStr)]
        out string ppszFileName);
    }
    /// <summary>
    /// Create a shortcut to <c>target</c> with <c>description</c> at <c>filepath</c>
    /// </summary>
    /// <param name="target">A path to redirect</param>
    /// <param name="description">The description of the shortcut</param>
    /// <param name="filepath">A filepath of the shortcut</param>
    public static void CreateShortcut(string target,string description,string filepath)
    {
        if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
        if(!filepath.EndsWith(".lnk"))
            filepath += ".lnk";
        IShellLinkW link = (IShellLinkW)new CShellLink();
        link.SetPath(target);
        link.SetDescription(description);
        IPersistFile file = (IPersistFile)link;
        file.Save(filepath,false);
    }
}