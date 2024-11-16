using System.Diagnostics.CodeAnalysis;

namespace ThemModdingHerds.VelvetBeautifier.GameBanana;
public class Argument(string link,string? itemType,int? id) : IParsable<Argument>
{
    public string Link {get;set;} = link;
    public string? ItemType {get;set;} = itemType;
    public int? Id {get;set;} = id;
    public Argument(string link): this(link,null,null)
    {

    }
    public int GetId()
    {
        if(Id != null) return Id.Value;
        string id_s = Link[(Link.IndexOf("mods/") + 5)..];
        if(id_s.Contains('/'))
            id_s = id_s[id_s.IndexOf('/')..];
        return int.Parse(id_s);
    }
    public static Argument Parse(string s,IFormatProvider? provider)
    {
        // [URL_TO_ARCHIVE]
        // [URL_TO_ARCHIVE],[MOD_TYPE],[MOD_ID]
        if(!s.Contains(',')) return new(s);
        string[] args = s.Split(',');
        if(args.Length != 3) throw new Exception("didn't find 3 elements");
        string url = args[0];
        string itemType = args[1];
        int id = int.Parse(args[2]);
        return new(url,itemType,id);
    }
    public static bool TryParse([NotNullWhen(true)] string? s,IFormatProvider? provider,[MaybeNullWhen(false)] out Argument result)
    {
        result = null;
        try
        {
            result = Parse(s ?? string.Empty,provider);
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
    public static bool TryParse([NotNullWhen(true)] string? s,[MaybeNullWhen(false)] out Argument result) => TryParse(s,null,out result);
}