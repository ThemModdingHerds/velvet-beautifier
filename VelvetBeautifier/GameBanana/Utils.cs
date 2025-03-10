using System.Text.RegularExpressions;
namespace ThemModdingHerds.VelvetBeautifier.GameBanana;
public static partial class Utils
{
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
        return $"https://api.gamebanana.com/Core/Item/Data?itemtype={itemtype}&itemid={id}&fields={field}&format=json&return_keys=true";
    }

    [GeneratedRegex(@"^(https?:\/\/)?(www\.)?gamebanana\.com\/mmdl\/\d+$")]
    private static partial Regex GameBananaMMDLURL();
    [GeneratedRegex(@"^(https?:\/\/)?(www\.)?gamebanana\.com\/mods\/\d+$")]
    private static partial Regex GameBananaModsURL();
}