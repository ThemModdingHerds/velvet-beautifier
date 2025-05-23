namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Various methods used everywhere
/// </summary>
public static class Velvet
{
    /// <summary>
    /// The name of the software
    /// </summary>
    public const string NAME = "Velvet Beautifier";
    /// <summary>
    /// The short description of the software
    /// </summary>
    public const string DESCRIPTION = "Mod Loader/Tool for Them's Fightin' Herds";
    /// <summary>
    /// The alternative name of the software
    /// </summary>
    public const string ALTNAME = "velvetbeautifier";
    /// <summary>
    /// The URI scheme of Velvet Beautifier
    /// </summary>
    public const string URI_SCHEME = ALTNAME;
    /// <summary>
    /// The author who created this software
    /// </summary>
    public const string AUTHOR = "N1ghtTheF0x";
    /// <summary>
    /// The group/organization which this tool is a part of
    /// </summary>
    public const string GROUP = "Them's Modding Herds";
    /// <summary>
    /// The file path of the Mod List Game News image used in-game
    /// </summary>
    public const string GAMENEWS_MODLIST_FILENAME = "GameNews-ModList.png";
    /// <summary>
    /// the path to the resource of the Mod List Game News image
    /// </summary>
    public const string GAMENEWS_MODLIST_RESOURCE_PATH = $"ThemModdingHerds.VelvetBeautifier.{GAMENEWS_MODLIST_FILENAME}";
    public static void Info(string text)
    {
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(Velvetify(text));
        Console.ForegroundColor = color;
    }
    /// <summary>
    /// Print a error message to the <c>Console</c>
    /// </summary>
    /// <param name="text">The text to print</param>
    public static void Error(string text)
    {
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(Velvetify(text));
        Console.ForegroundColor = color;
    }
    /// <summary>
    /// Print a warning message to the <c>Console</c>
    /// </summary>
    /// <param name="text">The text to print</param>
    public static void Warn(string text)
    {
        ConsoleColor color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Error.WriteLine(Velvetify(text));
        Console.ForegroundColor = color;
    }
    /// <summary>
    /// Print a exception to the <c>Console</c>
    /// </summary>
    /// <param name="exception">The exception to print</param>
    public static void Error(Exception exception) => Error(exception.ToString());
    /// <summary>
    /// Formats any string value to Velvet's accent
    /// </summary>
    /// <param name="text">The string to format</param>
    /// <param name="force">Force the string to format (because it only happens on April 1st), default is <c>false</c></param>
    /// <returns></returns>
    public static string Velvetify(string text,bool force = false)
    {
        return force || IsAprilFools ? text
        .Replace("th","z")
        .Replace('w','v')
        .Replace("Th","Z")
        .Replace('W','Z')
        .Replace("TH","Z") : text;
    }
    /// <summary>
    /// Check if today is April 1st (April fools)
    /// </summary>
    public static bool IsAprilFools {get => DateTime.Now.Month == 4 && DateTime.Now.Day == 1;}
    /// <summary>
    /// Gets the Application Data Folder (Windows = AppData/local, Linux = .local/share)
    /// </summary>
    public static string AppDataFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),ALTNAME);
}