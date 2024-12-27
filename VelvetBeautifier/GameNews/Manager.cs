using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GameNews;
public static class GameNewsManager
{
    public const string FILENAME = "GameNews.ini";
    public const string FOLDERNAME = "news_images";
    public static Checksum? Checksum {get;private set;}
    public static string GetFilePath(Game game) => Path.Combine(game.ScriptsFolder,FILENAME);
    public static string GetNewsImages(Game game) => Path.Combine(game.ScriptsFolder,FOLDERNAME);
    public static List<News> GetGameNews(Game game) => HasFile(game) ? News.ReadGameNews(GetFilePath(game)) : [];
    public static bool HasFile(Game game) => File.Exists(GetFilePath(game));
    public static bool HasNewsImages(Game game) => Directory.Exists(GetNewsImages(game));
    public static async Task Init()
    {
        Checksum = (await ChecksumsTFH.Read())?.GameNews;
    }
    public static void CreateBackup(Game game)
    {
        if(!HasFile(game)) return;
        string filepath = GetFilePath(game);
        BackupManager.MakeBackup(filepath,Checksum);
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
            Velvet.GITHUB_PROJECT_URL
        );
        List<News> gamenews = [];
        if(ModLoaderTool.Outdated)
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
        Velvet.Info($"updating {FILENAME}...");
        News.WriteGameNews(GetFilePath(game),gamenews);
        CopyImages(game);
    }
    public static void CopyImages(Game game)
    {
        if(!HasNewsImages(game)) return;
        string filepath = Path.Combine(GetNewsImages(game),Velvet.GAMENEWS_MODLIST_FILENAME);
        if(File.Exists(filepath))
            File.Delete(filepath);
        using FileStream stream = File.OpenWrite(filepath);
        Dotnet.GetGameNewsModsListResource()
        .CopyTo(stream);
    }
}