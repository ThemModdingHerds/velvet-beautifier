namespace ThemModdingHerds.VelvetBeautifier.CLI;
public static class CommandLine
{
    private static bool IsArg(string arg)
    {
        return arg.StartsWith("--");
    }
    public static Dictionary<string,string> Generate(string[] argv)
    {
        Dictionary<string,string> dict = [];
        for(long i = 0;i < argv.LongLength;i++)
        {
            string arg = argv[i];
            if(IsArg(arg))
            {
                string key = arg[2..];
                string value = "";
                long next = i + 1;
                if(next < argv.LongLength && !IsArg(argv[next]))
                {
                    value = argv[next];
                    i = next;
                }
                if(dict.ContainsKey("key"))
                {
                    dict[key] = value;
                    continue;
                }
                dict.Add(key,value);
            }
        }
        return dict;
    }
}