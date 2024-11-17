using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.Help;
public class AboutCommand : Command
{
    public AboutCommand(IMainFormItem parent): base(parent)
    {
        SetText("&About");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        new About().ShowDialog(MainForm);
    }
}