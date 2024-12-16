using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class CommandLine(ModLoaderTool mlt,string[] argv)
{
    public string[] Argv {get;} = argv;
    public Dictionary<string,string?> Arguments {get;} = Create(argv);
    public const string ARGUMENT_PREFIX = "--";
    public ModLoaderTool ModLoaderTool {get;} = mlt;
    public IReadOnlyList<ICommandArgumentHandler> Handlers {get;} = [
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
    public void Process()
    {
        if(Argv.Length == 1)
        {
            if(Argv[0] != Dotnet.ExecutableDllPath && Uri.TryCreate(Argv[0],UriKind.Absolute,out Uri? uri))
            {
                string content = uri.AbsolutePath;
                ModInstallResult result = ModInstallResult.Invalid;
                if(Url.IsUrl(content) || File.Exists(content) || Directory.Exists(content))
                {
                    var task = ModLoaderTool.InstallMod(content);
                    task.Wait();
                    result = task.Result;
                }
                else if(GameBanana.Argument.TryParse(content,out GameBanana.Argument? argument))
                {
                    var task = ModLoaderTool.InstallMod(argument.Link);
                    task.Wait();
                    result = task.Result;
                }
                Velvet.Info("you can close this window now...");
                Console.ReadLine();
                Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
            }
        }
        foreach(ICommandArgumentHandler handler in Handlers)
            if(Arguments.TryGetValue(handler.Name,out string? value))
                handler.OnExecute(ModLoaderTool,value);
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
    public void OnExecute(ModLoaderTool application,string? value);
}