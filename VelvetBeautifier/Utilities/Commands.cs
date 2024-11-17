using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class RegisterSchemeHandler : ICommandArgumentHandler
{
    public string Name => "register-scheme";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        bool result = Win32.CreateURIScheme();
        Environment.Exit(result ? 0 : 1);
    }
}
public class InstallModHandler : ICommandArgumentHandler
{
    public string Name => "install";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        var task = application.InstallMod(value);
        task.Wait();
        ModInstallResult result = task.Result;
        Environment.Exit(result == ModInstallResult.Ok ? 0 : 1);
    }
}
public class RemoveModHandler : ICommandArgumentHandler
{
    public string Name => "remove";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        bool result = application.UninstallMod(value);
        Environment.Exit(result ? 0 : 1);
    }
}
public class ApplyModsHandler : ICommandArgumentHandler
{
    public string Name => "apply";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        application.ApplyMods();
        Environment.Exit(0);
    }
}
public class RevertHandler : ICommandArgumentHandler
{
    public string Name => "revert";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        application.Revert();
        Environment.Exit(0);
    }
}
public class CreateModHandler : ICommandArgumentHandler
{
    public string Name => "create";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        application.CreateMod(value);
        Environment.Exit(0);
    }
}
public class CreateRevergePackageHandler : ICommandArgumentHandler
{
    public string Name => "create-gfs";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        if(
            application.CommandLine.Arguments.TryGetValue("input",out string? input) &&
            application.CommandLine.Arguments.TryGetValue("output",out string? output)
        )
            ModLoaderTool.CreateRevergePackage(input,output);
        Environment.Exit(0);
    }
}
public class CreateTFHResourceHandler : ICommandArgumentHandler
{
    public string Name => "create-tfhres";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        if(application.CommandLine.Arguments.TryGetValue("output",out string? output))
            ModLoaderTool.CreateTFHResource(output);
        Environment.Exit(0);
    }
}
public class ExtractHandler : ICommandArgumentHandler
{
    public string Name => "extract";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        if(
            application.CommandLine.Arguments.TryGetValue("input",out string? input) &&
            application.CommandLine.Arguments.TryGetValue("output",out string? output)
        )
            ModLoaderTool.Extract(input,output);
        Environment.Exit(0);
    }
}
public class EnableModHandler : ICommandArgumentHandler
{
    public string Name => "enable";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        if(application.CommandLine.Arguments.TryGetValue("id",out string? id))
            application.EnableMod(id);
        Environment.Exit(0);
    }
}
public class DisableModHandler : ICommandArgumentHandler
{
    public string Name => "disable";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        if(application.CommandLine.Arguments.TryGetValue("id",out string? id))
            application.DisableMod(id);
        Environment.Exit(0);
    }
}
public class ListModsHandler : ICommandArgumentHandler
{
    public string Name => "list";
    public void OnExecute(ModLoaderTool application,string? value)
    {
        List<Mod> mods = application.ModDB.Mods;
        if(mods.Count == 0)
        {
            Velvet.Info("no mods are installed");
            Environment.Exit(0);
        }
        Velvet.Info($"{mods.Count} mods installed:");
        foreach(Mod mod in mods)
        {
            Console.WriteLine();
            string status = mod.Enabled ? "Enabled" : "Disabled";
            Velvet.Info($"{mod.Info.Name} v{mod.Info.Version} by {mod.Info.Author} ({status})");
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
    public void OnExecute(ModLoaderTool application,string? value)
    {
        bool forced = application.CommandLine.Arguments.ContainsKey("force");
        if(!forced)
        {
            Velvet.Info("are you SURE you want to reset?");
            string confirm = Velvet.Velvetify($"Yes I want to Reset {Velvet.NAME}");
            Velvet.Info($"type '{confirm}' to confirm this action: ");
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
        Velvet.Info("I hope you know what you're doing...");
        application.Reset();
        Environment.Exit(0);
    }
}