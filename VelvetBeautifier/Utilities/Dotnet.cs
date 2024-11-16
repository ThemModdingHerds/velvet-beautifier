using System.Reflection;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Dotnet
{
    public static Assembly Library {get => Assembly.GetExecutingAssembly();}
    public static AssemblyName LibraryName {get => Library.GetName();}
    public static Version LibraryVersion {get => LibraryName.Version ?? new();}
    public static Assembly? ExecutableDll {get => Assembly.GetEntryAssembly();}
    public static string? ExecutableDllPath {get => ExecutableDll?.Location;}
    public static string? ExecutablePath {get => Path.ChangeExtension(ExecutableDllPath,".exe");}
    public static string ExecutableFolder {get => Path.GetDirectoryName(ExecutableDllPath) ?? Environment.CurrentDirectory;}
}