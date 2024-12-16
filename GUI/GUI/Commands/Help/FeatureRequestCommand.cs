using System;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands.Help;
public class FeatureRequestCommand : Command
{
    public FeatureRequestCommand(IMainFormItem parent) : base(parent)
    {
        SetText("Request &feature","Suggest an idea for this project");
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        Url.OpenUrl(Velvet.GITHUB_REPO_FEATURE_REQUEST_URL);
    }
}