namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class BackupManager(string folder)
{
    public static readonly string BACKUP_FOLDER = Path.Combine(Environment.CurrentDirectory,"backup");
    public string Folder {get;} = folder;
    public BackupManager() : this(BACKUP_FOLDER)
    {

    }
    public static string GetBackupName(string path)
    {
        return Path.GetFileName(path) + ".bak";
    }
    public string GetBackupPath(string path)
    {
        return Path.Combine(Folder,GetBackupName(path));
    }
    public string MakeBackup(string path)
    {
        string filepath = GetBackupPath(path);
        if(ExistsBackup(path))
            return filepath;
        if(!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);
        File.Copy(path,filepath,true);
        return filepath;
    }
    public bool ExistsBackup(string path)
    {
        return File.Exists(Path.Combine(Folder,GetBackupName(path)));
    }
    public void Revert(string path)
    {
        if(!ExistsBackup(path))
            return;
        string filepath = GetBackupPath(path);
        File.Copy(filepath,path,true);
    }
    public void RevertFolder(string folder)
    {
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            Revert(file);
    }
    public void BackupFolder(string folder)
    {
        if(ExistsBackupFolder(folder))
            return;
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            MakeBackup(file);
    }
    public bool ExistsBackupFolder(string folder)
    {
        string[] files = Directory.GetFiles(folder);
        foreach(string file in files)
            if(!ExistsBackup(file))
                return false;
        return true;
    }
}