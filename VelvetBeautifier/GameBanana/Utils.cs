using System.Text.RegularExpressions;
namespace ThemModdingHerds.VelvetBeautifier.GameBanana;
public static partial class Utils
{
    public const string GAMEBANANA_API_URL = "https://api.gamebanana.com";
    public const string GAMEBANANA_UAPI_URL = "https://gamebanana.com/apiv11";
    public const int GAMEBANANA_GAME_ID = 16837;
    public static bool ValidMMDLUrl(string url) => GameBananaMMDLURL().IsMatch(url);
    public static bool ValidModsUrl(string url) => GameBananaModsURL().IsMatch(url);
    public static int GetModId(string url)
    {
        if(!ValidMMDLUrl(url) && !url.Contains("/mods/")) return -1;
        try
        {
            string id_s = url[(url.IndexOf("mods/") + 5)..];
            if(id_s.Contains('/'))
                id_s = id_s[id_s.IndexOf('/')..];
            return int.Parse(id_s);
        }
        catch{}
        return -1;
    }
    public static string CreateCoreItemDataRequestUrl(string itemtype,int id,List<string> fields)
    {
        string field = string.Join(',',fields);
        return $"{GAMEBANANA_API_URL}/Core/Item/Data?itemtype={itemtype}&itemid={id}&fields={field}&format=json&return_keys=true";
    }
    public static string CreateSearchRequestUrl(int page, int perPage = 15)
    {
        return $"{GAMEBANANA_UAPI_URL}/Mod/Index?_nPerpage={perPage}&_aFilters[Generic_Game]={GAMEBANANA_GAME_ID}&_nPage={page}";
    }

    [GeneratedRegex(@"^(https?:\/\/)?(www\.)?gamebanana\.com\/mmdl\/\d+$")]
    private static partial Regex GameBananaMMDLURL();
    [GeneratedRegex(@"^(https?:\/\/)?(www\.)?gamebanana\.com\/mods\/\d+$")]
    private static partial Regex GameBananaModsURL();
}