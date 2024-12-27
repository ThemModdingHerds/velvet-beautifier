using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModListView : Panel, IMainFormItem
{
    public MainForm MainForm {get;private set;}
    public ModListView(MainForm parent)
    {
        MainForm = parent;
        AllowDrop = true;
        DragDrop += OnDragDropURI;
    }
    public void RefreshModList()
    {
        TableLayout table = new()
        {
            Rows = {
                new TableRow
                {
                    ScaleHeight = false,
                    Cells = {
                        new TableCell(new Label{BackgroundColor = Colors.LightGrey,Text = Velvet.Velvetify("Name")},true),
                        new TableCell(new Label{BackgroundColor = Colors.LightGrey,Text = Velvet.Velvetify("Version")},true),
                        new TableCell(new Label{BackgroundColor = Colors.LightGrey,Text = Velvet.Velvetify("Author")},true),
                    }
                }
            }
        };
        foreach(Mod mod in ModDB.Mods)
            table.Rows.Add(new ModListItemRow(mod,MainForm));
        table.Rows.Add(null);
        Content = table;
    }
    protected async void OnDragDropURI(object? sender,DragEventArgs e)
    {
        if(!e.Data.ContainsUris) return;
        Uri[] uris = e.Data?.Uris ?? [];
        foreach(Uri uri in uris)
        {
            string path = uri.IsFile ? uri.LocalPath : uri.AbsoluteUri;
            await ModDB.InstallMod(path);
        }
        RefreshModList();
    }
    protected override void OnDragOver(DragEventArgs e)
    {
        if(!e.Data.ContainsUris)
        {
            e.Effects = DragEffects.None;
            return;
        }
        e.Effects = DragEffects.Copy;
    }
}