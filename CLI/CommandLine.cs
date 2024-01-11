namespace ThemModdingHerds.VelvetBeautifier;
public class CommandLine(string[] argv)
{
    public List<string> Argv {get;} = [..argv];
    public bool Has(string arg)
    {
        return Argv.Contains("--" + arg) || Argv.Contains('-' + arg[0].ToString());
    }
    public string? Get(string arg)
    {
        if(!Has(arg)) return null;
        return Argv[Argv.IndexOf(arg)+1];
    }
}