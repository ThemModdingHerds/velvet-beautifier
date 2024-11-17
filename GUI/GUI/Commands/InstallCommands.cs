using System;
using System.Threading.Tasks;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public class InstallFileCommand : Command
{
    public InstallFileCommand(IMainFormItem parent): base(parent)
    {
        SetText("From &file","Install mods from files");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        OpenFileDialog dialog = new()
        {
            Title = Velvet.Velvetify("Select files to install"),
            MultiSelect = true
        };
        if(dialog.ShowDialog(MainForm) == DialogResult.Ok)
        {
            int count = 0;
            foreach(string file in dialog.Filenames)
            {
                var task = MainForm.ModLoaderTool.InstallMod(file);
                task.Wait();
                if(task.Result == Modding.ModInstallResult.Ok)
                    count++;
            }
            VelvetEto.ShowMessageBox($"Installed {count} mod/s");
        }
    }
}
public class InstallFolderCommand : Command
{
    public InstallFolderCommand(IMainFormItem parent): base(parent)
    {
        SetText("From &folder","Install mod from folder");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        SelectFolderDialog dialog = new()
        {
            Title = Velvet.Velvetify("Select folder to install"),
        };
        if(dialog.ShowDialog(MainForm) == DialogResult.Ok)
        {
            string file = dialog.Directory;
            var task = MainForm.ModLoaderTool.InstallMod(file);
            task.Wait();
            if(task.Result == Modding.ModInstallResult.Ok)
            VelvetEto.ShowMessageBox($"Installed mods");
        }
    }
}