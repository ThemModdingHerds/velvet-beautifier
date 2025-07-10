using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;

public static class ActionDialog
{
    public enum Type
    {
        Apply,
        Revert,
        LaunchClient,
        LaunchServer,
        ExtractFiles
    }
    public static Action Show(Type type)
    {
        return type switch
        {
            Type.Apply => ApplyMods,
            Type.Revert => Revert,
            Type.LaunchClient => LaunchClient,
            Type.LaunchServer => LaunchServer,
            Type.ExtractFiles => ExtractFiles,
            _ => throw new Exception()
        };
    }
    private static void ApplyMods()
    {
        ModDB.Apply();
        MessageBox.Query("Applied mods", "Mods have been applied! You can now launch the game", "Ok");
    }
    private static void Revert()
    {
        BackupManager.Revert();
        MessageBox.Query("Reverted", "Reverted back to orignal game, no mods applied", "Ok");
    }
    private static void LaunchClient()
    {
        if (ModLoaderTool.Client == null)
        {
            MessageBox.ErrorQuery("No client", "No client has been provided in your configuration! Check if they are available/correct!", "Ok");
            return;
        }
        ModLoaderTool.Client.Launch();
    }
    private static void LaunchServer()
    {
        if (ModLoaderTool.Server == null)
        {
            MessageBox.ErrorQuery("no server", "No server has been provided in your configuration! Check if they are available/correct!", "Ok");
            return;
        }
        ModLoaderTool.Server.Launch();
    }
    private static void ExtractFiles()
    {
        OpenDialog input = new("Extract file", "Select file to extract", null, OpenDialog.OpenMode.File)
        {
            AllowsMultipleSelection = false
        };
        Application.Run(input);
        if (!File.Exists(input.FilePath.ToString())) return;
        OpenDialog output = new("Extraction output", "Select the folder to output extracted files", null, OpenDialog.OpenMode.Directory)
        {
            AllowsMultipleSelection = false
        };
        if (!Directory.Exists(input.FilePath.ToString())) return;
        bool result = ModLoaderTool.Extract(input.FilePath.ToString(), output.FilePath.ToString());
        if (result)
            MessageBox.Query("Extraction complete", $"Extracted {Path.GetFileName(input.FilePath.ToString())} to the output directory", "Ok");
    }
}