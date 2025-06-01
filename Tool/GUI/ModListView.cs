using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModListView : ListView
{
    public class Item(Mod mod)
    {
        public Mod Mod => mod;
        public override string ToString()
        {
            return Mod.Info.Name;
        }
    }
    public ModListView()
    {
        Refresh();
    }
    public void Refresh() => SetSource(ModDB.Mods.Select(mod => new Item(mod)).ToList());
}