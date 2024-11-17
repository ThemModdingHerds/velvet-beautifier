using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public class About : AboutDialog
{
    public About()
    {
        ((Dialog)ControlObject).Icon = Utils.WindowIcon;
        Title = Velvet.Velvetify(Title);
        Version = Dotnet.LibraryVersion.ToString();
        ProgramName = Velvet.Velvetify(Velvet.NAME);
        Developers = [Velvet.Velvetify(Velvet.AUTHOR)];
        License = Utils.License;
        Logo = Utils.VelvetImage;
        WebsiteLabel = Velvet.Velvetify("Source Code");
        Website = new System.Uri(Velvet.REPO_URL);
    }
}