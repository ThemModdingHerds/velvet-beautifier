using System.Reflection;
using System.Text;
using Gtk;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public static class Utils
{
    // ThemModdingHerds.VelvetBeautifier.GUI.*
    public static Gdk.Pixbuf VelvetIcon => Gdk.Pixbuf.LoadFromResource("ThemModdingHerds.VelvetBeautifier.GUI.icon.ico");
    public static string License => GetResourceString("ThemModdingHerds.VelvetBeautifier.GUI.LICENSE");
    public static Gdk.Pixbuf VelvetImage => Gdk.Pixbuf.LoadFromResource("ThemModdingHerds.VelvetBeautifier.GUI.velvet.png");
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
    private static FileFilter CreateFilter(string name,params string[] patterns)
    {
        FileFilter filter = new()
        {
            Name = name
        };
        foreach(string pattern in patterns)
            filter.AddPattern(pattern);
        return filter;
    }
    private static FileFilter CreateMimeTypeFilter(string name,params string[] mimetypes)
    {
        FileFilter filter = new()
        {
            Name = name
        };
        foreach(string mimetype in mimetypes)
            filter.AddMimeType(mimetype);
        return filter;
    }
    public static FileFilter GFSFilter => CreateFilter("Reverge Package files","*.gfs");
    public static FileFilter TFHRESFilter => CreateFilter("TFHResource files","*.tfhres");
    public static FileFilter ZipFilter => CreateMimeTypeFilter("ZIP files","application/zip");
    public static FileFilter RarFilter => CreateMimeTypeFilter("RAR files","application/vnd.rar");
    public static FileFilter SevenZipFilter => CreateMimeTypeFilter("7zip files","application/x-7z-compressed");
    public static FileFilter TarFilter => CreateMimeTypeFilter("Tar file","application/x-tar");
    public static FileFilter GZipFilter => CreateMimeTypeFilter("GZip file","application/gzip");
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