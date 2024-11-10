using System.Reflection;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Dotnet
{
    public static Assembly AsmExe {get => Assembly.GetExecutingAssembly();}
    public static string? ExecutablePath {get => Assembly.GetEntryAssembly()?.Location;}
    public static AssemblyName AsmName {get => AsmExe.GetName();}
    public static Version ExeVersion {get => AsmName.Version ?? new();}
}