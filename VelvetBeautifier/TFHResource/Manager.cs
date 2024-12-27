using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.TFHResource;
public static class TFHResourceManager
{
    public static bool Tampering {get;private set;} = false;
    public static List<Checksum> Checksums {get;private set;} = [];
    public static string GetResources(Game game) => Path.Combine(game.Folder,"Scripts","src","Farm","resources");
    public static bool HasResources(Game game) => Directory.Exists(GetResources(game));
    public static string GetResource(Game game,Checksum checksum) => Path.Combine(GetResources(game),checksum.Name);
    public static async Task Init()
    {
        Checksums = (await ChecksumsTFH.Fetch())?.TFHResources ?? [];
    }
    public static void CreateBackup(Game game)
    {
        if(!game.HasResources) return;
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetResource(game,checksum);
            bool tampered = BackupManager.MakeBackup(filepath,checksum);
            if(!Tampering)
                Tampering = tampered;
        }
    }
    public static void Revert(Game game)
    {
        if(!game.HasResources) return;
        foreach(Checksum checksum in Checksums)
        {
            Velvet.Info($"restoring {checksum.Name}...");
            string filepath = GetResource(game,checksum);
            BackupManager.Revert(filepath);
        }
    }
    public static void Apply(Game game)
    {
        if(!game.HasResources) return;
        Revert(game);
        if(!ModDB.HasTFHResourceMods()) return;
        Velvet.Info("modifying .tfhres files...");
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetResource(game,checksum);
            Database target = Database.Open(filepath);
            using Database modded = target.Clone();
            target.Close();
            bool any = false;
            foreach(Mod mod in ModDB.EnabledMods)
            {
                Database? db = mod.GetTFHResource(checksum.Name);
                if(db == null) continue;
                any = true;
                modded.Upsert(db);
                db.Close();
            }
            if(!any) continue;
            Velvet.Info($"applying .tfhres modifications to {checksum.Name}");
            modded.Save(filepath,true);
        }
    }
}