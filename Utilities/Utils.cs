using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Utils
{
    public static void SavePID()
    {
        Process cur = Process.GetCurrentProcess();
        File.WriteAllText(Path.Combine(Environment.CurrentDirectory,".pid"),cur.Id.ToString());
    }
    public static bool HasPID()
    {
        return File.Exists(Path.Combine(Environment.CurrentDirectory,".pid"));
    }
}