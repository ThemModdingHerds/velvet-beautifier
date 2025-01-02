using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GFS;
public static class RevergePackageManager
{
    public static bool Tampered {get;private set;} = false;
    public static List<GameFile> Files => Utilities.GameFiles.Read()?.Data01 ?? [];
    public static string GetData01File(Game game,GameFile gamefile) => Path.Combine(game.Data01Folder,gamefile.Name);
    public static void CreateBackup(Game game)
    {
        if(!game.HasData01) return;
        foreach(GameFile gamefile in Files)
        {
            string filepath = GetData01File(game,gamefile);
            bool tampered = BackupManager.MakeBackup(filepath,gamefile);
            if(tampered)
                Tampered = true;
        }
    }
    public static void Revert(Game game)
    {
        if(!game.HasData01) return;
        foreach(GameFile gamefile in Files)
        {
            Velvet.Info($"restoring {gamefile.Name}...");
            string filepath = GetData01File(game,gamefile);
            BackupManager.Revert(filepath);
        }
    }
    public static void Apply(Game game)
    {
        if(!game.HasData01) return;
        Revert(game);
        if(!ModDB.HasRevergePackageMods()) return;
        Velvet.Info("modifying .gfs files...");
        foreach(GameFile gamefile in Files)
        {
            string filepath = GetData01File(game,gamefile);
            RevergePackage target = RevergePackage.Open(filepath);
            RevergePackage modded = new(target.Header,target);
            bool any = false;
            foreach(Mod mod in ModDB.EnabledMods)
            {
                RevergePackage? gfsmod = mod.GetRevergePackage(gamefile.Name);
                if(gfsmod == null) continue;
                modded.AddRange(gfsmod);
                any = true;
            }
            if(!any) continue;
            Velvet.Info($"applying .gfs modifications to {gamefile.Name}");
            if(File.Exists(filepath))
                File.Delete(filepath);
            Writer writer = new(filepath);
            writer.Write(modded);
        }
    }
}