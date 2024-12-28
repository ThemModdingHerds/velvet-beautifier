using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI.Commands;
using ThemModdingHerds.VelvetBeautifier.GUI.Commands.File;
using ThemModdingHerds.VelvetBeautifier.GUI.Commands.Help;
using ThemModdingHerds.VelvetBeautifier.GUI.Commands.Tools;
using ThemModdingHerds.VelvetBeautifier.GUI.Interfaces;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI.Items;
public class MenuBar : Eto.Forms.MenuBar, IMainFormItem
{
    public MainForm MainForm {get => (MainForm)Parent;}
    public MenuBar(MainForm parent)
    {
        Parent = parent;
        AboutItem = new AboutCommand(this);
        HelpItems.AddRange([
            new ResetCommand(this),
            new OpenURLCommand(this,Velvet.GITHUB_REPO_GUI_GUIDE_URL,"&How to use this?","Learn how to use Velvet Beautifier"),
            new OpenURLCommand(this,Velvet.GITHUB_REPO_BUG_REPORT_URL,"Report &issue","Report an issue to GitHub"),
            new OpenURLCommand(this,Velvet.GITHUB_REPO_FEATURE_REQUEST_URL,"Request &feature","Suggest an idea for this project")
        ]);
        Items.AddRange([
            CreateFile(),
            CreateTools()
        ]);
    }
    private SubMenuItem CreateFile()
    {
        SubMenuItem installMod = new
        ([
            new InstallFileCommand(this),
            new InstallFolderCommand(this),
            new InstallTextCommand(this)
        ])
        {
            Text = "&Install Mod"
        };
        SubMenuItem file = new
        ([
            new RefreshCommand(this),
            new ApplyCommand(this),
            new RevertCommand(this),
            new LaunchCommand(this),
            installMod,
            new SeparatorMenuItem(),
            new ExitCommand(this)
        ])
        {
            Text = "&File",
        };
        return file;
    }
    private SubMenuItem CreateTools()
    {
        SubMenuItem tools = new
        ([
            new ExtractCommand(this),
            new SeparatorMenuItem(),
            new CreateRevergePackageCommand(this),
            new CreateTFHResouceCommand(this),
            new SeparatorMenuItem(),
            new CreateModCommand(this)
        ])
        {
            Text = "&Tools"
        };

        return tools;
    }
}