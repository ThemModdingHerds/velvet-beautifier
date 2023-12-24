namespace ThemModdingHerds.VelvetBeautifier;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] argv)
    {
        Utils.AttachConsole(-1);
        
        Velvet.ConsoleWriteLine("Loading ApplicationConfiguration...");
        ApplicationConfiguration.Initialize();
        CommandLineResult cmdResult = HandleCommandLine(argv);

        Velvet.ConsoleWriteLine("Starting App...");
        Application.Run(new MainForm());
    }
    
    static CommandLineResult HandleCommandLine(string[] argv)
    {
        foreach(string arg in argv)
        {
            if(arg.StartsWith(Utils.Scheme))
            {
                GameBanana.HandleCommandLine(arg[(Utils.Scheme.Length + 1)..]);
                return CommandLineResult.GameBanana;
            }
        }
        return CommandLineResult.None;
    }
}