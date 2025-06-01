using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.CLI.GUI;
public class GuiCommand : ICommandArgumentHandler
{
    public string Name => "gui";

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