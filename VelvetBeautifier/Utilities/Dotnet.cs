using System.Reflection;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various fields that have something to do with .NET
/// </summary>
public static class Dotnet
{
    /// <summary>
    /// Returns the Assembly of the method that invoked the currently executing method.
    /// </summary>
    public static Assembly Calling => Assembly.GetCallingAssembly();
    /// <summary>
    /// Gets the process executable in the default application domain. In other application domains, this is the first executable that was executed by AppDomain.ExecuteAssembly(string).
    /// </summary>
    public static Assembly? Entry => Assembly.GetEntryAssembly();
    /// <summary>
    /// Gets the assembly that contains the code that is currently executing.
    /// </summary>
    public static Assembly Executing => Assembly.GetExecutingAssembly();
    /// <summary>
    /// Alias of <c>Executing</c>
    /// </summary>
    public static Assembly Library => Executing;
    /// <summary>
    /// Gets the AssemblyName of the library
    /// </summary>
    public static AssemblyName LibraryName => Library.GetName();
    /// <summary>
    /// Gets the Version of the library
    /// </summary>
    public static Version LibraryVersion => LibraryName.Version ?? new();
    /// <summary>
    /// Alias of <c>Entry</c>
    /// </summary>
    public static Assembly? ExecutableDll => Entry;
    /// <summary>
    /// Gets the filepath of the .dll file of the executable
    /// </summary>
    public static string? ExecutableDllPath => ExecutableDll?.Location;
    /// <summary>
    /// Gets the filepath of the executable
    /// </summary>
    public static string? ExecutablePath => Path.ChangeExtension(ExecutableDllPath, ".exe");
    /// <summary>
    /// Gets the folder of the executable
    /// </summary>
    public static string ExecutableFolder => Path.GetDirectoryName(ExecutableDllPath) ?? Environment.CurrentDirectory;
    /// <summary>
    /// Get the GameNews-ModList.png as a Resource Stream
    /// </summary>
    /// <returns>The Stream of the resouce, should be always valid</returns>
    public static Stream GetGameNewsModsListResource() => Library.GetManifestResourceStream(Velvet.GAMENEWS_MODLIST_RESOURCE_PATH) ?? throw new Exception("impossible");
}