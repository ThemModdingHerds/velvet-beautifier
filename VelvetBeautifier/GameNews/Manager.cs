using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GameNews;
public static class GameNewsManager
{
    public const string FILENAME = "GameNews.ini";
    public const string FOLDERNAME = "news_images";
    public static bool Tampering {get;private set;} = false;
    public static GameFile? File => GameFiles.Read()?.GameNews;
    public static bool Verify(Game game) => File?.Verify(GetFilePath(game)) ?? true;
    public static string GetFilePath(Game game) => Path.Combine(game.ScriptsFolder,FILENAME);
    public static string GetNewsImages(Game game) => Path.Combine(game.ScriptsFolder,FOLDERNAME);
    public static List<News> GetGameNews(Game game) => HasFile(game) ? News.ReadGameNews(GetFilePath(game)) : [];
    public static bool HasFile(Game game) => System.IO.File.Exists(GetFilePath(game));
    public static bool HasNewsImages(Game game) => Directory.Exists(GetNewsImages(game));
    public static void CreateBackup(Game game)
    {
        if(!HasFile(game)) return;
        string filepath = GetFilePath(game);
        Tampering = !BackupManager.MakeBackup(filepath,File);
    }
    public static void Revert(Game game)
    {
        if(!HasFile(game)) return;
        Velvet.Info($"restoring {FILENAME}...");
        BackupManager.Revert(GetFilePath(game));
    }
    public static void Apply(Game game)
    {
        if(!HasFile(game)) return;
        Revert(game);
        News modStats = new(
            1,
            Velvet.Velvetify($"{ModDB.EnabledMods.Count} mods loaded"),
            Velvet.GAMENEWS_MODLIST_FILENAME,
            Velvet.Velvetify(Velvet.NAME),
            Velvet.Velvetify("this game has been modified, you may experience unstable/broken sessions"),
            GitHubUtilities.OWNER_URL
        );
        List<News> gamenews = [];
        if(GitHubRelease.Outdated)
        {
            News outdatedNews = new(
                1,
                Velvet.Velvetify("outdated"),
                Velvet.GAMENEWS_MODLIST_FILENAME,
                Velvet.Velvetify(Velvet.NAME),
                Velvet.Velvetify($"you are using an old version of {Velvet.NAME}, update to have better support"),
                GitHubUtilities.LATEST_RELEASE_URL
            );
            gamenews.Add(outdatedNews);
        }
        gamenews.Add(modStats);
        gamenews.AddRange(GetGameNews(game));
        Velvet.Info($"updating {FILENAME}...");
        News.WriteGameNews(GetFilePath(game),gamenews);
        CopyImages(game);
    }
    public static void CopyImages(Game game)
    {
        if(!HasNewsImages(game)) return;
        string filepath = Path.Combine(GetNewsImages(game),Velvet.GAMENEWS_MODLIST_FILENAME);
        if(System.IO.File.Exists(filepath))
            System.IO.File.Delete(filepath);
        using FileStream stream = System.IO.File.OpenWrite(filepath);
        Dotnet.GetGameNewsModsListResource()
        .CopyTo(stream);
    }
}