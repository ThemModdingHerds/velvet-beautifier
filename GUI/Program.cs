using Gtk;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Application.Init();

        var app = new Application("org.GUI.GUI", GLib.ApplicationFlags.None);
        app.Register(GLib.Cancellable.Current);

        var win = new MainWindow();
        app.AddWindow(win);

        win.Show();
        Application.Run();
    }
}
