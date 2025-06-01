using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI;

public class ModListView : ListView
{
    public class Item : CheckBox
    {
        private readonly Mod _mod;
        public Item(Mod mod): base(mod.Info.ToString(),mod.Enabled)
        {
            _mod = mod;
            Toggled += (_) => mod.SetEnabled(Checked);
        }
        public override string ToString()
        {
            return $"{_mod.Info}";
        }
    }
    public ModListView()
    {
        Refresh();
    }
    public void Refresh() => SetSource(ModDB.Mods.Select(mod => new Item(mod)).ToList());
}