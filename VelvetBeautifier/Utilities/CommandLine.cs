using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains methods and fields for the command line
/// </summary>
public static class CommandLine
{
    /// <summary>
    /// The command line arguments of the current process, this is literally just <c>Environment.GetCommandLineArgs</c> which means it has the following structure:
    /// <c>[executablePath,...args]</c>
    /// </summary>
    public static string[] Args => Environment.GetCommandLineArgs();
    /// <summary>
    /// The path of the executable
    /// </summary>
    public static string Executable => Args[0];
    /// <summary>
    /// The arguments passed to the command line
    /// </summary>
    public static string[] Argv => Args[1..];
    /// <summary>
    /// The parsed arguments with their value if they have one
    /// </summary>
    public static Dictionary<string,string?> Arguments {get;private set;} = [];
    /// <summary>
    /// The prefix of the parsed arguments
    /// </summary>
    public const string ARGUMENT_PREFIX = "--";
    /// <summary>
    /// This is a collection of commands for handling specfic command line arguments
    /// </summary>
    private static List<ICommandArgumentHandler> handlers = [
        new RegisterSchemeHandler(), // register URI scheme
        new InstallModHandler(), // installing a mod
        new RemoveModHandler(), // removing a mod
        new UpdateModHandler(), // updating a mod
        new ApplyModsHandler(), // applying the mods to the game
        new RevertHandler(), // revert the game back to its orignal state
        new CreateModHandler(), // create a mod
        new CreateRevergePackageHandler(), // create a Reverge Package file (.gfs)
        new CreateTFHResourceHandler(), // create a TFHResource file (.tfhres)
        new ExtractHandler(), // extracting files (Reverge Package, TFHResource)
        new EnableModHandler(), // enable a mod
        new DisableModHandler(), // disable a mod
        new ListModsHandler(), // list the mods in the output
        new ResetHandler(), // "uninstall"
        new SetConfigHandler() // set values in the configuration file
    ];
    /// <summary>
    /// Add a custom command to the pool
    /// </summary>
    /// <param name="handler">A custom handler</param>
    public static void AddHandler(params ICommandArgumentHandler[] handler) => handlers.AddRange(handler);
    /// <summary>
    /// Process the command line arguments, may exit the current process
    /// </summary>
    public static void Process()
    {
        string[] args = Argv;
#if DEBUG
        // make changes to 'args' here
#endif
        Arguments = Create(args);
        if (args.Length == 1)
        {
            // this is for URI scheme
            if (args[0] != Dotnet.ExecutableDllPath && Uri.TryCreate(args[0], UriKind.Absolute, out Uri? uri))
            {
                string content = uri.AbsolutePath;
                ModInstallResult result = ModInstallResult.Invalid;
                // basic URI scheme
                if (Url.IsUrl(content) || File.Exists(content) || Directory.Exists(content))
                {
                    result = ModDB.InstallMod(content);
                }
                // GameBanana 1-Click installer™
                else if (GameBanana.Argument.TryParse(content, out GameBanana.Argument? argument))
                {
                    result = ModDB.InstallMod(argument.DownloadLink);
                }
                Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
            }
        }
        bool handled = false;
        // go through each command
        foreach (ICommandArgumentHandler handler in handlers)
        {
            if (handled) break;
            if (Arguments.TryGetValue(handler.Name, out string? value))
            {
                try
                {
                    int result = handler.OnExecute(value);
                    if (result != 0)
                        Environment.Exit(result);
                    handled = true;
                }
                catch (Exception exception)
                {
                    Velvet.Error(exception);
                }
            }
        }
        // if any of the arguments was handled, exit the current process
        if (handled)
            Environment.Exit(0);
    }
    /// <summary>
    /// Check if <c>arg</c> is an argument (arguments are string with -- prefixed)
    /// </summary>
    /// <param name="arg">The argument to check</param>
    /// <returns></returns>
    private static bool IsArg(string arg)
    {
        return arg.StartsWith(ARGUMENT_PREFIX);
    }
    /// <summary>
    /// Parse <c>argv</c> to a dictionary with optional value
    /// </summary>
    /// <param name="argv">A array of arguments (from Main)</param>
    /// <returns>The parsed arguments</returns>
    private static Dictionary<string,string?> Create(string[] argv)
    {
        Dictionary<string,string?> dict = [];
        for(long i = 0;i < argv.LongLength;i++)
        {
            string arg = argv[i];
            if(IsArg(arg))
            {
                string key = arg[2..];
                string? value = null;
                long next = i + 1;
                if(next < argv.LongLength && !IsArg(argv[next]))
                {
                    value = argv[next];
                    i = next;
                }
                if(dict.ContainsKey(key))
                {
                    dict[key] = value;
                    continue;
                }
                dict.Add(key,value);
            }
        }
        return dict;
    }
}
/// <summary>
/// A command argument handler is a interface for handling a specific command
/// </summary>
public interface ICommandArgumentHandler
{
    /// <summary>
    /// The name of the command
    /// </summary>
    public string Name {get;}
    /// <summary>
    /// This method gets triggered if <c>Name</c> was found in the arguments
    /// </summary>
    /// <param name="value">The value of <c>Name</c> if it has any</param>
    /// <returns>The exit code, if this is not zero it will exit immediately</returns>
    public int OnExecute(string? value);
}