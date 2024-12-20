using System.Text.RegularExpressions;
using ThemModdingHerds.VelvetBeautifier.Utilities;
namespace ThemModdingHerds.VelvetBeautifier.GameBanana;
public static partial class Utils
{
    public static bool ValidUrl(string url)
    {
        return GameBananaURL().IsMatch(url);
        //return url.StartsWith("http://gamebanana.com") ||
        //        url.StartsWith("https://gamebanana.com") ||
        //        url.StartsWith("http://www.gamebanana.com") ||
        //        url.StartsWith("https://www.gamebanana.com");
    }
    public static int GetModId(string url)
    {
        if(!ValidUrl(url) && !url.Contains("/mods/")) return -1;
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

    [GeneratedRegex(@"^(https?:\/\/)?(www\.)?gamebanana\.com.*$")]
    private static partial Regex GameBananaURL();
}