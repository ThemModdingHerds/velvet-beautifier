using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.File;
public class ExitCommand : Command
{
    public ExitCommand(IMainFormItem parent): base(parent)
    {
        SetText("E&xit");
        Shortcut = Keys.Alt | Keys.F4;
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        MainForm.Close();
    }
}