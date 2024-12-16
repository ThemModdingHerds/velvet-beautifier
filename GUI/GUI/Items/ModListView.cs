using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ModListView : TableLayout, IMainFormItem
{
    public ModDB ModDB {get => MainForm.ModLoaderTool.ModDB;}
    public MainForm MainForm {get;private set;}
    public ModListView(MainForm parent)
    {
        MainForm = parent;
        AllowDrop = true;
        RefreshModList();
    }
    public void RefreshModList()
    {
        foreach(Mod mod in ModDB.Mods)
            Rows.Add(new ModListItemRow(mod));
    }
    protected override void OnDragDrop(DragEventArgs e)
    {
        if(!e.Data.ContainsUris) return;
        Uri[] uris = e.Data?.Uris ?? [];
        foreach(Uri uri in uris)
        {
            string path = uri.LocalPath;
            Task task = MainForm.ModLoaderTool.InstallMod(path);
            task.Wait();
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