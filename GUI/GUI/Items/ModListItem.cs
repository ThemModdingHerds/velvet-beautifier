using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModListItemRow : TableRow
{
    public MainForm MainForm {get;private set;}
    public Mod Mod {get;}
    private ModDB ModDB => MainForm.ModLoaderTool.ModDB;
    private readonly CheckBox enabled;
    private readonly Label version;
    private readonly Label author;
    public ModListItemRow(Mod mod,MainForm mainForm)
    {
        MainForm = mainForm;
        ScaleHeight = false;
        Mod = mod;
        enabled = new()
        {
            Text = Velvet.Velvetify(mod.Info.Name),
            Checked = mod.Enabled,
        };
        enabled.CheckedChanged += OnModEnabledCheckedChanged;
        version = new()
        {
            Text = mod.Info.Version.ToString()
        };
        author = new()
        {
            Text = Velvet.Velvetify(mod.Info.Author)
        };
        Cells.Add(new TableCell(WrapPanel(enabled)));
        Cells.Add(new TableCell(WrapPanel(version)));
        Cells.Add(new TableCell(WrapPanel(author)));
    }
    private Panel WrapPanel(Control control)
    {
        Panel panel = new()
        {
            Content = control,
            ContextMenu = CreateContextMenu(),
            ToolTip = Mod.Info.Description
        };
        return panel;
    }
    private ContextMenu CreateContextMenu()
    {
        ContextMenu menu = new()
        {
            Items = {
                new ButtonMenuItem(OnUninstallButton)
                {
                    Text = Velvet.Velvetify("Uninstall")
                },
                new ButtonMenuItem((object? sender, EventArgs e) => FileSystem.OpenFolder(Mod.Folder))
                {
                    Text = Velvet.Velvetify("Open Folder")
                }
            }
        };
        if(Mod.Info.Url != null)
            menu.Items.Add(new ButtonMenuItem((object? sender, EventArgs e) => Url.OpenUrl(Mod.Info.Url))
            {
                Text = Velvet.Velvetify("Open Link")
            });
        return menu;
    }
    private void OnUninstallButton(object? sender, EventArgs e)
    {
        DialogResult result = VelvetEto.ShowMessageBox("",$"Are you sure you want to uninstall '{Mod.Info.Name}'",MessageBoxButtons.OKCancel,MessageBoxType.Question);
        if(result != DialogResult.Ok) return;
        ModDB.UninstallMod(Mod.Info.Id);
        MainForm.ModList.RefreshModList();
    }
    private void OnModEnabledCheckedChanged(object? sender, EventArgs e)
    {
        Mod.SetEnabled(enabled.Checked ?? false);
    }
}