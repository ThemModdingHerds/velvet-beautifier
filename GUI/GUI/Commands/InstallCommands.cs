using System;
using System.Linq;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Modding;
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
                ModInstallResult result = ModDB.InstallMod(file);
                if(result == ModInstallResult.Ok)
                    count++;
            }
            VelvetEto.ShowMessageBox($"Installed {count}/{dialog.Filenames.Count()} mods");
            MainForm.ModList.RefreshModList();
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
            ModInstallResult result = ModDB.InstallMod(file);
            if(result == ModInstallResult.Ok)
                VelvetEto.ShowMessageBox($"Installed mods");
            MainForm.ModList.RefreshModList();
        }
    }
}
public class InstallTextCommand : Command
{
    public InstallTextCommand(IMainFormItem parent): base(parent)
    {
        SetText("From &text","Install mod from a Text (URL, GameBanana ID)");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        new InstallTextModForm(MainForm).Show();
    }
}