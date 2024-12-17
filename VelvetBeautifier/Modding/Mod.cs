using ThemModdingHerds.GFS;
using ThemModdingHerds.Levels;
using ThemModdingHerds.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;
namespace ThemModdingHerds.VelvetBeautifier.Modding;
public class Mod
{
    public const string MOD_ENABLED_NAME = "enabled";
    public const string MODINFO_NAME = "mod.json";
    public const string LEVELS_NAME = "levels";
    public ModInfo Info {get;}
    public string Folder {get;}
    public bool Enabled {get => File.Exists(Path.Combine(Folder,MOD_ENABLED_NAME));}
    public static bool IsMod(string folder)
    {
        string modinfo_path = Path.Combine(folder,MODINFO_NAME);
        return File.Exists(modinfo_path);
    }
    public static Mod Create(string path,ModInfo info)
    {
        string modinfoPath = Path.Combine(path,MODINFO_NAME);
        Directory.CreateDirectory(path);
        info.Write(modinfoPath);
        return new(path);
    }
    public static Mod Create(ModInfo info) => Create(FileSystem.CreateTempFolder(),info);
    public Mod(string folder)
    {
        Folder = folder;
        string filepath = Path.Combine(folder,MODINFO_NAME);
        if(!IsMod(folder))
            throw new FileNotFoundException($"no mod entry in {folder}",filepath);
        Info = ModInfo.Read(filepath);
    }
    public void Enable()
    {
        if(Enabled) return;
        File.Create(Path.Combine(Folder,MOD_ENABLED_NAME)).Close();
    }
    public void Disable()
    {
        if(!Enabled) return;
        File.Delete(Path.Combine(Folder,MOD_ENABLED_NAME));
    }
    public void Toggle()
    {
        if(Enabled)
        {
            Disable();
            return;
        }
        Enable();
    }
    public void SetEnabled(bool enabled)
    {
        if(enabled)
        {
            Enable();
            return;
        }
        Disable();
    }
    public Database? GetTFHResource(string name)
    {
        string database = Path.Combine(Folder,name);
        if(!File.Exists(database)) return null;
        return Database.Open(database);
    }
    public Dictionary<string,Database> GetTFHResources()
    {
        Dictionary<string,Database> databases = [];
        List<Checksum> gameFiles = ChecksumsTFH.FetchSync()?.TFHResources ?? [];
        foreach(Checksum gameFile in gameFiles)
        {
            Database? db = GetTFHResource(gameFile.Name);
            if(db == null) continue;
            databases.Add(gameFile.Name,db);
        }
        return databases;
    }
    public RevergePackage? GetRevergePackage(string name)
    {
        string gfs = Path.Combine(Folder,name);
        if(!Directory.Exists(gfs)) return null;
        return RevergePackage.Create(gfs);
    }
    public Dictionary<string,RevergePackage> GetRevergePackages()
    {
        Dictionary<string,RevergePackage> packages = [];
        List<Checksum> gameFiles = ChecksumsTFH.FetchSync()?.Data01 ?? [];
        foreach(Checksum gameFile in gameFiles)
        {
            RevergePackage? gfs = GetRevergePackage(gameFile.Name);
            if(gfs == null) continue;
            packages.Add(gameFile.Name,gfs);
        }
        return packages;
    }
    public LevelPack? GetLevelPack()
    {
        string folder = Path.Combine(Folder,LEVELS_NAME);
        if(!Directory.Exists(folder)) return null;
        return LevelPack.Read(folder);
    }
}