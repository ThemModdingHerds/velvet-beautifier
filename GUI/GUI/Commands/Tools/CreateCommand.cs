using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.Tools;
public class CreateRevergePackageCommand : Command
{
    public CreateRevergePackageCommand(IMainFormItem parent): base(parent)
    {
        SetText("Create &Reverge Package (.gfs) file","Create a Reverge Package File (.gfs) from a folder");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        SelectFolderDialog input = new()
        {
            Title = Velvet.Velvetify("Select a folder to turn into a Reverge Package File")
        };
        if(input.ShowDialog(MainForm) == DialogResult.Ok)
        {
            SaveFileDialog output = new()
            {
                Title = Velvet.Velvetify("Save Reverge Package File"),
                Filters = {Utils.GFSFilter}
            };
            if(output.ShowDialog(MainForm) == DialogResult.Ok)
            {
                string folder = input.Directory;
                string filepath = output.FileName;
                GFS.Utils.Create(folder,filepath);
            }
        }
    }
}
public class CreateTFHResouceCommand : Command
{
    public CreateTFHResouceCommand(IMainFormItem parent): base(parent)
    {
        SetText("Create &TFHResouce (.tfhres) file","Create a empty TFHResouce File (.tfhres)");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        SaveFileDialog output = new()
        {
            Title = Velvet.Velvetify("Save empty TFHResource File"),
            Filters = {Utils.TFHRESFilter}
        };
        if(output.ShowDialog(MainForm) == DialogResult.Ok)
        {
            string filepath = output.FileName;
            TFHResource.Utils.CreateEmpty(filepath);
        }
    }
}
public class CreateModCommand : Command
{
    public CreateModCommand(IMainFormItem parent): base(parent)
    {
        SetText("Create new &mod","Create a new empty mod");
        Shortcut = Keys.Control | Keys.N;
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        new CreateModForm(MainForm).Show();
    }
}