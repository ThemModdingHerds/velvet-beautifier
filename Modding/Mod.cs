using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;
using ThemModdingHerds.VelvetBeautifier.Patches;
namespace ThemModdingHerds.VelvetBeautifier.Modding;
public class Mod
{
    public static readonly string MOD_ENABLED_NAME = "enabled";
    public static readonly string MODINFO_NAME = "mod.json";
    public ModInfo Info {get;}
    public string Folder {get;}
    [Obsolete("use patches")]
    public List<TFHResourceMod> TFHResourceMods {get;}
    public List<Patch> Patches {get;}
    public bool Enabled {get => File.Exists(Path.Combine(Folder,MOD_ENABLED_NAME));}
    public static bool IsMod(string folder)
    {
        string modinfo_path = Path.Combine(folder,MODINFO_NAME);
        return File.Exists(modinfo_path);
    }
    public Mod(string folder)
    {
        Folder = folder;
        string filepath = Path.Combine(folder,MODINFO_NAME);
        if(!IsMod(folder))
            throw new VelvetException("new Mod","no mod entry in " + folder);
        Info = ModInfo.Read(filepath);
        Info.Mod = this;
        TFHResourceMods = ReadTFHResourceMod();
        Patches = PatchUtils.ReadPatches(Folder);
    }
    private List<string> GetFolders(List<string> filter)
    {
        List<string> folders = [];
        string[] fullfolders = Directory.GetDirectories(Folder);
        foreach(string fullfolder in fullfolders)
        {
            string name = Path.GetFileName(fullfolder);
            if(!filter.Contains(name))
                continue;
            folders.Add(name);
        }
        return folders;
    }
    [Obsolete("use patches")]
    public List<string> GetTFHResources()
    {
        return GetFolders(GameFiles.TFHResources);
    }
    [Obsolete("use patches")]
    public List<string> GetData01()
    {
        return GetFolders(GameFiles.Data01);
    }
    [Obsolete("use patches")]
    private List<TFHResourceMod> ReadTFHResourceMod()
    {
        List<string> resources = GetTFHResources();
        List<TFHResourceMod> mods = [];
        foreach(string resource in resources)
            mods.Add(TFHResourceModding.Create(this,Path.Combine(Folder,resource)));
        return mods;
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
}