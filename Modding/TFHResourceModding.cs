using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Modding;
public static class TFHResourceModding
{
    private static string GetSourceFilePath(string folder,string path)
    {
        return path.Replace(folder + "\\","").Replace("\\","/");
    }
    private static string GetDatabasePath(string path)
    {
        return "database:/" + Path.GetFileName(path);
    }
    [Obsolete("use patches")]
    public static TFHResourceMod Create(Mod mod,string path)
    {
        if(!Directory.Exists(path))
            throw new VelvetException("TFHResourceMod.Create",path + " does not exist");
        string resource = Path.GetFileName(path);
        List<string> files = IO.GetAllFiles(path);
        List<CacheRecord> records = [];
        List<CachedImage> images = [];
        List<CachedTextfile> textfiles = [];
        foreach(string file in files)
        {
            if(file.EndsWith(".png"))
                continue; // TODO: images
            else
                textfiles.Add(CreateTextFile(mod,resource,file));
        }
        return new(resource)
        {
            Records = records,
            Images = images,
            TextFiles = textfiles
        };
    }
    public static CachedTextfile CreateTextFile(Mod mod,string resource,string path)
    {
        if(!File.Exists(path))
            throw new VelvetException("TFHResourceUtils.CreateTextFile",path + " does not exist");
        string shortname = GetDatabasePath(path);
        string source_file = GetSourceFilePath(Path.Combine(mod.Folder,resource),path);
        byte[] text_data = File.ReadAllBytes(path);
        return new()
        {
            ShortName = shortname,
            SourceFile = source_file,
            TextData = text_data
        };
    }
    public static CacheRecord CreateRecord(Mod mod,string resource,string path)
    {
        if(!File.Exists(path))
            throw new VelvetException("TFHResourceUtils.CreateRecord",path + " does not exist");
        string shortname = GetDatabasePath(path);
        string source_file = GetSourceFilePath(Path.Combine(mod.Folder,resource),path);
        return new()
        {
            ShortName = shortname,
            SourcePath = source_file
        };
    }
}