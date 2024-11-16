using System.Reflection;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Dotnet
{
    public static Assembly Library {get => Assembly.GetExecutingAssembly();}
    public static AssemblyName LibraryName {get => Library.GetName();}
    public static Version LibraryVersion {get => LibraryName.Version ?? new();}
    public static Assembly? Executable {get => Assembly.GetEntryAssembly();}
    public static string? ExecutablePath {get => Executable?.Location;}
    public static string ExecutableFolder {get => Path.GetDirectoryName(ExecutablePath) ?? Environment.CurrentDirectory;}
}