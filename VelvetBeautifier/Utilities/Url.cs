using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Various methods for URL related stuff
/// </summary>
public static class Url
{
    /// <summary>
    /// Open <c>url</c> in the default Browser
    /// </summary>
    /// <param name="url">The URL to open</param>
    public static void OpenUrl(string url)
    {
        // first check if this is a url at all
        if(!IsUrl(url)) return;
        // warn the user in the console
        Velvet.Warn($"opening {url} in browser...");
        // this does not start a process
        Process.Start(new ProcessStartInfo(url){UseShellExecute = true});
    }
    /// <summary>
    /// Check if <c>url</c> is a URL (it must start with <c>http</c>)
    /// </summary>
    /// <param name="url">The string to verify</param>
    /// <returns></returns>
    public static bool IsUrl(string? url) => url != null && url.StartsWith("http") && Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri? _);
}