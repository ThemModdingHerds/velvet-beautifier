namespace ThemModdingHerds.VelvetBeautifier;
public static class BackupManager
{
    public static string GetBackupName(string path)
    {
        return Path.GetFileName(path) + ".bak";
    }
    public static string GetBackupPath(string path)
    {
        return Path.Combine(Config.Current.BackupFolder,GetBackupName(path));
    }
    public static void MakeBackup(string path)
    {
        string filepath = GetBackupPath(path);
        if(!Config.Current.ExistsBackupFolder())
            Directory.CreateDirectory(Config.Current.BackupFolder);
        File.Copy(path,filepath,true);
    }
    public static bool ExistsBackup(string path)
    {
        return File.Exists(Path.Combine(Config.Current.BackupFolder,GetBackupName(path)));
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
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            MakeBackup(file);
    }
    public static void BackupTFHResources()
    {
        BackupFolder(Config.Current.GetTFHResourcesFolder());
    }
    public static void RevertTFHResources()
    {
        RevertFolder(Config.Current.GetTFHResourcesFolder());
    }
    public static void BackupData01()
    {
        BackupFolder(Config.Current.GetData01Folder());
    }
    public static void RevertData01()
    {
        RevertFolder(Config.Current.GetData01Folder());
    }
}