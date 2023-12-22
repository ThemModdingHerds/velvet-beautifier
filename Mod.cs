using System.Text.Json;
using ThemModdingHerds.Resource;

namespace ThemModdingHerds.VelvetBeautifier;
public class Mod
{
    public ModInfo Info {get;}
    public string Folder {get;}
    public Mod(string folder)
    {
        Folder = folder;
        string filepath = Path.Combine(folder,"mod.json");
        if(!File.Exists(filepath))
            throw new Exception("no mod entry");
        Info = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText(filepath)) ?? throw new Exception("couldn't read mod entry");
    }
    public List<string> GetFolders(List<string> filter)
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
    public Database Add(Database db,CachedTextfile textfile)
    {

        return db;
    }
}