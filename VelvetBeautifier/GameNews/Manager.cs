using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GameNews;
public static class GameNewsManager
{
    public const string FILENAME = "GameNews.ini";
    public const string FOLDERNAME = "news_images";
    public static Checksum? Checksum => ChecksumsTFH.FetchSync()?.GameNews;
    public static string GetFilePath(Game game) => Path.Combine(game.GetScripts(),FILENAME);
    public static string GetNewsImages(Game game) => Path.Combine(game.GetScripts(),FOLDERNAME);
    public static List<News> GetGameNews(Game game) => HasFile(game) ? News.ReadGameNews(GetFilePath(game)) : [];
    public static bool HasFile(Game game) => File.Exists(GetFilePath(game));
    public static void CreateBackup(Game game)
    {
        if(!HasFile(game)) return;
        BackupManager.MakeBackup(GetFilePath(game));
    }
    public static void Revert(Game game)
    {
        if(!HasFile(game)) return;
        BackupManager.Revert(GetFilePath(game));
    }
    public static void Apply(Game game)
    {
        Revert(game);
        News modStats = new(
            1,
            Velvet.Velvetify($"{ModDB.EnabledMods.Count} mods loaded"),
            Velvet.GAMENEWS_MODLIST_FILENAME,
            Velvet.Velvetify(Velvet.NAME),
            Velvet.Velvetify("this game has been modified, you may experience unstable/broken sessions"),
            Velvet.GITHUB_PROJECT_URL
        );
        List<News> gamenews = [];
        if(ModLoaderTool.IsOutdated())
        {
            News outdatedNews = new(
                1,
                Velvet.Velvetify("outdated"),
                Velvet.GAMENEWS_MODLIST_FILENAME,
                Velvet.Velvetify(Velvet.NAME),
                Velvet.Velvetify($"you are using an old version of {Velvet.NAME}, update to have better support"),
                Velvet.GITHUB_REPO_LATEST_RELEASE_URL
            );
            gamenews.Add(outdatedNews);
        }
        gamenews.Add(modStats);
        gamenews.AddRange(GetGameNews(game));
        News.WriteGameNews(GetFilePath(game),gamenews);
        CopyImages(game);
    }
    public static void CopyImages(Game game)
    {
        string filepath = Path.Combine(GetNewsImages(game),Velvet.GAMENEWS_MODLIST_FILENAME);
        if(File.Exists(filepath))
            File.Delete(filepath);
        using FileStream stream = File.OpenWrite(filepath);
        Dotnet.GetGameNewsModsListResource()
        .CopyTo(stream);
    }
}