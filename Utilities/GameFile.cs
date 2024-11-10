namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public class GameFile(string name,string sha256,bool canModify)
{
    public string Name {get;} = name;
    public string Checksum {get;} = sha256;
    public bool CanModify {get;} = canModify;
    public bool Verify(string file)
    {
        return Crypto.Checksum(file,Checksum);
    }
}