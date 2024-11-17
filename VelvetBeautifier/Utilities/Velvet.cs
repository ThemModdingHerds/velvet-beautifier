namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Velvet
{
    public const string NAME = "Velvet Beautifier";
    public const string AUTHOR = "N1ghtTheF0x";
    public const string GROUP = "Them's Modding Herds";
    public const string PROJECT_URL = "https://github.com/ThemModdingHerds";
    public const string REPO_URL = "https://github.com/ThemModdingHerds/velvet-beautifier";
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
        .Replace('w','v')
        .Replace("Th","Z")
        .Replace('W','Z')
        .Replace("TH","Z") : input;
    }
}