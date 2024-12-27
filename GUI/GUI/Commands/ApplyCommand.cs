using System;
using System.Threading;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public class ApplyCommand : Command
{
    public ApplyCommand(IMainFormItem parent): base(parent)
    {
        SetText("&Apply","Apply enabled mods");
        Shortcut = Keys.Control | Keys.A;
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        MainForm.Enabled = false;
        ModLoaderTool.ApplyMods();
        MainForm.Enabled = true;
        VelvetEto.ShowMessageBox("Mods applied","enabled mods have been applied, you can now start the game with mods");
    }
}