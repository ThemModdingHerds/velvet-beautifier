using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.CLI;
class Program
{
    private static void Main(string[] argv)
    {
        ModLoaderTool app = new(argv);
        SetupResult setup = app.Setup();
        if(setup != SetupResult.Success && setup != SetupResult.NotRequired)
        {
            Environment.Exit(1);
            return;
        }
        app.CommandLine.Process();
        
        Velvet.Info("no parameters specified, check out the guide for usage:");
        Console.WriteLine("https://github.com/ThemModdingHerds/velvet-beautifier/blob/main/CLI.md");
        Environment.Exit(1);
    }
}