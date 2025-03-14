using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.Levels;
using ThemModdingHerds.VelvetBeautifier.GFS;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Levels;
public static class LevelManager
{
    public const string FILENAME = "levels.gfs";
    public static string GetFilePath(Game game) => Path.Combine(game.Data01Folder,FILENAME);
    public const string FOLDERNAME = "levels";
    public static string Folder => Path.Combine(Velvet.AppDataFolder,FOLDERNAME);
    public static void Create()
    {
        if(!BackupManager.ExistsBackup(FILENAME)) return;
        if(Directory.Exists(Folder))
            Directory.Delete(Folder,true);
        Directory.CreateDirectory(Folder);
        string filepath = BackupManager.GetBackupPath(FILENAME);
        try
        {
            string gfs = GFS.Utils.Extract(filepath);
            LevelPack pack = LevelPack.Read(Path.Combine(gfs,"temp","levels"));
            pack.Save(Folder);
        }
        catch(Exception exception)
        {
            Velvet.Error(exception);
        }
    }
    public static void Apply(Game game)
    {
        if(!BackupManager.ExistsBackup(FILENAME)) return;
        if(!ModDB.HasLevelPackMods()) return;
        Velvet.Info("create level pack from game files...");
        Create();
        LevelPack pack = LevelPack.Read(Folder);
        string temp = FileSystem.CreateTempFolder();
        string tempLevels = Path.Combine(temp,"temp","levels");
        Directory.CreateDirectory(tempLevels);
        foreach(Mod mod in ModDB.EnabledMods)
        {
            LevelPack? modpack = mod.GetLevelPack();
            if(modpack == null) continue;
            pack.Add(modpack);
        }
        pack.Save(tempLevels);
        Velvet.Info($"applying levels modifications with {pack.Levels.Count} levels and {pack.Worlds.Entries.Count} entries to {FILENAME}");
        string gfs = GetFilePath(game);
        RevergePackage levels = RevergePackage.Open(gfs);
        RevergePackage newLevels = RevergePackage.Create(temp);
        RevergePackage modded = RevergePackage.Merge(levels.Header,levels,newLevels);
        if(File.Exists(gfs))
            File.Delete(gfs);
        using Writer writer = new(gfs);
        writer.Write(modded);
        if(Directory.Exists(temp))
            Directory.Delete(temp,true);
    }
}