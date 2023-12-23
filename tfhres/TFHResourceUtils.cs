using ThemModdingHerds.Resource;

namespace ThemModdingHerds.VelvetBeautifier;
public static class TFHResourceUtils
{
    private static string GetSourceFilePath(string folder,string path)
    {
        return path.Replace(folder + "\\","").Replace("\\","/");
    }
    private static string GetDatabasePath(string path)
    {
        return "database:/" + Path.GetFileName(path);
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
    public static Database MakeTempCopy(string path)
    {
        if(!File.Exists(path))
            throw new VelvetException("TFHResourceUtils.MakeTempCopy",path + " does not exist");
        byte[] data = File.ReadAllBytes(path);
        DirectoryInfo tempDir = Directory.CreateTempSubdirectory();
        string filepath = Path.Combine(tempDir.FullName,Path.GetFileName(path));
        File.WriteAllBytes(filepath,data);
        return new Database(filepath);
    }
    public static Database MakeTempCopy(Database db)
    {
        return MakeTempCopy(db.Path);
    }
    public static void Replace(string dest,Database db)
    {
        File.Copy(db.Path,dest,true);
    }
    public static void Replace(Database dest,Database db)
    {
        Replace(dest.Path,db);
    }
}