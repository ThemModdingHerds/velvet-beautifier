namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Velvet
{
    public const string NAME = "Velvet Beautifier";
    public const string AUTHOR = "N1ghtTheF0x";
    public const string GROUP = "Them's Modding Herds";
    public const string GAMENEWS_MODLIST_FILENAME = "GameNews-ModList.png";
    public const string GAMENEWS_MODLIST_RESOURCE_PATH = $"ThemModdingHerds.VelvetBeautifier.{GAMENEWS_MODLIST_FILENAME}";
    public const string GITHUB_OWNER = "ThemModdingHerds";
    public const string GITHUB_REPO = "velvet-beautifier";
    public const string GITHUB_REPO_CHECKSUMS = "checksums";
    public const string GITHUB_PROJECT_URL = $"https://github.com/{GITHUB_OWNER}";
    public const string GITHUB_REPO_URL = $"{GITHUB_PROJECT_URL}/{GITHUB_REPO}";
    public const string GITHUB_REPO_CHECKSUMS_URL = $"${GITHUB_PROJECT_URL}/${GITHUB_REPO_CHECKSUMS}";
    public const string GITHUB_CHECKSUMS_TFH_FILE_URL = "https://raw.githubusercontent.com/ThemModdingHerds/checksums/refs/heads/main/tfh.json";
    public const string GITHUB_REPO_RELEASES_URL = $"${GITHUB_REPO_URL}/releases";
    public const string GITHUB_REPO_LATEST_RELEASE_URL = $"${GITHUB_REPO_RELEASES_URL}/latest";
    public const string GITHUB_REPO_BUG_REPORT_URL = $"{GITHUB_REPO_URL}/issues/new?assignees=&labels=bug&projects=&template=bug_report.md&title=";
    public const string GITHUB_REPO_FEATURE_REQUEST_URL = $"{GITHUB_REPO_URL}/issues/new?assignees=&labels=enhancement&projects=&template=feature_request.md&title=";
    public static void Info(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(Velvetify(text));
    }
    public static void Error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(Velvetify(text));
    }
    public static void Warn(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Error.WriteLine(Velvetify(text));
    }
    public static void Error(Exception exception) => Error(exception.ToString());
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