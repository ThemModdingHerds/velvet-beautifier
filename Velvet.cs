namespace ThemModdingHerds.VelvetBeautifier;
public static class Velvet
{
    public static void ShowMessageBox(string text)
    {
        MessageBox.Show(Velvetify(text));
    }
    public static void ShowMessageBox(string text,string caption)
    {
        MessageBox.Show(Velvetify(text),Velvetify(caption));
    }
    public static void ConsoleWriteLine(string text)
    {
        Console.WriteLine(Velvetify(text));
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
public class VelvetException(string message) : Exception(Velvet.Velvetify(message))
{
    
}