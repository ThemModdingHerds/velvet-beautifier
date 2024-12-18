using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GFS;
public static class RevergePackageManager
{
    public const string FOLDERNAME = "data01";
    public static List<Checksum> Checksums => ChecksumsTFH.FetchSync()?.Data01 ?? [];
    public static string GetData01(Game game) => Path.Combine(game.Folder,FOLDERNAME);
    public static string GetData01File(Game game,Checksum checksum) => Path.Combine(GetData01(game),checksum.Name);
    public static bool HasData01(Game game) => Directory.Exists(GetData01(game));
    public static void CreateBackup(Game game)
    {
        if(!HasData01(game)) return;
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetData01File(game,checksum);
            BackupManager.MakeBackup(filepath,checksum);
        }
    }
    public static void Revert(Game game)
    {
        if(!HasData01(game)) return;
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetData01File(game,checksum);
            BackupManager.Revert(filepath);
        }
    }
    public static void Apply(Game game)
    {
        if(!HasData01(game)) return;
        Revert(game);
        foreach(Checksum checksum in Checksums)
        {
            string filepath = GetData01File(game,checksum);
            RevergePackage target = RevergePackage.Open(filepath);
            int mods = 0;
            RevergePackage modded = new(target.Header,target);
            foreach(Mod mod in ModDB.EnabledMods)
            {
                RevergePackage? gfsmod = mod.GetRevergePackage(checksum.Name);
                if(gfsmod == null) continue;
                modded.AddRange(gfsmod);
                mods++;
            }
            if(mods == 0) continue;
            Velvet.Info($"applying {mods} modifications to {checksum.Name}");
            if(File.Exists(filepath))
                File.Delete(filepath);
            Writer writer = new(filepath);
            writer.Write(modded);
        }
    }
}