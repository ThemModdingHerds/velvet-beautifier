using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Url
{
    public static void OpenUrl(string url)
    {
        if(!IsUrl(url)) return;
        Velvet.Warn($"opening {url} in browser...");
        Process.Start(new ProcessStartInfo(url){UseShellExecute = true});
    }
    public static bool IsUrl(string url)
    {
        return url.StartsWith("http") && Uri.TryCreate(url,UriKind.RelativeOrAbsolute,out Uri? _);
    }
}