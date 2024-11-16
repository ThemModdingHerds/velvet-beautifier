using System.Runtime.InteropServices;
using SharpCompress.Common;
using SharpCompress.Readers;
using ThemModdingHerds.IO.Binary;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class FileSystem
{
    public static string ExecutableExtension {get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : "";}
    public static List<string> GetAllFiles(string folder,string filter = "*.*")
    {
        if(!Directory.Exists(folder))
            return [];
        return [..Directory.EnumerateFiles(folder,filter,SearchOption.AllDirectories)];
    }
    public static void CopyFolder(string source,string dest)
    {
        foreach(string dirpath in Directory.GetDirectories(source,"*",SearchOption.AllDirectories))
            Directory.CreateDirectory(dirpath.Replace(source,dest));
        foreach(string filepath in Directory.GetFiles(source,"*.*",SearchOption.AllDirectories))
            File.Copy(filepath,filepath.Replace(source,dest),true);
    }
    public static string CreateTempFile(string path)
    {
        if(!File.Exists(path))
            throw new FileNotFoundException($"{path} does not exist",path);
        string folder = Directory.CreateTempSubdirectory().FullName;
        string filename = Path.GetFileName(path);
        string tempfile = Path.Combine(folder,filename);
        File.Copy(path,tempfile,true);
        return tempfile;
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
    private static readonly ExtractionOptions extractionOptions = new()
    {
        Overwrite = true,
        ExtractFullPath = true,
        PreserveFileTime = true,
        PreserveAttributes = true
    };
}