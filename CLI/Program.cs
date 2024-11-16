namespace ThemModdingHerds.VelvetBeautifier.CLI;
class Program
{
    private static void Main(string[] argv)
    {
        Application app = new(argv);
        app.CommandLine.Process();
    }
}