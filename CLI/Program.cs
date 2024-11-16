using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.CLI;
class Program
{
    private static Application App {get;} = new();
    private static async Task<int> Main(string[] argv)
    {
        if(argv.Length == 1 && GameBanana.Utils.HasArgument(argv[0]))
        {
            await App.InstallGameBananaMod(GameBanana.Utils.ParseArgument(argv[0]));
            return 0;
        }

        SetupResult setupResult = App.Setup();
        if(setupResult == SetupResult.BackupFail || setupResult == SetupResult.OldConfig) return 1;

        Dictionary<string,string> commandLine = CommandLine.Generate(argv);
        commandLine.TryGetValue("input",out string? input);
        commandLine.TryGetValue("output",out string? output);
        commandLine.TryGetValue("id",out string? id);
        bool forced = commandLine.ContainsKey("force");

        if(commandLine.TryGetValue("install",out string? install))
        {
            ModInstallResult result = await App.InstallMod(install);
            return result == ModInstallResult.Ok ? 0 : 1;
        }
        if(commandLine.TryGetValue("remove",out string? remove))
        {
            bool result = App.UninstallMod(remove);
            return result ? 0 : 1;
        }
        if(commandLine.ContainsKey("register-scheme"))
        {
            bool result = Win32.CreateURIScheme();
            return result ? 0 : 1;
        }
        if(commandLine.ContainsKey("apply"))
        {
            App.ApplyMods();
            return 0;
        }
        if(commandLine.ContainsKey("revert"))
        {
            App.Revert();
            return 0;
        }
        if(commandLine.ContainsKey("create"))
        {
            App.CreateMod(id);
            return 0;
        }
        if(commandLine.ContainsKey("create-gfs"))
        {
            Application.CreateGFS(input,output);
            return 0;
        }
        if(commandLine.ContainsKey("create-tfhres"))
        {
            Application.CreateTFHRES(output);
            return 0;
        }
        if(commandLine.ContainsKey("extract"))
        {
            Application.Extract(input,output);
            return 0;
        }
        if(commandLine.ContainsKey("enable"))
        {
            App.EnableMod(id);
            return 0;
        }
        if(commandLine.ContainsKey("disable"))
        {
            App.DisableMod(id);
            return 0;
        }
        if(commandLine.ContainsKey("list"))
        {
            List<Mod> mods = App.ModDB.Mods;
            if(mods.Count == 0)
            {
                Velvet.Info("no mods are installed");
                return 0;
            }
            Velvet.Info($"{mods.Count} mods installed:");
            foreach(Mod mod in mods)
            {
                Console.WriteLine();
                PrintMod(mod);
            }
            return 0;
        }
        if(commandLine.ContainsKey("reset"))
        {
            if(!forced)
            {
                Velvet.Info("are you SURE you want to reset?");
                string confirm = Velvet.Velvetify($"Yes I want to Reset {Velvet.NAME}");
                Velvet.Info($"type '{confirm}' to confirm this action: ");
                string? confirmed = Console.ReadLine();
                if(confirmed == null)
                {
                    Velvet.Error("you didn't type anything, nothing will happen");
                    return 1;
                }
                if(confirmed != confirm)
                {
                    Velvet.Error("you didn't type the required text, nothing will happen");
                    return 1;
                }
            }
            Velvet.Info("I hope you know what you're doing...");
            App.Reset();
            return 0;
        }
        Velvet.Info("no parameters specified, check out the guide for usage:");
        Console.WriteLine("https://github.com/ThemModdingHerds/velvet-beautifier/blob/main/CLI.md");
        return 1;
    }
    private static void PrintMod(Mod mod)
    {
        string status = mod.Enabled ? "Enabled" : "Disabled";
        Velvet.Info($"{mod.Info.Name} v{mod.Info.Version} by {mod.Info.Author} ({status})");
        if(mod.Info.Url != null) Console.WriteLine(mod.Info.Url);
        Console.WriteLine();
        Velvet.Info(mod.Info.Description);
    }
}