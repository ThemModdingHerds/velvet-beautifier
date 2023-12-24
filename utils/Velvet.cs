using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier;
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
    public static void ShowMessageBox(string text,string caption)
    {
        MessageBox.Show(Velvetify(text),Velvetify(caption));
        ConsoleWriteLine(text);
    }
    public static void ConsoleWriteLine(string text)
    {
        Console.WriteLine(Velvetify(text));
        Debug.WriteLineIf(IsDebug,Velvetify(text));
    }
    public static string Velvetify(string input)
    {
        return input
        .Replace("th","z")
        .Replace("w","v")
        .Replace("Th","Z")
        .Replace("W","Z")
        .Replace("TH","Z");
    }
}
public class VelvetException : Exception
{
    public VelvetException(string origin,string message): base(origin + ": " + Velvet.Velvetify(message))
    {
        Velvet.ShowMessageBox(ToString());
    }
}