using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.TFHResource;
public static class TFHResourceManager
{
    public static bool Tampering {get;private set;} = false;
    public static List<GameFile> Files => Utilities.GameFiles.Fetch()?.TFHResources ?? [];
    public static string GetResources(Game game) => Path.Combine(game.Folder,"Scripts","src","Farm","resources");
    public static bool HasResources(Game game) => Directory.Exists(GetResources(game));
    public static string GetResource(Game game,GameFile gamefile) => Path.Combine(GetResources(game),gamefile.Name);
    public static void CreateBackup(Game game)
    {
        if(!game.HasResources) return;
        foreach(GameFile gamefile in Files)
        {
            string filepath = GetResource(game,gamefile);
            bool tampered = BackupManager.MakeBackup(filepath,gamefile);
            if(tampered)
                Tampering = true;
        }
    }
    public static void Revert(Game game)
    {
        if(!game.HasResources) return;
        foreach(GameFile gamefile in Files)
        {
            Velvet.Info($"restoring {gamefile.Name}...");
            string filepath = GetResource(game,gamefile);
            BackupManager.Revert(filepath);
        }
    }
    public static void Apply(Game game)
    {
        if(!game.HasResources) return;
        Revert(game);
        if(!ModDB.HasTFHResourceMods()) return;
        Velvet.Info("modifying .tfhres files...");
        foreach(GameFile gamefile in Files)
        {
            string filepath = GetResource(game,gamefile);
            Database target = Database.Open(filepath);
            using Database modded = target.Clone();
            target.Close();
            bool any = false;
            foreach(Mod mod in ModDB.EnabledMods)
            {
                Database? db = mod.GetTFHResource(gamefile.Name);
                if(db == null) continue;
                any = true;
                modded.Upsert(db);
                db.Close();
            }
            if(!any) continue;
            Velvet.Info($"applying .tfhres modifications to {gamefile.Name}");
            modded.Save(filepath,true);
        }
    }
}