using System.IO.Compression;
using System.Runtime.InteropServices;

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
            throw new VelvetException("Utils.CreateTempFile",path + " does not exist");
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
    public static bool ExtractZip(string path,string output)
    {
        try
        {
            ZipFile.ExtractToDirectory(path,output);
        }
        catch(Exception)
        {
            return false;
        }
        return true;
    }
    public static string? ExtractZip(string path)
    {
        string output = CreateTempFolder();
        return ExtractZip(path,output) ? output : null;
    }
}