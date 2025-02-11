using Gtk;
using ThemModdingHerds.VelvetBeautifier.GitHub;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public static class VelvetGtk
{
    public const string APP_ID = "org.tmh.velvetbeautifier";
    public const string GLADEFILE = "VelvetBeautifier.glade";
    public static void ShowMessageBox(this Window parent,string content,MessageType type = MessageType.Info)
    {
        MessageDialog message = new(parent,DialogFlags.DestroyWithParent,type,ButtonsType.Ok,Velvet.Velvetify(content));
        message.Run();
        message.Destroy();
    }
    public static ResponseType ShowMessageBox(this Window parent,string content,ButtonsType buttons,MessageType type = MessageType.Info)
    {
        MessageDialog message = new(parent,DialogFlags.DestroyWithParent,type,buttons,Velvet.Velvetify(content));
        int result = message.Run();
        message.Destroy();
        return (ResponseType)result;
    }
    public static string[] OpenFileDialog(this Window parent,string title,IEnumerable<FileFilter> filters,bool multi = false)
    {
        FileChooserDialog dialog = new(title,parent,FileChooserAction.Open,"Cancel",ResponseType.Cancel,"Accept",ResponseType.Accept)
        {
            SelectMultiple = multi
        };
        foreach (FileFilter filter in filters)
            dialog.AddFilter(filter);
        int result = dialog.Run();
        string[] filenames = [];
        if(result == (int)ResponseType.Accept)
            filenames = dialog.Filenames;
        dialog.Destroy();
        return filenames;
    }
    public static string? SaveFileDialog(this Window parent,string title,IEnumerable<FileFilter> filters)
    {
        FileChooserDialog dialog = new(title,parent,FileChooserAction.Save,"Cancel",ResponseType.Cancel,"Accept",ResponseType.Accept);
        foreach(FileFilter filter in filters)
            dialog.AddFilter(filter);
        int result = dialog.Run();
        string? filename = null;
        if(result == (int)ResponseType.Accept)
            filename = dialog.Filename;
        dialog.Destroy();
        return filename;
    }
    public static string[] OpenFolderDialog(this Window parent,string title,bool multi = false)
    {
        FileChooserDialog dialog = new(title,parent,FileChooserAction.SelectFolder,"Cancel",ResponseType.Cancel,"Accept",ResponseType.Accept)
        {
            SelectMultiple = multi
        };
        int result = dialog.Run();
        string[] filenames = [];
        if(result == (int)ResponseType.Accept)
            filenames = dialog.Filenames;
        dialog.Destroy();
        return filenames;
    }
    public static AboutDialog CreateAboutDialog()
    {
        AboutDialog about = new()
        {
            Icon = Utils.VelvetIcon,
            Version = Dotnet.LibraryVersion.ToString(),
            ProgramName = Velvet.Velvetify(Velvet.NAME),
            Authors = [Velvet.AUTHOR],
            License = Velvet.Velvetify(Utils.License),
            Website = GitHub.GitHub.REPO_URL,
            WebsiteLabel = Velvet.Velvetify("Source Code"),
            Logo = Utils.VelvetImage,
            Comments = Velvet.Velvetify(Velvet.DESCRIPTION)
        };
        about.Title = Velvet.Velvetify(about.Title);
        return about;
    }
}