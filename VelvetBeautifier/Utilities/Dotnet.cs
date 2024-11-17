using System.Reflection;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Dotnet
{
    public static Assembly Calling {get => Assembly.GetCallingAssembly();}
    public static Assembly? Entry {get => Assembly.GetEntryAssembly();}
    public static Assembly Executing {get => Assembly.GetExecutingAssembly();}
    public static Assembly Library {get => Executing;}
    public static AssemblyName LibraryName {get => Library.GetName();}
    public static Version LibraryVersion {get => LibraryName.Version ?? new();}
    public static Assembly? ExecutableDll {get => Entry;}
    public static string? ExecutableDllPath {get => ExecutableDll?.Location;}
    public static string? ExecutablePath {get => Path.ChangeExtension(ExecutableDllPath,".exe");}
    public static string ExecutableFolder {get => Path.GetDirectoryName(ExecutableDllPath) ?? Environment.CurrentDirectory;}
    public static bool IsConsoleAvailable {get
    {
        try
        {
            int height = Console.WindowHeight;
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }}
}