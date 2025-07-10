using System.Text;
using Terminal.Gui;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Tool.GUI.Dialogs;

public class AboutDialog : Dialog
{
    private class OkButton : Button
    {
        public OkButton(): base("Ok")
        {
            Clicked += () => Application.RequestStop();
        }
    }
    public static void Show() => Application.Run(new AboutDialog());
    private AboutDialog() : base($"About {Velvet.NAME}", 60, 8, new OkButton())
    {
        string[] lines = [
            $"v{Dotnet.LibraryVersion} - {Velvet.DESCRIPTION}",
            $"{GitHubUtilities.REPO_URL}"
        ];
        Label about = new(string.Join('\n', lines))
        {
            X = Pos.Center(),
            Y = Pos.Center()
        };
        Add(about);
    }
}