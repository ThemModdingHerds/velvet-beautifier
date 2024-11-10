using ThemModdingHerds.VelvetBeautifier.Utilities;
using ThemModdingHerds.VelvetBeautifier.Forms;
using FApplication = System.Windows.Forms.Application;
using System.Reflection;

/*[assembly:AssemblyVersion("1.2.0")]
[assembly:AssemblyCompany("N1ghtTheFox")]
#if DEBUG
[assembly:AssemblyConfiguration("debug")]
#else
[assembly:AssemblyConfiguration("release")]
#endif
[assembly:AssemblyDescription("Mod Loader/Tool for Them's Fightin' Herds")]
[assembly:AssemblyTitle("VelvetBeautifier")]*/
namespace ThemModdingHerds.VelvetBeautifier;
class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] argv)
    {
        Velvet.Info("Loading ApplicationConfiguration...");
        ApplicationConfiguration.Initialize();

        Velvet.Info("Starting App...");
        FApplication.Run(new MainForm()
        {
            Argv = [..argv]
        });
    }
}