using ThemModdingHerds.Resource;

namespace ThemModdingHerds.VelvetBeautifier;
public class TFHResourceMod(string resource)
{
    public string Resource {get;} = resource;
    public List<CacheRecord> Records {get;private set;} = [];
    public List<CachedImage> Images {get;private set;} = [];
    public List<CachedTextfile> TextFiles {get;private set;} = [];
    public Database Insert(Database db)
    {
        return db.Insert(Records)
        .Insert(Images)
        .Insert(TextFiles);
    }
    public static TFHResourceMod Create(Mod mod,string path)
    {
        if(!Directory.Exists(path))
            throw new VelvetException("TFHResourceMod.Create",path + " does not exist");
        string resource = Path.GetFileName(path);
        List<string> files = Utils.GetAllFiles(path);
        List<CacheRecord> records = [];
        List<CachedImage> images = [];
        List<CachedTextfile> textfiles = [];
        foreach(string file in files)
        {
            if(file.EndsWith(".png"))
                continue; // TODO: images
            else
                textfiles.Add(TFHResourceUtils.CreateTextFile(mod,resource,file));
        }
        return new(resource)
        {
            Records = records,
            Images = images,
            TextFiles = textfiles
        };
    }
    public static void ModIt(string path,List<TFHResourceMod> mods)
    {
        string backup = BackupManager.MakeBackup(path);
        // 1. Copy backup to temp file
        string temp = Utils.CreateTempFile(backup);
        // 2. add items to temp file
        Database db = new(temp);
        foreach(TFHResourceMod mod in mods)
            mod.Insert(db);
        // 3. replace game file with temp file
        File.Copy(temp,path,true);
    }
}