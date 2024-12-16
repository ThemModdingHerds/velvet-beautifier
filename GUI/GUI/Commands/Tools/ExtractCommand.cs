using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.Tools;
public class ExtractCommand : Command
{
    public ExtractCommand(IMainFormItem parent): base(parent)
    {
        SetText("&Extract","Extract .gfs or .tfhres files");
        Shortcut = Keys.Control | Keys.E;
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        OpenFileDialog input = new()
        {
            Title = Velvet.Velvetify("Select file to extract"),
            MultiSelect = false,
            Filters = {Utils.GFSFilter,Utils.TFHRESFilter}
        };
        if(input.ShowDialog(MainForm) == DialogResult.Ok)
        {
            SelectFolderDialog output = new()
            {
                Title = Velvet.Velvetify("Select the folder for output")
            };
            if(output.ShowDialog(MainForm) == DialogResult.Ok)
            {
                string filepath = input.FileName;
                string folder = output.Directory;
                ModLoaderTool.Extract(filepath,folder);
            }
        }
    }
}