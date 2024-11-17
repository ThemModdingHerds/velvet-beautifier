using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModListItem : ListItem
{
    public Mod Mod {get;}
    public ModListItem(Mod mod)
    {
        Mod = mod;
        Text = mod.Info.Name;
    }
}