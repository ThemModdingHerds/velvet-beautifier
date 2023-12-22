using ThemModdingHerds.Resource;

namespace ThemModdingHerds.VelvetBeautifier;
public class TFHResourceMod
{
    public List<CacheRecord> Records {get;} = [];
    public List<CachedImage> Images {get;} = [];
    public List<CachedTextfile> TextFiles {get;} = [];
    public Database Insert(Database db)
    {
        return db.Insert(Records)
        .Insert(Images)
        .Insert(TextFiles);
    }
    public static void Insert(string path,TFHResourceMod mod)
    {
        Database db = new(path);
        mod.Insert(db);
    }
}