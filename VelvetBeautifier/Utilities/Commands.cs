using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class RegisterSchemeHandler : ICommandArgumentHandler
{
    public string Name => "register-scheme";
    public void OnExecute(string? value)
    {
        bool result = Win32.CreateURIScheme();
        Environment.Exit(result ? 0 : 1);
    }
}
public class InstallModHandler : ICommandArgumentHandler
{
    public string Name => "install";
    public void OnExecute(string? value)
    {
        ModInstallResult result = ModDB.InstallMod(value);
        Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
    }
}
public class RemoveModHandler : ICommandArgumentHandler
{
    public string Name => "remove";
    public void OnExecute(string? value)
    {
        bool result = ModDB.UninstallMod(value);
        Environment.Exit(result ? 0 : 1);
    }
}
public class ApplyModsHandler : ICommandArgumentHandler
{
    public string Name => "apply";
    public void OnExecute(string? value)
    {
        ModLoaderTool.ApplyMods();
        Environment.Exit(0);
    }
}
public class RevertHandler : ICommandArgumentHandler
{
    public string Name => "revert";
    public void OnExecute(string? value)
    {
        ModLoaderTool.Revert();
        Environment.Exit(0);
    }
}
public class CreateModHandler : ICommandArgumentHandler
{
    public string Name => "create";
    public void OnExecute(string? value)
    {
        ModInstallResult result = ModDB.Create(value);
        Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
    }
}
public class CreateRevergePackageHandler : ICommandArgumentHandler
{
    public string Name => "create-gfs";
    public void OnExecute(string? value)
    {
        if(
            CommandLine.Arguments.TryGetValue("input",out string? input) &&
            CommandLine.Arguments.TryGetValue("output",out string? output)
        )
            GFS.Utils.Create(input,output);
        Environment.Exit(0);
    }
}
public class CreateTFHResourceHandler : ICommandArgumentHandler
{
    public string Name => "create-tfhres";
    public void OnExecute(string? value)
    {
        TFHResource.Utils.CreateEmpty(value);
        Environment.Exit(0);
    }
}
public class ExtractHandler : ICommandArgumentHandler
{
    public string Name => "extract";
    public void OnExecute(string? value)
    {
        if(
            CommandLine.Arguments.TryGetValue("input",out string? input) &&
            CommandLine.Arguments.TryGetValue("output",out string? output)
        )
        {
            if(input == null || output == null) Environment.Exit(1);
            string inputPath = Path.Combine(Environment.CurrentDirectory,input);
            string outputPath = Path.Combine(Environment.CurrentDirectory,output);
            if(inputPath.EndsWith(".gfs"))
            {
                Velvet.Info($"extracting GFS at {inputPath} to {outputPath}...");
                GFS.Utils.Extract(inputPath,outputPath);
                Environment.Exit(0);
            }
            if(inputPath.EndsWith(".tfhres"))
            {
                Velvet.Info($"extracting GFS at {inputPath} to {outputPath}...");
                TFHResource.Utils.Extract(inputPath,outputPath);
                Environment.Exit(0);
            }
        }
        Environment.Exit(1);
    }
}
public class EnableModHandler : ICommandArgumentHandler
{
    public string Name => "enable";
    public void OnExecute(string? value)
    {
        Mod? mod = ModDB.FindModById(value);
        if(mod == null)
            Environment.Exit(1);
        mod.Enable();
        Environment.Exit(0);
    }
}
public class DisableModHandler : ICommandArgumentHandler
{
    public string Name => "disable";
    public void OnExecute(string? value)
    {
        Mod? mod = ModDB.FindModById(value);
        if(mod == null)
            Environment.Exit(1);
        mod.Enable();
        Environment.Exit(0);
    }
}
public class ListModsHandler : ICommandArgumentHandler
{
    public string Name => "list";
    public void OnExecute(string? value)
    {
        List<Mod> mods = ModDB.Mods;
        if(mods.Count == 0)
        {
            Velvet.Warn("no mods are installed");
            Environment.Exit(0);
        }
        Velvet.Info($"{mods.Count} mods installed:");
        foreach(Mod mod in mods)
        {
            Console.WriteLine();
            Velvet.Info(mod.ToString());
            if(mod.Info.Url != null) Console.WriteLine(mod.Info.Url);
            Console.WriteLine();
            Velvet.Info(mod.Info.Description);
        }
        Environment.Exit(0);
    }
}
public class ResetHandler : ICommandArgumentHandler
{
    public string Name => "reset";
    public void OnExecute(string? value)
    {
        bool forced = CommandLine.Arguments.ContainsKey("force");
        if(!forced)
        {
            Velvet.Warn("are you SURE you want to reset?");
            string confirm = Velvet.Velvetify($"Yes I want to Reset {Velvet.NAME}");
            Velvet.Warn($"type '{confirm}' to confirm this action: ");
            string? confirmed = Console.ReadLine();
            if(confirmed == null)
            {
                Velvet.Error("you didn't type anything, nothing will happen");
                Environment.Exit(1);
            }
            if(confirmed != confirm)
            {
                Velvet.Error("you didn't type the required text, nothing will happen");
                Environment.Exit(1);
            }
        }
        Velvet.Error("I hope you know what you're doing...");
        ModLoaderTool.Reset();
        Environment.Exit(0);
    }
}