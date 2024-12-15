namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Velvet
{
    public const string NAME = "Velvet Beautifier";
    public const string AUTHOR = "N1ghtTheF0x";
    public const string GROUP = "Them's Modding Herds";
    public const string GITHUB_OWNER = "ThemModdingHerds";
    public const string GITHUB_REPO = "velvet-beautifier";
    public const string GITHUB_PROJECT_URL = $"https://github.com/{GITHUB_OWNER}";
    public const string GITHUB_REPO_URL = $"https://github.com/{GITHUB_OWNER}/{GITHUB_REPO}";
    public const string GITHUB_REPO_RELEASES_URL = $"${GITHUB_REPO_URL}/releases";
    public const string GITHUB_REPO_LATEST_RELEASE_URL = $"${GITHUB_REPO_RELEASES_URL}/latest";
    public const string GAMENEWS_MODLIST_FILENAME = "GameNews-ModList.png";
    public const string GAMENEWS_MODLIST_RESOURCE_PATH = $"ThemModdingHerds.VelvetBeautifier.{GAMENEWS_MODLIST_FILENAME}";
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