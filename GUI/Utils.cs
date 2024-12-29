using System.Reflection;
using System.Text;
using Gtk;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public static class Utils
{
    // ThemModdingHerds.VelvetBeautifier.GUI.*
    public static string License => GetResourceString("ThemModdingHerds.VelvetBeautifier.GUI.LICENSE");
    public static Image VelvetImage => Image.LoadFromResource("ThemModdingHerds.VelvetBeautifier.GUI.velvet.png");
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
    public static FileFilter GFSFilter {
        get
        {
            FileFilter filter = new()
            {
                Name = "Reverge Package files"
            };
            filter.AddPattern("*.gfs");
            return filter;
        }
    }
    public static FileFilter TFHRESFilter {
        get
        {
            FileFilter filter = new()
            {
                Name = "TFHResource files"
            };
            filter.AddPattern("*.tfhres");
            return filter;
        }
    }
    public static void JoinThread(ParameterizedThreadStart start)
    {
        Thread thread = new(start);
        thread.Start();
        thread.Join();
    }
    public static void JoinThread(ThreadStart start)
    {
        Thread thread = new(start);
        thread.Start();
        thread.Join();
    }
}