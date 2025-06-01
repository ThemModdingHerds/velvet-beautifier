namespace ThemModdingHerds.VelvetBeautifier.GitHub;
public class GitHubUtilities
{
    public const string OWNER = "ThemModdingHerds";
    public const string REPO = "velvet-beautifier";
    public const string CHECKSUM_REPO = "checksums";
    public const string OWNER_URL = $"https://github.com/{OWNER}";
    public const string REPO_URL = $"{OWNER_URL}/{REPO}";
    public const string RELEASES_URL = $"{REPO_URL}/releases";
    public const string LATEST_RELEASE_URL = $"{RELEASES_URL}/latest";
    public const string BUG_REPORT_URL = $"{REPO_URL}/issues/new?assignees=&labels=bug&projects=&template=bug_report.md&title=";
    public const string FEATURE_REQUEST_URL = $"{REPO_URL}/issues/new?assignees=&labels=enhancement&projects=&template=feature_request.md&title=";
    public const string GUI_GUIDE_URL = $"{REPO_URL}/blob/main/GUI.md";
}