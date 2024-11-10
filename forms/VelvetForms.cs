using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Forms;
public static class VelvetForms
{
    public static void ShowMessageBox(string text)
    {
        MessageBox.Show(Velvet.Velvetify(text));
        Velvet.Info(text);
    }
    public static void ShowMessageBox(string text,string title)
    {
        MessageBox.Show(Velvet.Velvetify(text),Velvet.Velvetify(title));
        Velvet.Info($"{title}: ${text}");
    }
    public static DialogResult ShowMessageBox(string text,string title,MessageBoxButtons buttons)
    {
        DialogResult result = MessageBox.Show(Velvet.Velvetify(text),Velvet.Velvetify(title),buttons);
        Velvet.Info($"{title}: ${text}");
        return result;
    }
}