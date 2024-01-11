using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Velvet
{
    private static bool IsDebug {get;} = false;
    static Velvet()
    {
#if DEBUG
        IsDebug = true;
#endif
    }
    public static void ShowMessageBox(string text)
    {
        MessageBox.Show(Velvetify(text));
        ConsoleWriteLine(text);
    }
    public static void ShowMessageBox(string text,string title)
    {
        MessageBox.Show(Velvetify(text),Velvetify(title));
        ConsoleWriteLine(title + ": " + text);
    }
    public static DialogResult ShowMessageBox(string text,string title,MessageBoxButtons buttons)
    {
        DialogResult result = MessageBox.Show(Velvetify(text),Velvetify(title),buttons);
        ConsoleWriteLine(title + ": " + text);
        return result;
    }
    public static void ConsoleWriteLine(string text)
    {
        Console.WriteLine(Velvetify(text));
        Debug.WriteLineIf(IsDebug,Velvetify(text));
    }
    public static string Velvetify(string input)
    {
        return Special.IsAprilFools ? input
        .Replace("th","z")
        .Replace("w","v")
        .Replace("Th","Z")
        .Replace("W","Z")
        .Replace("TH","Z") : input;
    }
}
public class VelvetException : Exception
{
    public VelvetException(string origin,string message): base(origin + ": " + Velvet.Velvetify(message))
    {
        Velvet.ShowMessageBox(ToString());
    }
}