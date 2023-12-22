using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier;
public static class ModManager
{
    public static Dictionary<string,Mod> Mods {get;set;} = [];
    public static void RefreshMods()
    {
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
}