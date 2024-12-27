using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods for files/folders
/// </summary>
public static class FileSystem
{
    /// <summary>
    /// Safe method of getting all the files in <c>folder</c>
    /// </summary>
    /// <param name="folder">The folder to scan</param>
    /// <param name="filter">A wildcard to get specific files, default is <c>"*.*"</c></param>
    /// <returns>A list of full file paths</returns>
    public static List<string> GetAllFiles(string folder,string filter = "*.*")
    {
        if(!Directory.Exists(folder)) return [];
        return [..Directory.EnumerateFiles(folder,filter,SearchOption.AllDirectories)];
    }
    /// <summary>
    /// Safe method of getting all the subfolders in <c>folder</c>
    /// </summary>
    /// <param name="folder">The folder to scan</param>
    /// <param name="filter">A wildcard to get specific folders, default is <c>"*.*"</c></param>
    /// <returns>A list of full folder paths</returns>
    public static List<string> GetAllSubfolders(string folder,string filter = "*")
    {
        if(!Directory.Exists(folder)) return [];
        return [..Directory.EnumerateDirectories(folder,filter,SearchOption.AllDirectories)];
    }
    public static void CopyFolder(string source,string dest)
    {
        foreach(string dirpath in GetAllSubfolders(source,"*"))
            Directory.CreateDirectory(dirpath.Replace(source,dest));
        foreach(string filepath in GetAllFiles(source,"*.*"))
            File.Copy(filepath,filepath.Replace(source,dest),true);
    }
    public static string CreateTempFile()
    {
        string folder = Directory.CreateTempSubdirectory().FullName;
        string path = Guid.NewGuid().ToString();
        string tempfile = Path.Combine(folder,path);
        File.Create(tempfile).Close();
        return tempfile;
    }
    public static string CreateTempFolder()
    {
        return Directory.CreateTempSubdirectory().FullName;
    }
    public static string SafePath(string path)
    {
        char[] notAllowed = [.. Path.GetInvalidFileNameChars(),.. Path.GetInvalidPathChars()];
        foreach(char no in notAllowed)
            path = path.Replace(new string([no]),"");
        return path;
    }
    public static void OpenFolder(string path)
    {
        if(!Directory.Exists(path)) return;
        Process.Start(new ProcessStartInfo()
        {
            FileName = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar,
            UseShellExecute = true,
            Verb = "open"
        });
    }
}