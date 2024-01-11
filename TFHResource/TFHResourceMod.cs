using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.TFHResource;
[Obsolete("use patches")]
public class TFHResourceMod(string resource)
{
    public string Resource {get;} = resource;
    [Obsolete("use patches")]
    public List<CacheRecord> Records {get; set;} = [];
    [Obsolete("use patches")]
    public List<CachedImage> Images {get; set;} = [];
    [Obsolete("use patches")]
    public List<CachedTextfile> TextFiles {get; set;} = [];
    [Obsolete("use patches")]
    public List<InkBytecode> InkBytecodes {get; set;} = [];
    [Obsolete("use patches")]
    public List<JotBytecode> JotBytecodes {get; set;} = [];
    [Obsolete("use patches")]
    public TFHResourceFile Insert(TFHResourceFile db)
    {
        List<CacheRecord> db_records = db.GetEntries<CacheRecord>();
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
        List<CachedImage> db_images = db.GetEntries<CachedImage>();
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
        List<CachedTextfile> db_textfiles = db.GetEntries<CachedTextfile>();
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
        List<InkBytecode> db_inkbytecodes = db.GetEntries<InkBytecode>();
        foreach(InkBytecode inkBytecode in InkBytecodes)
        {
            bool updated = false;
            foreach(InkBytecode db_inkbytecode in db_inkbytecodes)
            {
                if(db_inkbytecode.ShortName == inkBytecode.ShortName)
                {
                    db.Update(db_inkbytecode.HiberliteId,inkBytecode);
                    updated = true;
                }
            }
            if(updated) continue;
            db.Insert(inkBytecode);
        }
        List<JotBytecode> db_jotbytecodes = db.GetEntries<JotBytecode>();
        foreach(JotBytecode jotBytecode in JotBytecodes)
        {
            bool updated = false;
            foreach(JotBytecode db_jotbytecode in db_jotbytecodes)
            {
                if(db_jotbytecode.ShortName == jotBytecode.ShortName)
                {
                    db.Update(db_jotbytecode.HiberliteId,jotBytecode);
                    updated = true;
                }
            }
            if(updated) continue;
            db.Insert(jotBytecode);
        }
        return db;
    }
}