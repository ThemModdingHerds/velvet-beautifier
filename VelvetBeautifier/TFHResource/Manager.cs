using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.TFHResource;
public static class TFHResourceManager
{
    public static List<Checksum> Checksums => ChecksumsTFH.FetchSync()?.TFHResources ?? [];
    public static string GetResources(Game game) => Path.Combine(game.Folder,"Scripts","src","Farm","resources");
    public static bool HasResources(Game game) => Directory.Exists(GetResources(game));
    public static string GetResource(Game game,Checksum checksum) => Path.Combine(GetResources(game),checksum.Name);
    public static void CreateBackup(Game game)
    {
        if(!HasResources(game)) return;
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetResource(game,checksum);
            BackupManager.MakeBackup(filepath,checksum);
        }
    }
    public static void Revert(Game game)
    {
        if(!HasResources(game)) return;
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetResource(game,checksum);
            BackupManager.Revert(filepath);
        }
    }
    public static void Apply(Game game)
    {
        if(!HasResources(game)) return;
        Revert(game);
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetResource(game,checksum);
            using Database target = Database.Open(filepath);
            using Database modded = target.Clone();
            int mods = 0;
            foreach(Mod mod in ModDB.EnabledMods)
            {
                using Database? db = mod.GetTFHResource(checksum.Name);
                if(db == null) continue;
                modded.Upsert(db);
                mods++;
            }
            if(mods == 0) return;
            Velvet.Info($"applying {mods} modifications to {checksum.Name}");
            modded.Save(filepath,true);
        }
    }
}