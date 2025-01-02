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
    /// <summary>
    /// Copy the content of <c>soure</c> to <c>dest</c>
    /// </summary>
    /// <param name="source">A folder to copy from</param>
    /// <param name="dest">A folder to copy to</param>
    public static void CopyFolder(string source,string dest)
    {
        // Create folders for the files
        foreach(string dirpath in GetAllSubfolders(source,"*"))
            Directory.CreateDirectory(dirpath.Replace(source,dest));
        // Copy the files
        foreach(string filepath in GetAllFiles(source,"*.*"))
            File.Copy(filepath,filepath.Replace(source,dest),true);
    }
    /// <summary>
    /// Create a empty file at <c>path</c>
    /// </summary>
    /// <param name="filepath">A filepath</param>
    public static void CreateFile(string filepath) => File.Create(filepath).Close();
    /// <summary>
    /// Create a empty file in the temporal folder of the Operating System
    /// </summary>
    /// <returns>A valid filepath in the temporal folder</returns>
    public static string CreateTempFile()
    {
        string folder = CreateTempFolder();
        string filename = Guid.NewGuid().ToString();
        string tempfile = Path.Combine(folder,filename);
        CreateFile(tempfile);
        return tempfile;
    }
    /// <summary>
    /// Create a empty folder in the temporal folder of the Operating System
    /// </summary>
    /// <returns>A valid folderpath in the temporal folder</returns>
    public static string CreateTempFolder() => Directory.CreateTempSubdirectory($"{Velvet.ALTNAME}-").FullName; // very easy as you can see
    /// <summary>
    /// Remove invalid characters from <c>path</c>
    /// </summary>
    /// <param name="path">A path with perhaps invalid characters</param>
    /// <returns>A path without invalid characters</returns>
    public static string SafePath(string path)
    {
        char[] notAllowed = [.. Path.GetInvalidFileNameChars(),.. Path.GetInvalidPathChars()];
        foreach(char no in notAllowed)
            path = path.Replace(new string([no]),"");
        return path;
    }
    /// <summary>
    /// Open up a folder at <c>path</c> in a file explorer
    /// </summary>
    /// <param name="folderpath">A folder path</param>
    /// <returns>true if the folder exists and the folder was opened (?), otherwise false</returns>
    public static bool OpenFolder(string folderpath)
    {
        if(!Directory.Exists(folderpath)) return false;
        return Process.Start(new ProcessStartInfo()
        {
            FileName = Path.EndsInDirectorySeparator(folderpath) ? folderpath : folderpath + Path.DirectorySeparatorChar,
            UseShellExecute = true,
            Verb = "open"
        }) != null;
    }
}