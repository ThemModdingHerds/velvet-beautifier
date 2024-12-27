using System;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public class OpenURLCommand : Command
{
    public string URL {get;}
    public OpenURLCommand(IMainFormItem parent,string url,string text,string tooltip): base(parent)
    {
        URL = url;
        SetText(text,tooltip);
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        Url.OpenUrl(URL);
    }
}