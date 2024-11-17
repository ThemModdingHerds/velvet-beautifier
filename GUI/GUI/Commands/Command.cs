using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Commands;
public abstract class Command : Eto.Forms.Command
{
    public IMainFormItem MainFormItem {get => (IMainFormItem)Parent;}
    public MainForm MainForm {get => MainFormItem.MainForm;}
    public Command(IMainFormItem parent)
    {
        Parent = parent;
    }
    public void SetText(string text)
    {
        MenuText = Velvet.Velvetify(text);
        ToolBarText = MenuText.Replace("&",string.Empty);
    }
    public void SetText(string text,string tooltip)
    {
        SetText(text);
        ToolTip = Velvet.Velvetify(tooltip);
    }
}