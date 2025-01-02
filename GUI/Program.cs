using Gtk;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ModLoaderTool.Init();
        CommandLine.Process();
        Application.Init();

        var app = new Application(VelvetGtk.APP_ID,GLib.ApplicationFlags.None);
        app.Register(GLib.Cancellable.Current);

        var win = new MainWindow();
        app.AddWindow(win);

        win.Show();
        Application.Run();
    }
}
