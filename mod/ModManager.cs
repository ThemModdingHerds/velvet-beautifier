using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier;
public static class ModManager
{
    public static Dictionary<string,Mod> Mods {get;set;} = [];
    public static void RefreshMods()
    {
        Mods.Clear();
        if(!Config.Current.ExistsModsFolder())
            Directory.CreateDirectory(Config.Current.ModsFolder);
        string[] folders = Directory.GetDirectories(Config.Current.ModsFolder);
        foreach(string folder in folders)
            try
            {
                Mods.Add(folder,new(folder));
            }
            catch(Exception err)
            {
                Velvet.ConsoleWriteLine(err.ToString());
            }
    }
    public static Mod? FindModByName(string name)
    {
        foreach(Mod mod in Mods.Values)
            if(mod.Info.Name == name)
                return mod;
        return null;
    }
    public static Mod? FindModById(string id)
    {
        foreach(Mod mod in Mods.Values)
            if(mod.Info.Id == id)
                return mod;
        return null;
    }
    public static void RevertMods()
    {
        BackupManager.RevertTFHResources();
        //BackupManager.RevertData01();
        Velvet.ConsoleWriteLine("The mods have been disabled");
    }
}