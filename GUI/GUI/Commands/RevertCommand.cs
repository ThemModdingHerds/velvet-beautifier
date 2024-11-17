using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public class RevertCommand : Command
{
    public RevertCommand(IMainFormItem parent): base(parent)
    {
        SetText("&Revert","Remove all modifications from the game");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        MainForm.ModLoaderTool.Revert();
    }
}