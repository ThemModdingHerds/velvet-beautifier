using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public class LaunchCommand : Command
{
    public LaunchCommand(IMainFormItem parent): base(parent)
    {
        SetText("&Launch Game","Launch the game with modifications");
        Shortcut = Keys.Control | Keys.L;
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        MainForm.ModLoaderTool.ApplyMods();
        MainForm.ModLoaderTool.Client.Launch();
    }
}