using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.CLI;
class Program
{
    private static void Main(string[] argv)
    {
        ModLoaderTool mlt = new(argv);
        Velvet.Info("no parameters specified, check out the guide for usage:");
        Console.WriteLine("https://github.com/ThemModdingHerds/velvet-beautifier/blob/main/CLI.md");
        Environment.Exit(1);
    }
}