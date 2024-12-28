using ThemModdingHerds.VelvetBeautifier.Modding;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class RegisterSchemeHandler : ICommandArgumentHandler
{
    public string Name => "register-scheme";
    public int OnExecute(string? value)
    {
        bool result = Win32.CreateURIScheme();
        return result ? 0 : 1;
    }
}
public class InstallModHandler : ICommandArgumentHandler
{
    public string Name => "install";
    public int OnExecute(string? value)
    {
        ModInstallResult result = ModDB.InstallMod(value);
        return result == ModInstallResult.Ok ? 0 : 1;
    }
}
public class RemoveModHandler : ICommandArgumentHandler
{
    public string Name => "remove";
    public int OnExecute(string? value)
    {
        bool result = ModDB.UninstallMod(value);
        return result ? 0 : 1;
    }
}
public class ApplyModsHandler : ICommandArgumentHandler
{
    public string Name => "apply";
    public int OnExecute(string? value)
    {
        ModLoaderTool.ApplyMods();
        return 0;
    }
}
public class RevertHandler : ICommandArgumentHandler
{
    public string Name => "revert";
    public int OnExecute(string? value)
    {
        ModLoaderTool.Revert();
        return 0;
    }
}
public class CreateModHandler : ICommandArgumentHandler
{
    public string Name => "create";
    public int OnExecute(string? value)
    {
        ModInstallResult result = ModDB.Create(value);
        return result == ModInstallResult.Ok ? 0 : 1;
    }
}
public class CreateRevergePackageHandler : ICommandArgumentHandler
{
    public string Name => "create-gfs";
    public int OnExecute(string? value)
    {
        if(!CommandLine.Arguments.TryGetValue("input",out string? input))
        {
            Velvet.Error("'input' parameter is missing");
            return 1;
        }
        if(!CommandLine.Arguments.TryGetValue("output",out string? output))
        {
            Velvet.Error("'output' parameter is missing");
            return 1;
        }
        GFS.Utils.Create(input,output);
        return 0;
    }
}
public class CreateTFHResourceHandler : ICommandArgumentHandler
{
    public string Name => "create-tfhres";
    public int OnExecute(string? value)
    {
        bool result = TFHResource.Utils.CreateEmpty(value);
        return result ? 0 : 1;
    }
}
public class ExtractHandler : ICommandArgumentHandler
{
    public string Name => "extract";
    public int OnExecute(string? value)
    {
        if(!CommandLine.Arguments.TryGetValue("input",out string? input))
        {
            Velvet.Error("'input' parameter is missing");
            return 1;
        }
        if(!CommandLine.Arguments.TryGetValue("output",out string? output))
        {
            Velvet.Error("'output' parameter is missing");
            return 1;
        }
        if(input == null || output == null) return 1;
        string inputPath = Path.Combine(Environment.CurrentDirectory,input);
        string outputPath = Path.Combine(Environment.CurrentDirectory,output);
        if(inputPath.EndsWith(".gfs"))
        {
            Velvet.Info($"extracting GFS at {inputPath} to {outputPath}...");
            GFS.Utils.Extract(inputPath,outputPath);
            return 0;
        }
        if(inputPath.EndsWith(".tfhres"))
        {
            Velvet.Info($"extracting GFS at {inputPath} to {outputPath}...");
            TFHResource.Utils.Extract(inputPath,outputPath);
            return 0;
        }
        Velvet.Error($"cannot extract {Path.GetFileName(inputPath)}");
        return 1;
    }
}
public class EnableModHandler : ICommandArgumentHandler
{
    public string Name => "enable";
    public int OnExecute(string? value)
    {
        Mod? mod = ModDB.FindModById(value);
        if(mod == null)
            return 1;
        mod.Enable();
        return 0;
    }
}
public class DisableModHandler : ICommandArgumentHandler
{
    public string Name => "disable";
    public int OnExecute(string? value)
    {
        Mod? mod = ModDB.FindModById(value);
        if(mod == null)
            return 1;
        mod.Enable();
        return 0;
    }
}
public class ListModsHandler : ICommandArgumentHandler
{
    public string Name => "list";
    public int OnExecute(string? value)
    {
        List<Mod> mods = ModDB.Mods;
        if(mods.Count == 0)
        {
            Velvet.Warn("no mods are installed");
            return 0;
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
        return 0;
    }
}
public class ResetHandler : ICommandArgumentHandler
{
    public string Name => "reset";
    public int OnExecute(string? value)
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
                return 1;
            }
            if(confirmed != confirm)
            {
                Velvet.Error("you didn't type the required text, nothing will happen");
                return 1;
            }
        }
        Velvet.Error("I hope you know what you're doing...");
        ModLoaderTool.Reset();
        return 0;
    }
}