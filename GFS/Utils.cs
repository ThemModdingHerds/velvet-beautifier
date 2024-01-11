using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;

namespace ThemModdingHerds.VelvetBeautifier.GFS;
public static class Utils
{
    public static void Extract(List<string> gfs_paths,string output)
    {
        foreach(string gfs_path in gfs_paths)
            Extract(gfs_path,output);
    }
    public static void Extract(string gfs_path,string output)
    {
        Reader reader = new(gfs_path);
        GFSFile file = reader.ReadGFSFile();
        foreach(FileEntry entry in file.Entries)
        {
            string fullpath = Path.Combine(output,entry.Path);
            string? dirpath = Path.GetDirectoryName(fullpath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.Create(fullpath);
            if(entry.HasData) 
                File.WriteAllBytes(fullpath,entry.Data);
        }
    }
    public static GFSFile Merge(GFSFile target,params GFSFile[] files)
    {
        foreach(GFSFile file in files)
        foreach(FileEntry entry in file.Entries)
            target.Entries.Add(entry);
        return target;
    }
}