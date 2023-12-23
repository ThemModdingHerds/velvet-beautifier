namespace ThemModdingHerds.VelvetBeautifier;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
#if DEBUG
        Utils.AllocConsole();
        Utils.AttachConsole(-1);
#endif
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        Velvet.ConsoleWriteLine("Loading ApplicationConfiguration...");
        ApplicationConfiguration.Initialize();
        Velvet.ConsoleWriteLine("Starting App...");
        Application.Run(new MainForm());
    }    
}