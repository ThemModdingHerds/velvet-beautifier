using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public class Application
{
    public static Application Instance {get;} = new();
    public Config Config {get;} = Config.Read();
    public ModDB ModDB {get;} = new();
    public BackupManager BackupManager {get;} = new();
    public Game Client {get;}
    public Dictionary<string,string> ClientChecksums {get;}
    public Game Server {get;}
    public Dictionary<string,string> ServerChecksums {get;}
    private Application()
    {
        Client = new Game(Config.ClientPath,Game.CLIENT_NAME);
        Server = new Game(Config.ServerPath,Game.SERVER_NAME);

        ClientChecksums = Client.CreateChecksums();
        ServerChecksums = Server.CreateChecksums();
    }
    public void ApplyTFHRES(string path)
    {
        // 1. make backup
        string backup = BackupManager.MakeBackup(path);
        // 2. copy backup to temp file
        string temp = IO.CreateTempFile(backup);
        // 3. add items to temp file
        TFHResourceFile db = new(temp);
        // 4. replace game file with temp file
        File.Copy(temp,path,true);
    }
}