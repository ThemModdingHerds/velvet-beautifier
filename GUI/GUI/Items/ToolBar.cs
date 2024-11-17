using ThemModdingHerds.VelvetBeautifier.GUI.Commands;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class ToolBar : Eto.Forms.ToolBar, IMainFormItem
{
    public MainForm MainForm {get => (MainForm)Parent;}
    public ToolBar(MainForm parent)
    {
        Parent = parent;
        Items.AddRange([
            new RefreshCommand(this),
            new ApplyCommand(this),
            new RevertCommand(this),
            new LaunchCommand(this)
        ]);
    }
}