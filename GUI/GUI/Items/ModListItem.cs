using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModListItem : Panel
{
    public Mod Mod {get;}
    private readonly CheckBox enabled = new();
    private readonly Label version = new();
    private readonly Label author = new();
    public ModListItem(Mod mod)
    {
        Mod = mod;
        ToolTip = mod.Info.Description;
        enabled.Text = mod.Info.Name;
        enabled.CheckedChanged += OnModEnabledCheckedChanged;
        version.Text = mod.Info.Version.ToString();
        author.Text = mod.Info.Author;
        Content = new TableLayout
        {
            Rows = {
                new TableRow
                {
                    ScaleHeight = true,
                    Cells = {
                        new TableCell(enabled,true),
                        new TableCell(version,true),
                        new TableCell(author,true)
                    }
                }
            }
        };
    }
    private void OnModEnabledCheckedChanged(object? sender, EventArgs e)
    {
        Mod.SetEnabled(enabled.Checked ?? false);
    }
}