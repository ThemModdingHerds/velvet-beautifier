using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class CommandLine
{
    public static string[] Args => Environment.GetCommandLineArgs();
    public static Dictionary<string,string?> Arguments {get;} = Create(Args);
    public const string ARGUMENT_PREFIX = "--";
    public static IReadOnlyList<ICommandArgumentHandler> Handlers {get;} = [
        new RegisterSchemeHandler(),
        new InstallModHandler(),
        new RemoveModHandler(),
        new ApplyModsHandler(),
        new RevertHandler(),
        new CreateModHandler(),
        new CreateRevergePackageHandler(),
        new CreateTFHResourceHandler(),
        new ExtractHandler(),
        new EnableModHandler(),
        new DisableModHandler(),
        new ListModsHandler(),
        new ListModsHandler(),
        new ResetHandler()
    ];
    public static void Process()
    {
        if(Args.Length == 1)
        {
            if(Args[0] != Dotnet.ExecutableDllPath && Uri.TryCreate(Args[0],UriKind.Absolute,out Uri? uri))
            {
                string content = uri.AbsolutePath;
                ModInstallResult result = ModInstallResult.Invalid;
                if(Url.IsUrl(content) || File.Exists(content) || Directory.Exists(content))
                {
                    result = ModDB.InstallMod(content);
                }
                else if(GameBanana.Argument.TryParse(content,out GameBanana.Argument? argument))
                {
                    result = ModDB.InstallMod(argument.Link);
                }
                Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
            }
        }
        bool handled = false;
        foreach(ICommandArgumentHandler handler in Handlers)
        {
            if(handled) break;
            if(Arguments.TryGetValue(handler.Name,out string? value))
            {
                try
                {
                    handler.OnExecute(value);
                    handled = true;
                }
                catch(Exception exception)
                {
                    Velvet.Error(exception);
                }
            }
        }
        if(handled) return;
        Environment.Exit(1);
    }
    private static bool IsArg(string arg)
    {
        return arg.StartsWith(ARGUMENT_PREFIX);
    }
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
public interface ICommandArgumentHandler
{
    public string Name {get;}
    public void OnExecute(string? value);
}