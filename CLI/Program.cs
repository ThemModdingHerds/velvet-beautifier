using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.CLI;
class Program
{
    private static Application App {get;} = new();
    private static async Task<int> Main(string[] argv)
    {
        if(argv.Length == 1 && GameBanana.Utils.HasArgument(argv[0]))
            await App.InstallGameBananaMod(GameBanana.Utils.ParseArgument(argv[0]));
        App.BackupGameFiles();
        Dictionary<string,string> commandLine = CommandLine.Generate(argv);
        commandLine.TryGetValue("input",out string? input);
        commandLine.TryGetValue("output",out string? output);
        commandLine.TryGetValue("id",out string? id);
        if(commandLine.TryGetValue("install",out string? install))
            await App.InstallMod(install);
        if(commandLine.TryGetValue("remove",out string? remove))
            App.UninstallMod(remove);
        if(commandLine.ContainsKey("register-scheme"))
            Win32.CreateURIScheme();
        if(commandLine.ContainsKey("apply"))
            App.ApplyMods();
        if(commandLine.ContainsKey("revert"))
            App.Revert();
        if(commandLine.ContainsKey("create"))
            App.CreateMod(id);
        if(commandLine.ContainsKey("create-gfs"))
            Application.CreateGFS(input,output);
        if(commandLine.ContainsKey("create-tfhres"))
            Application.CreateTFHRES(output);
        if(commandLine.ContainsKey("extract"))
            Application.Extract(input,output);
        if(commandLine.ContainsKey("enable"))
            App.EnableMod(id);
        if(commandLine.ContainsKey("disable"))
            App.DisableMod(id);
        return 0;
    }
}