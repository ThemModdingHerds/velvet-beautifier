using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.GFS;
using ThemModdingHerds.VelvetBeautifier.Patches;
using ThemModdingHerds.VelvetBeautifier.TFHResource;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains methods for creating backups of files
/// </summary>
public static class BackupManager
{
    /// <summary>
    /// The foldername where the backups are stored
    /// </summary>
    public const string FOLDERNAME = "backup";
    /// <summary>
    /// The file extension of a backup
    /// </summary>
    public const string FILE_EXT = ".bak";
    /// <summary>
    /// The filename when a invalid file was detected
    /// </summary>
    public const string INVALID_FILENAME = "invalid";
    /// <summary>
    /// The full path to the folder where backups are stored
    /// </summary>
    public static string Folder => Path.Combine(Velvet.AppDataFolder,FOLDERNAME);
    /// <summary>
    /// The full filepath of the invalid file
    /// </summary>
    public static string InvalidFilePath => Path.Combine(Folder,INVALID_FILENAME);
    /// <summary>
    /// Are the backups invalid? This happens if the game files have been tampered with already
    /// </summary>
    public static bool Invalid => File.Exists(InvalidFilePath);
    /// <summary>
    /// Initialize the backup manager
    /// </summary>
    public static void Init()
    {
        Clear();
        MakeBackup(ModLoaderTool.Client);
        MakeBackup(ModLoaderTool.Server);
    }
    /// <summary>
    /// Get the backup filename of <c>filepath</c>
    /// </summary>
    /// <param name="filepath">A filepath</param>
    /// <returns>The backup filename of <c>filepath</c></returns>
    public static string GetBackupName(string filepath) => Path.GetFileName(filepath) + FILE_EXT;
    /// <summary>
    /// Get the full backup filepath of <c>filepath</c>
    /// </summary>
    /// <param name="filepath">A filepath</param>
    /// <returns>The full backup filepath of <c>filepath</c></returns>
    public static string GetBackupPath(string filepath) => Path.Combine(Folder, GetBackupName(filepath));
    /// <summary>
    /// Create a backup of <c>filepath</c> if it doesn't exist
    /// </summary>
    /// <param name="filepath">A valid filepath</param>
    /// <returns>The full backup filepath of <c>filepath</c></returns>
    public static string MakeBackup(string filepath)
    {
        string backup = GetBackupPath(filepath);
        // if it already exists, just return it
        if(ExistsBackup(filepath))
            return backup;
        Velvet.Info($"creating backup of {Path.GetFileName(filepath)}");
        if(!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);
        File.Copy(filepath,backup,true);
        return backup;
    }
    /// <summary>
    /// Create a backup of <c>filepath</c> and check if it matches <c>gamefile</c>
    /// </summary>
    /// <param name="filepath">A valid filepath</param>
    /// <param name="gamefile">A game file</param>
    /// <returns>true if <c>filepath</c> matches the gamefiles, otherwise false</returns>
    public static bool MakeBackup(string filepath,GameFile? gamefile)
    {
        // return false if the gamefile is invalid
        bool tampered = !gamefile?.Verify(filepath) ?? false;
        if(tampered && gamefile != null)
        {
            Velvet.Warn($"{gamefile.Name} has been tampered with! It might cause problems");
            if(!File.Exists(InvalidFilePath))
                FileSystem.CreateFile(InvalidFilePath);
        }
        MakeBackup(filepath);
        return tampered;
    }
    /// <summary>
    /// Create backup of <c>game</c>'s game files
    /// </summary>
    /// <param name="game">A valid game instance</param>
    private static void MakeBackup(Game? game)
    {
        // game must be valid
        if(game == null || !game.Valid()) return;
        RevergePackageManager.CreateBackup(game);
        TFHResourceManager.CreateBackup(game);
        GameNewsManager.CreateBackup(game);
    }
    /// <summary>
    /// Check if there's a backup of <c>filepath</c>
    /// </summary>
    /// <param name="filepath">A valid filepath</param>
    /// <returns>true if there's a backup, otherwise false</returns>
    public static bool ExistsBackup(string filepath) => File.Exists(Path.Combine(Folder, GetBackupName(filepath)));
    /// <summary>
    /// Copy the backup of <c>filepath</c> to <c>filepath</c>, <c>filepath</c> MUST be the orignal file
    /// </summary>
    /// <param name="filepath">A valid filepath</param>
    public static void Revert(string filepath)
    {
        if(!ExistsBackup(filepath))
            return;
        string backup = GetBackupPath(filepath);
        File.Copy(backup,filepath,true);
    }
    /// <summary>
    /// Restore <c>game</c> to its orignal state
    /// </summary>
    /// <param name="game">A valid game instance</param>
    private static void Revert(Game? game)
    {
        // game must be valid
        if(game == null || !game.Valid()) return;
        Velvet.Info($"reverting game files from {game.ExecutableName}");
        RevergePackageManager.Revert(game);
        TFHResourceManager.Revert(game);
        GameNewsManager.Revert(game);
        PatchManager.Revert(game);
    }
    /// <summary>
    /// Restore the client's and/or server's game files to their orignal state
    /// </summary>
    public static void Revert()
    {
        Revert(ModLoaderTool.Client);
        Revert(ModLoaderTool.Server);
    }
    /// <summary>
    /// Check if the folder where backups are stored exists
    /// </summary>
    /// <returns>true if the folder exists, otherwise false</returns>
    public static bool Exists() => Directory.Exists(Folder);
    /// <summary>
    /// Restore the backups and delete the folder of the backups
    /// </summary>
    public static void Clear()
    {
        Revert();
        if(Directory.Exists(Folder))
            Directory.Delete(Folder,true);
    }
}