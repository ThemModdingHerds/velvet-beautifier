using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public static class VelvetEto
{
    public static void ShowMessageBox(string title,MessageBoxType type = MessageBoxType.Information)
    {
        MessageBox.Show(Velvet.Velvetify(title),type);
    }
    public static void ShowMessageBox(string title,string content,MessageBoxType type = MessageBoxType.Information)
    {
        MessageBox.Show(Velvet.Velvetify(content),Velvet.Velvetify(title),type);
    }
    public static DialogResult ShowMessageBox(string title,MessageBoxButtons buttons,MessageBoxType type = MessageBoxType.Information)
    {
        return MessageBox.Show(Velvet.Velvetify(title),buttons,type);
    }
    public static DialogResult ShowMessageBox(string title,string content,MessageBoxButtons buttons,MessageBoxType type = MessageBoxType.Information)
    {
        return MessageBox.Show(Velvet.Velvetify(content),Velvet.Velvetify(title),buttons,type);
    }
}