using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.Help;
public class ResetCommand : Command
{
    public ResetCommand(IMainFormItem parent): base(parent)
    {
        SetText("Reset","Remove all data for a clean slate");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        DialogResult result = VelvetEto.ShowMessageBox("Reset","Are you sure you want to reset",MessageBoxButtons.YesNo);
        if(result == DialogResult.Yes)
        {
            VelvetEto.ShowMessageBox("Restarting",MessageBoxType.Warning);
            ModLoaderTool.Reset();
            MainForm.Close();
        }
    }
}