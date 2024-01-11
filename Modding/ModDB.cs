using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Modding;
public class ModDB
{
    public static readonly string MODS_FOLDER = Path.Combine(Environment.CurrentDirectory,"mods");
    public string Folder {get;}
    private readonly List<Mod> mods = [];
    public List<Mod> Mods {get => mods;}
    public List<Mod> EnabledMods {get => [..mods.Where((mod) => mod.Enabled)];}
    static List<Mod> ReadFolder(string folder)
    {
        List<Mod> mods = [];

        foreach(string modfolder in Directory.GetDirectories(folder))
            if(Mod.IsMod(modfolder))
                mods.Add(new(modfolder));
            else
                Velvet.ConsoleWriteLine(modfolder + " is not a valid mod folder. skipping it");
        return mods;
    }
    public ModDB(string folder)
    {
        Folder = folder;
        Directory.CreateDirectory(folder);
        Refresh();
    }
    public ModDB() : this(MODS_FOLDER)
    {

    }
    public void Refresh()
    {
        mods.Clear();
        mods.AddRange(ReadFolder(Folder));
    }
    public ModInstallResult InstallMod(Mod mod)
    {
        if(mods.Contains(mod)) return ModInstallResult.AlreadyExists;

        IO.CopyFolder(mod.Folder,Path.Combine(Folder,mod.Info.Id));

        return ModInstallResult.Ok;
    }
    public void UninstallMod(Mod mod)
    {
        if(!mods.Contains(mod)) return;

        Directory.Delete(mod.Folder,true);
        mods.Remove(mod);
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
    [Obsolete("use patches")]
    public List<TFHResourceMod> GetTFHResourceMods()
    {
        List<TFHResourceMod> tfhres_mods = [];
        foreach(Mod mod in mods)
            tfhres_mods.AddRange(mod.TFHResourceMods);
        return tfhres_mods;
    }
    [Obsolete("use patches")]
    public List<TFHResourceMod> GetTFHResourceMods(string resource)
    {
        return [..GetTFHResourceMods().Where((mod) => mod.Resource == resource)];
    }
    public void Apply()
    {

    }
}