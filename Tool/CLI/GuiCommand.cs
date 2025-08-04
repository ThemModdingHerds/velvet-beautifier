using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Tool.GUI;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.CLI;
public class GuiCommand : ICommandArgumentHandler
{
    public string Name => "gui";
    public string Description => "Open the GUI inside the terminal";
    private Exception? hasException = null;
    public int OnExecute(string? value)
    {
        try
        {
            Application.Init();

            Application.Run(MainTopLevel.Instance, HandleError);
            Application.Shutdown();
        }
        catch (Exception ex)
        {
            hasException = ex;
        }
        if (hasException != null)
        {
            Velvet.Error(hasException);
            return 1;
        }
        return 0;
    }
    private bool HandleError(Exception exception)
    {
        hasException = exception;
        return false;
    }
}