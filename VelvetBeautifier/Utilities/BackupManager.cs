namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class BackupManager
{
    public const string FOLDERNAME = "backup";
    public const string FILE_EXT = ".bak";
    public static string Folder => Path.Combine(Dotnet.ExecutableFolder,FOLDERNAME);
    public static string GetBackupName(string path)
    {
        return Path.GetFileName(path) + FILE_EXT;
    }
    public static string GetBackupPath(string path)
    {
        return Path.Combine(Folder,GetBackupName(path));
    }
    public static string MakeBackup(string path)
    {
        string filepath = GetBackupPath(path);
        if(ExistsBackup(path))
            return filepath;
        Velvet.Info($"creating backup of {Path.GetFileName(path)}");
        if(!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);
        File.Copy(path,filepath,true);
        return filepath;
    }
    public static bool MakeBackup(string path,Checksum checksum)
    {
        bool tampered = !checksum.Verify(path);
        if(tampered)
            Velvet.Warn($"{checksum.Name} has been tampered with! It might cause problems");
        MakeBackup(path);
        return tampered;
    }
    public static bool ExistsBackup(string path)
    {
        return File.Exists(Path.Combine(Folder,GetBackupName(path)));
    }
    public static void Revert(string path)
    {
        if(!ExistsBackup(path))
            return;
        string filepath = GetBackupPath(path);
        File.Copy(filepath,path,true);
    }
    public static void RevertFolder(string folder)
    {
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            Revert(file);
    }
    public static void BackupFolder(string folder)
    {
        if(ExistsBackupFolder(folder))
            return;
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            MakeBackup(file);
    }
    public static bool ExistsBackupFolder(string folder)
    {
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            if(!ExistsBackup(file))
                return false;
        return true;
    }
    public static void Clear()
    {
        if(Directory.Exists(Folder))
            Directory.Delete(Folder,true);
    }
}