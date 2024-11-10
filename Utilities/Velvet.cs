namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Velvet
{
    public const string NAME = "Velvet Beautifier";
    public static void Info(string text)
    {
        Console.WriteLine(Velvetify(text));
    }
    public static void Error(string text)
    {
        Console.Error.WriteLine(Velvetify(text));
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
public class VelvetException(string origin,string message) : Exception($"{origin}: {Velvet.Velvetify(message)}")
{

}