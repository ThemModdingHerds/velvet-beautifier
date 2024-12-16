using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModListItemRow : TableRow
{
    public Mod Mod {get;}
    private readonly CheckBox enabled = new();
    private readonly Label version = new();
    private readonly Label author = new();
    public ModListItemRow(Mod mod)
    {
        Mod = mod;
        enabled.Text = mod.Info.Name;
        enabled.CheckedChanged += OnModEnabledCheckedChanged;
        version.Text = mod.Info.Version.ToString();
        author.Text = mod.Info.Author;
        ScaleHeight = true;
        Cells.Add(new TableCell(enabled,true));
        Cells.Add(new TableCell(version,true));
        Cells.Add(new TableCell(author,true));
    }
    private void OnModEnabledCheckedChanged(object? sender, EventArgs e)
    {
        Mod.SetEnabled(enabled.Checked ?? false);
    }
}