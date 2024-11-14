using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Url
{
    public static void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo(url){UseShellExecute = true});
    }
    public static bool IsUrl(string url)
    {
        return Uri.TryCreate(url,UriKind.RelativeOrAbsolute,out Uri? _);
    }
}