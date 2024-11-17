using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public class RefreshCommand : Command
{
    public RefreshCommand(IMainFormItem parent): base(parent)
    {
        SetText("&Refresh","Refresh Mod List");
        Shortcut = Keys.Control | Keys.R;
    }
    protected override void OnExecuted(EventArgs e)
    {
        base.OnExecuted(e);
        MainForm.ModList.RefreshModList();
    }
}