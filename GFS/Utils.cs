using System.Text;
using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;

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
}