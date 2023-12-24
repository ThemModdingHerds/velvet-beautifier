using System.Text.Json;
using ThemModdingHerds.Resource;

namespace ThemModdingHerds.VelvetBeautifier;
public class Mod
{
    public ModInfo Info {get;}
    public string Folder {get;}
    public List<TFHResourceMod> TFHResourceMods {get;}
    public Mod(string folder)
    {
        Folder = folder;
        string filepath = Path.Combine(folder,"mod.json");
        if(!File.Exists(filepath))
            throw new VelvetException("new Mod","no mod entry in " + folder);
        Info = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText(filepath)) ?? throw new VelvetException("new Mod","couldn't read mod entry in " + folder);
        TFHResourceMods = ReadTFHResourceMod();
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
    public List<string> GetTFHResources()
    {
        return GetFolders(Modable.TFHResources);
    }
    public List<string> GetData01()
    {
        return GetFolders(Modable.Data01);
    }
    private List<TFHResourceMod> ReadTFHResourceMod()
    {
        List<string> resources = GetTFHResources();
        List<TFHResourceMod> mods = [];
        foreach(string resource in resources)
            mods.Add(TFHResourceMod.Create(this,Path.Combine(Folder,resource)));
        return mods;
    }
}