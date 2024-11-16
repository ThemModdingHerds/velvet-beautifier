using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Modding;
public class ModDB
{
    public string Folder {get;}
    private readonly List<Mod> mods = [];
    public List<Mod> Mods {get => mods;}
    public List<Mod> EnabledMods {get => [..mods.Where((mod) => mod.Enabled)];}
    public List<Mod> DisabledMods {get => [..mods.Where((mod) => !mod.Enabled)];}
    static List<Mod> ReadFolder(string folder)
    {
        List<Mod> mods = [];

        foreach(string modfolder in Directory.GetDirectories(folder))
        {
            if(Mod.IsMod(modfolder))
            {
                mods.Add(new(modfolder));
                continue;
            }
            Velvet.Info($"{modfolder} is not a valid mod folder. skipping it");
        }
        return mods;
    }
    public ModDB(string folder)
    {
        Folder = folder;
        Directory.CreateDirectory(folder);
        Refresh();
    }
    public void Refresh()
    {
        mods.Clear();
        mods.AddRange(ReadFolder(Folder));
    }
    public ModInstallResult InstallMod(Mod mod)
    {
        if(mods.Contains(mod)) return ModInstallResult.AlreadyExists;
        string path = Path.Combine(Folder,mod.Info.Id);
        Directory.CreateDirectory(path);
        FileSystem.CopyFolder(mod.Folder,path);

        return ModInstallResult.Ok;
    }
    public ModInstallResult InstallMod(string? folder)
    {
        if(folder == null || !Mod.IsMod(folder)) return ModInstallResult.Invalid;
        return InstallMod(new Mod(folder));
    }
    public bool UninstallMod(string id)
    {
        Refresh();
        Mod? mod = FindModById(id);
        if(mod == null) return false;
        UninstallMod(mod);
        return true;
    }
    public void UninstallMod(Mod mod)
    {
        if(!mods.Contains(mod)) return;

        Directory.Delete(mod.Folder,true);
    }
    public Mod? FindModByName(string name)
    {
        foreach(Mod mod in mods)
            if(mod.Info.Name == name)
                return mod;
        return null;
    }
    public Mod? FindModById(string id)
    {
        foreach(Mod mod in mods)
            if(mod.Info.Id == id)
                return mod;
        return null;
    }
    public void Clear()
    {
        if(Directory.Exists(Folder))
            Directory.Delete(Folder);
    }
}