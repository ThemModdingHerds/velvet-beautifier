using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// This class contains methods and fields of the game, this can be the Client (actual game) or the Server
/// </summary>
/// <param name="folder">The folder of the game</param>
public abstract class Game(string folder)
{
    /// <summary>
    /// Name of the game
    /// </summary>
    public const string NAME = "Them's Fightin' Herds";
    /// <summary>
    /// The game installation folder
    /// </summary>
    public string Folder => folder;
    /// <summary>
    /// The filename of the executable
    /// </summary>
    public abstract string ExecutableName {get;}
    /// <summary>
    /// The full path of the executable
    /// </summary>
    public string Executable => Path.Combine(folder,ExecutableName);
    /// <summary>
    /// The full path of the <c>Scripts</c> folder
    /// </summary>
    public string ScriptsFolder => Path.Combine(Folder,"Scripts");
    public string Data01Folder => Path.Combine(Folder,"data01");
    public string ResourcesFolder => Path.Combine(Folder,"Scripts","src","Farm","resources");
    public bool HasData01 => Directory.Exists(Data01Folder);
    public bool HasResources => Directory.Exists(ResourcesFolder);
    /// <summary>
    /// Check if this is a valid installation
    /// </summary>
    /// <returns></returns>
    public abstract bool Valid();
    /// <summary>
    /// Launch the executable
    /// </summary>
    public virtual void Launch()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = Executable,
            Verb = "open",
            WorkingDirectory = Path.GetDirectoryName(Executable)
        });
    }
}
/// <summary>
/// This class contains methods and fields for the client (the game you play)
/// </summary>
/// <param name="folder">The folder of the client installation</param>
public class Client(string folder) : Game(folder)
{
    public override string ExecutableName {
        get
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "Them's Fightin' Herds.exe";
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "ThemsFightinHerds.Linux.x64";
            if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "Them's Fighting Herds.app/Contents/MacOS/Them's Fighting Herds";
            return string.Empty;
        }
    }
    /// <summary>
    /// The path to the game if found
    /// </summary>
    public static string? FoundPath => Steam.GetGamePath() ?? EpicGames.GetGamePath() ?? null;
    public override bool Valid()
    {
        return File.Exists(Executable) && HasResources && HasData01;
    }
}
/// <summary>
/// This class contains methods and fields for the server
/// </summary>
/// <param name="folder">The folder of the server</param>
public class Server(string folder) : Game(folder)
{
    public override string ExecutableName {
        get
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "LobbyExe.exe";
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return "LobbyServer.Linux.x64";
            return string.Empty;
        }
    }
    public override bool Valid()
    {
        return File.Exists(Executable) && HasResources;
    }
}
public class GameEventArgs(Game game) : EventArgs
{
    public Game Game => game;
}