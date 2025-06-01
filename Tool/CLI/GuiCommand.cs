using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Tool.GUI;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.CLI;
public class GuiCommand : ICommandArgumentHandler
{
    public string Name => "gui";
    public string Description => "Open the GUI inside the terminal";
    public int OnExecute(string? value)
    {
        try
        {
            Application.Init();

            MainTopLevel top = new();
            
            Application.Run(top);
            Application.Shutdown();
            return 0;
        }
        catch (Exception ex)
        {
            Velvet.Error(ex);
            return 1;
        }
    }
}