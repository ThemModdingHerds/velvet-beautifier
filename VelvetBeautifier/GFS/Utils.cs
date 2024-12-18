using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GFS;
public static class Utils
{
    public static void Extract(IEnumerable<string> gfs_paths,string output)
    {
        foreach(string gfs_path in gfs_paths)
            Extract(gfs_path,output);
    }
    public static void Extract(string gfs_path,string output)
    {
        Velvet.Info($"extracting .gfs at {gfs_path} to {output}...");
        Reader reader = new(gfs_path);
        RevergePackage file = reader.ReadRevergePackage();
        foreach(KeyValuePair<string,RevergePackageEntry> pair in file)
        {
            RevergePackageEntry entry = pair.Value;
            string fullpath = Path.Combine(output,entry.Path);
            string? dirpath = Path.GetDirectoryName(fullpath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            FileStream stream = File.Create(fullpath);
            if(entry.Data.Length > 0)
                stream.Write(entry.Data);
            stream.Close();
        }
    }
    public static string Extract(string gfs_path)
    {
        string temp = FileSystem.CreateTempFolder();
        Extract(gfs_path,temp);
        return temp;
    }
    public static bool Create(string? input,string? output)
    {
        if(input == null || output == null) return false;
        Velvet.Info($"creating a .gfs from {input} to {output}...");
        try
        {
            string inputPath = Path.Combine(Environment.CurrentDirectory,input);
            string outputPath = Path.Combine(Environment.CurrentDirectory,output.EndsWith(".gfs") ? output : $"{output}.gfs");
            RevergePackage gfs = RevergePackage.Create(inputPath);
            using Writer writer = new(outputPath);
            writer.Write(gfs);
            return true;
        }
        catch(Exception exception)
        {
            Velvet.Error(exception);
            return false;
        }
    }
}