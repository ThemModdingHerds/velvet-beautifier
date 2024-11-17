using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.Help;
public class ReportIssueCommand : Command
{
    public ReportIssueCommand(IMainFormItem parent): base(parent)
    {
        SetText("Report &issue","Report an issue to GitHub");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        Url.OpenUrl("https://github.com/ThemModdingHerds/velvet-beautifier/issues/new/choose");
    }
}