using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using Eto.Drawing;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public static class Utils
{
    // ThemModdingHerds.VelvetBeautifier.GUI.*
    public static Icon WindowIcon {get => Icon.FromResource("ThemModdingHerds.VelvetBeautifier.GUI.icon.ico");}
    public static string License {get => GetResourceString("ThemModdingHerds.VelvetBeautifier.GUI.LICENSE");}
    public static Bitmap VelvetImage {get => Bitmap.FromResource("ThemModdingHerds.VelvetBeautifier.GUI.velvet.png");}
    private static Stream GetResourceStream(string resource)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resource) ?? throw new System.Exception($"No Resource '{resource}' found");
    }
    private static string GetResourceString(string resource)
    {
        Stream stream = GetResourceStream(resource);
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes);
        return Encoding.Default.GetString(bytes);
    }
    public static FileFilter GFSFilter {get => new("Reverge Package File",[".gfs"]);}
    public static FileFilter TFHRESFilter {get => new("TFHResouce File",[".tfhres"]);}
    public static void SaveExceptionReport(this Control parent,Exception exception,string origin)
    {
        SaveFileDialog output = new()
        {
            Title = Velvet.Velvetify("Save Report file"),
            Filters = {new FileFilter("Text file",[".txt"])},
            CheckFileExists = true
        };
        if(output.ShowDialog(parent) == DialogResult.Ok)
        {
            string filepath = output.FileName;
            string content = string.Join('\n',[
                $"{Velvet.NAME} v{Dotnet.LibraryVersion}",
                "",
                $"caught exception at {origin}:",
                exception.ToString()
            ]);
            File.WriteAllText(filepath,content);
        }
    }
    public static void SaveExceptionReport(this Window parent,Exception exception,string origin)
    {
        SaveFileDialog output = new()
        {
            Title = Velvet.Velvetify("Save Report file"),
            Filters = {new FileFilter("Text file",[".txt"])},
            CheckFileExists = true
        };
        if(output.ShowDialog(parent) == DialogResult.Ok)
        {
            string filepath = output.FileName;
            string content = string.Join('\n',[
                $"{Velvet.NAME} v{Dotnet.LibraryVersion}",
                "",
                $"caught exception at {origin}:",
                exception.ToString()
            ]);
            File.WriteAllText(filepath,content);
        }
    }
}