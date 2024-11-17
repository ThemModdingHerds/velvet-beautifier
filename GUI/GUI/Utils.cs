using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using Eto.Drawing;
using Eto.Forms;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public static class Utils
{
    // ThemModdingHerds.VelvetBeautifier.GUI.*
    public static Icon WindowIcon {get => Icon.FromResource("ThemModdingHerds.VelvetBeautifier.GUI.icon.ico");}
    public static string License {get => GetResourceString("ThemModdingHerds.VelvetBeautifier.GUI.LICENSE");}
    public static Bitmap VelvetImage {get => Bitmap.FromResource("ThemModdingHerds.VelvetBeautifier.GUI.velvet.png");}
    private static Stream GetResourceStream(string resource)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resource) ?? throw new System.Exception($"No Resource '{resource}' found");
    }
    private static string GetResourceString(string resource)
    {
        Stream stream = GetResourceStream(resource);
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes);
        return Encoding.Default.GetString(bytes);
    }
    public static FileFilter GFSFilter {get => new("Reverge Package File",[".gfs"]);}
    public static FileFilter TFHRESFilter {get => new("TFHResouce File",[".tfhres"]);}
}