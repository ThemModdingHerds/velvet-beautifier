using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Modding;
public class ModDB
{
    public string Folder {get;}
    public List<Mod> Mods {get => ReadFolder();}
    public List<Mod> EnabledMods {get => [..Mods.Where((mod) => mod.Enabled)];}
    public List<Mod> DisabledMods {get => [..Mods.Where((mod) => !mod.Enabled)];}
    private List<Mod> ReadFolder()
    {
        List<Mod> mods = [];

        foreach(string modfolder in Directory.GetDirectories(Folder))
        {
            if(Mod.IsMod(modfolder))
            {
                mods.Add(new(modfolder));
                continue;
            }
        }
        return mods;
    }
    public ModDB(string folder)
    {
        Folder = folder;
        Directory.CreateDirectory(folder);
    }
    public ModInstallResult InstallMod(Mod mod)
    {
        ModInstallResult result = ModInstallResult.Ok;
        if(Mods.Contains(mod))
        {
            UninstallMod(mod);
            result = ModInstallResult.AlreadyExists;
        }
        string path = Path.Combine(Folder,mod.Info.Id);
        Directory.CreateDirectory(path);
        FileSystem.CopyFolder(mod.Folder,path);
        return result;
    }
    public ModInstallResult InstallMod(string? folder)
    {
        if(folder == null) return ModInstallResult.Invalid;
        if(Mod.IsMod(folder))
            return InstallMod(new Mod(folder));
        return ModInstallResult.Invalid;
    }
    public bool UninstallMod(string id)
    {
        Mod? mod = FindModById(id);
        if(mod == null) return false;
        UninstallMod(mod);
        return true;
    }
    public void UninstallMod(Mod mod)
    {
        if(!Mods.Contains(mod)) return;

        Directory.Delete(mod.Folder,true);
    }
    public Mod? FindModByName(string name)
    {
        foreach(Mod mod in Mods)
            if(mod.Info.Name == name)
                return mod;
        return null;
    }
    public Mod? FindModById(string id)
    {
        foreach(Mod mod in Mods)
            if(mod.Info.Id == id)
                return mod;
        return null;
    }
    public void Clear()
    {
        if(Directory.Exists(Folder))
            Directory.Delete(Folder,true);
    }
}