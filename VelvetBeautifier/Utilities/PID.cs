using System.Diagnostics;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class PID
{
    public static void Save()
    {
        Process cur = Process.GetCurrentProcess();
        File.WriteAllText(Path.Combine(Environment.CurrentDirectory,".pid"),cur.Id.ToString());
    }
    public static bool Has()
    {
        return File.Exists(Path.Combine(Environment.CurrentDirectory,".pid"));
    }
    public static int Get()
    {
        if(!Has())
            throw new VelvetException("Utils.GetPID","Couldn't get pid");
        return int.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory,".pid")));
    }
}