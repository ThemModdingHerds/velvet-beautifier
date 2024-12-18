using ThemModdingHerds.TFHResource;
using ThemModdingHerds.TFHResource.Data;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.TFHResource;
public static class Utils
{
    public static void Extract(string db_path,string output)
    {
        Velvet.Info($"extraing .tfhres at {db_path} to {output}...");
        static string fix_buckgit_path(string path) => path.Replace("c:\\buckgit\\","");
        static string fix_database_path(string path) => path.Replace("database:/","");
        static string find_path(CachedImage image, List<CacheRecord> records)
        {
            foreach (CacheRecord record in records)
                if (image.Shortname == record.Shortname)
                    return fix_buckgit_path(record.SourcePath);
            return fix_database_path(image.Shortname);
        }
        Database db = Database.Open(db_path);
        List<CacheRecord> records = db.ReadCacheRecord();
        List<CachedImage> images = db.ReadCachedImage();
        List<CachedTextfile> textfiles = db.ReadCachedTextfile();
        List<InkBytecode> inkBytecodes = db.ReadInkBytecode();
        List<JotBytecode> jotBytecodes = db.ReadJotBytecode();
        List<LocalizedText> localizedTexts = db.ReadLocalizedText();
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
            string filepath = Path.Combine(output,"jot",FileSystem.SafePath(bytecode.Shortname) + ".xml");
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.WriteAllText(filepath,bytecode.Bytecode);
        }
        foreach(LocalizedText text in localizedTexts)
        {
            string filepath = Path.Combine(output,"lang",text.Langcode,fix_database_path(text.StoryfileDbname));
            string? dirpath = Path.GetDirectoryName(filepath);
            if(dirpath == null) continue;
            Directory.CreateDirectory(dirpath);
            File.AppendAllText(filepath,text.Tag + " = " + text.Text+"\n");
        }
        db.Close();
    }
    public static bool CreateEmpty(string? output)
    {
        if(output == null) return false;
        try
        {
            string outputPath = Path.Combine(Environment.CurrentDirectory,output.EndsWith(".tfhres") ? output : $"{output}.tfhres");
            using Database database = new();
            database.Save(outputPath);
            Velvet.Info($"created empty .tfhres at {outputPath}");
            return true;
        }
        catch(Exception exception)
        {
            Velvet.Error(exception);
            return false;
        }
    }
}