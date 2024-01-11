using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.TFHResource;
public static class TFHResourceUtils
{
    public static void Extract(string db_path,string output)
    {
        static string fix_buckgit_path(string path) => path.Replace("c:\\buckgit\\","");
        static string fix_database_path(string path) => path.Replace("database:/","");
        static string find_path(CachedImage image, List<CacheRecord> records)
        {
            foreach (CacheRecord record in records)
                if (image.ShortName == record.ShortName)
                    return fix_buckgit_path(record.SourcePath);
            return fix_database_path(image.ShortName);
        }
        TFHResourceFile db = new(db_path);
        List<CacheRecord> records = db.GetEntries<CacheRecord>();
        List<CachedImage> images = db.GetEntries<CachedImage>();
        List<CachedTextfile> textfiles = db.GetEntries<CachedTextfile>();
        List<InkBytecode> inkBytecodes = db.GetEntries<InkBytecode>();
        List<JotBytecode> jotBytecodes = db.GetEntries<JotBytecode>();
        List<LocalizedText> localizedTexts = db.GetEntries<LocalizedText>();
        foreach(CachedImage image in images)
        {
            string filepath = Path.Combine(output,find_path(image,records));
            if(image.IsCompressed == 1) filepath += ".compressed";
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.WriteAllBytes(filepath,image.ImageData);
        }
        foreach(CachedTextfile textfile in textfiles)
        {
            string filepath = Path.Combine(output,fix_buckgit_path(textfile.SourceFile));
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.WriteAllBytes(filepath,textfile.TextData);
        }
        foreach(InkBytecode bytecode in inkBytecodes)
        {
            string filepath = Path.Combine(output,fix_buckgit_path(bytecode.SourceFile));
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.WriteAllText(filepath,bytecode.Bytecode);
        }
        foreach(JotBytecode bytecode in jotBytecodes)
        {
            string filepath = Path.Combine(output,"jot",IO.SafePath(bytecode.ShortName) + ".xml");
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.WriteAllText(filepath,bytecode.Bytecode);
        }
        foreach(LocalizedText text in localizedTexts)
        {
            string filepath = Path.Combine(output,"lang",text.LanguageCode,fix_database_path(text.StoryFileDatabaseName));
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.AppendAllText(filepath,text.Tag + " = " + text.Text+"\n");
        }
    }
}