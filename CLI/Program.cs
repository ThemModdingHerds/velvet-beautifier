using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
class Program
{
    private static Application App {get => Application.Instance;}
    static int Main(string[] argv)
    {
        CommandLine commandLine = new(argv);
        if(!App.Client.Valid() || !App.Server.Valid())
        {
            Velvet.ConsoleWriteLine("The game or server are not configured in the 'config.json' file. Use 'VelvetBeautifier.GUI.exe to create one'");
            return 1;
        }
        return 0;
    }
}