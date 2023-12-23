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
        List<CacheRecord> db_records = db.GetCacheRecords();
        foreach(CacheRecord record in Records)
        {
            bool updated = false;
            foreach(CacheRecord db_record in db_records)
            {
                if(db_record.ShortName == record.ShortName)
                {
                    db.Update(db_record.HiberliteId,record);
                    updated = true;
                }
            }
            if(updated) continue;
            db.Insert(record);
        }
        List<CachedImage> db_images = db.GetCachedImages();
        foreach(CachedImage image in Images)
        {
            bool updated = false;
            foreach(CachedImage db_image in db_images)
            {
                if(db_image.ShortName == image.ShortName)
                {
                    db.Update(db_image.HiberliteId,image);
                    updated = true;
                }
            }
            if(updated) continue;
            db.Insert(image);
        }
        List<CachedTextfile> db_textfiles = db.GetCachedTextfiles();
        foreach(CachedTextfile textfile in TextFiles)
        {
            bool updated = false;
            foreach(CachedTextfile db_textfile in db_textfiles)
            {
                if(db_textfile.ShortName == textfile.ShortName)
                {
                    db.Update(db_textfile.HiberliteId,textfile);
                    updated = true;
                }
            }
            if(updated) continue;
            db.Insert(textfile);
        }
        return db;
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