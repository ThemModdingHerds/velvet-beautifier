using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;
public static class DeleteDialog
{
    public enum Type
    {
        Backup,
        Mods,
        Everything
    }
    public static Action Show(Type type)
    {
        return type switch
        {
            Type.Backup => DeleteBackups,
            Type.Mods => DeleteMods,
            Type.Everything => DeleteEverything,
            _ => throw new Exception(),
        };
    }
    private static void DeleteBackups()
    {
        int result = MessageBox.Query("Delete backups?", "Are you sure you want to delete the backup files?", "Yes", "No");
        if (result == 0)
        {
            BackupManager.Clear();
            MessageBox.Query("Deleted backups", "The backup files have been deleted", "Ok");
        }
    }
    private static void DeleteMods()
    {
        int result = MessageBox.Query("Delete mods?", "Are you sure you want to delete all mods?", "Yes", "No");
        if (result == 0)
        {
            BackupManager.Revert();
            ModDB.Clear();
            MessageBox.Query("Deleted mods", "The mods have been deleted", "Ok");
        }
    }
    private static void DeleteEverything()
    {
        int result = MessageBox.Query("Delete everything?", "Are you sure you want to delete everything?", "Yes", "No");
        if (result == 0)
        {
            BackupManager.Revert();
            ModLoaderTool.DeleteEverything();
            MessageBox.Query("Deleted everything", "Everything has been deleted! The software will close after this window!", "Ok");
            Application.RequestStop();
        }
    }
}