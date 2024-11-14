using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;
using ThemModdingHerds.TFHResource;
using ThemModdingHerds.TFHResource.Data;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier;
public class Application
{
    public Config Config {get;} = Config.Read();
    public ModDB ModDB {get;} = new();
    public BackupManager BackupManager {get;} = new();
    public Game Client {get;}
    public Game Server {get;}
    public Application()
    {
        Client = new Game(Config.ClientPath,Game.CLIENT_NAME);
        Server = new Game(Config.ServerPath,Game.SERVER_NAME);
    }
    public void BackupGameFiles()
    {
        if(Client.Valid())
            BackupGameFiles(Client);
        if(Server.Valid())
            BackupGameFiles(Server);
    }
    private void BackupGameFiles(Game game)
    {
        if(Directory.Exists(BackupManager.Folder)) return;
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(GameFile gameFile in GameFiles.Data01)
            {
                string file = Path.Combine(folder,gameFile.Name);
                if(gameFile.Verify(file))
                    BackupManager.MakeBackup(file);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(GameFile gameFile in GameFiles.TFHResources)
            {
                string file = Path.Combine(folder,gameFile.Name);
                if(gameFile.Verify(file))
                    BackupManager.MakeBackup(file);
            }
        }
    }
    public void ApplyMods()
    {
        ModDB.Refresh();
        Velvet.Info($"applying {ModDB.EnabledMods.Count} mods...");
        if(Client.Valid())
        {
            Velvet.Info("applying mods to client...");
            ApplyMods(Client);
        }
        if(Server.Valid())
        {
            Velvet.Info("applying mods to server...");
            ApplyMods(Server);
        }
    }
    public Dictionary<string,bool> VerifyFiles()
    {
        Dictionary<string,bool> dict = [];
        if(Client.Valid())
        {
            Velvet.Info("verifying files in client...");
            foreach(var pair in VerifyFiles(Client))
                dict.Add(pair.Key,pair.Value);
        }
        if(Server.Valid())
        {
            Velvet.Info("verifying files in server...");
            foreach(var pair in VerifyFiles(Server))
                dict.Add(pair.Key,pair.Value);
        }
        return dict;
    }
    public void Revert()
    {
        Velvet.Info("reverting files back to vanilla game...");
        if(Client.Valid())
        {
            Velvet.Info("reverting files from client...");
            Revert(Client);
        }
        if(Server.Valid())
        {
            Velvet.Info("reverting files from server...");
            Revert(Server);
        }
    }
    private void Revert(Game game)
    {
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(GameFile gameFile in GameFiles.Data01)
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(GameFile gameFile in GameFiles.TFHResources)
            {
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
            }
        }
    }
    private Dictionary<string,bool> VerifyFiles(Game game)
    {
        Dictionary<string,bool> dict = [];
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(GameFile gameFile in GameFiles.Data01)
            {
                string file = Path.Combine(folder,gameFile.Name);
                string checksum = Crypto.Checksum(file);
                string secure = gameFile.CanModify ? "Secured" : "Not Secured";
                string result = checksum == gameFile.Checksum ? "same file" : $"not the same file, got {checksum}";
                Velvet.Info($"{gameFile.Name} [{secure}] [{gameFile.Checksum}] = {result}");
                dict.Add(gameFile.Name,checksum == gameFile.Checksum);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(GameFile gameFile in GameFiles.TFHResources)
            {
                string file = Path.Combine(folder,gameFile.Name);
                string checksum = Crypto.Checksum(file);
                string secure = gameFile.CanModify ? "Secured" : "Not Secured";
                string result = checksum == gameFile.Checksum ? "same file" : $"not the same file, got {checksum}";
                Velvet.Info($"{gameFile.Name} [{secure}] [{gameFile.Checksum}] = {result}");
                dict.Add(gameFile.Name,checksum == gameFile.Checksum);
            }
        }
        return dict;
    }
    private void ApplyMods(Game game)
    {
        if(game.ExistsData01Folder())
        {
            string folder = game.GetData01Folder();
            foreach(GameFile gameFile in GameFiles.Data01)
            {
                if(!gameFile.CanModify)
                    Velvet.Error($"WARN: {gameFile.Name} cannot be modified! Expect the game not lanuching!");
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
                Reader reader = new(file);
                RevergePackage target = reader.ReadRevergePackage();
                reader.Close();
                int modCount = 0;
                RevergePackage modded = new(target.Header,target,target.Metadata);
                foreach(Mod mod in ModDB.EnabledMods)
                {
                    RevergePackage? gfsmod = mod.GetRevergePackage(gameFile.Name);
                    if(gfsmod == null) continue;
                    modded.Metadata.Add(gfsmod.Metadata);
                    foreach(var pair in gfsmod)
                        modded.Add(pair.Value);
                    modCount++;
                }
                if(modCount == 0) continue;
                Velvet.Info($"applying {modCount} to {gameFile.Name}");
                File.Delete(file);
                Writer writer = new(file);
                writer.Write(modded);
            }
        }
        if(game.ExistsTFHResourcesFolder())
        {
            string folder = game.GetTFHResourcesFolder();
            foreach(GameFile gameFile in GameFiles.TFHResources)
            {
                if(!gameFile.CanModify)
                    Velvet.Error($"WARN: {gameFile.Name} cannot be modified! Expect the game not lanuching!");
                string file = Path.Combine(folder,gameFile.Name);
                BackupManager.Revert(file);
                Database target = Database.Open(file);
                Database modded = target.Clone();
                List<Database> mods = [];
                foreach(Mod mod in ModDB.EnabledMods)
                {
                    Database? db = mod.GetTFHResource(gameFile.Name);
                    if(db == null) continue;
                    mods.Add(db);
                }
                if(mods.Count == 0) continue;
                Velvet.Info($"applying {mods.Count} to {gameFile.Name}");
                modded.Upsert([..mods]);
                foreach(Database db in mods)
                    db.Close();
                target.Close();
                modded.Save(file,true);
                modded.Close();
            }
        }
    }
    private void InstallFolder(string? modFolder) => ModDB.InstallMod(modFolder);
    public async Task InstallGameBananaMod(int id)
    {
        GameBanana.Mod? mod = await GameBanana.Mod.Fetch(id);
        if(mod == null) return;
        string modFolder = await mod.DownloadLatestUpdate();
        InstallFolder(modFolder);
    }
    public async Task InstallGameBananaMod(GameBanana.Argument argument) => await InstallGameBananaMod(argument.Id);
    public async Task InstallMod(string? str)
    {
        if(str == null) return;
        if(int.TryParse(str,out int id))
        {
            await InstallGameBananaMod(id);
            return;
        }
        if(Url.IsUrl(str))
        {
            InstallFolder(await DownloadManager.GetAndUnzip(str));
            return;
        }
        string path = Path.Combine(Environment.CurrentDirectory,str);
        if(Directory.Exists(path))
        {
            InstallFolder(path);
            return;
        }
        if(File.Exists(path))
        {
            InstallFolder(FileSystem.ExtractZip(path));
            return;
        }
    }
    public void UninstallMod(string? id)
    {
        if(id == null) return;
        ModDB.UninstallMod(id);
        return;
    }
    public void CreateMod(string? id)
    {
        if(id == null) return;
        ModInfo info = new()
        {
            Id = id
        };
        string path = Path.Combine(ModDB.Folder,id);
        Directory.CreateDirectory(path);
        info.Write(Path.Combine(path,Mod.MODINFO_NAME));
    }
    public static void CreateGFS(string? input,string? output)
    {
        if(input == null || output == null) return;
        string inputPath = Path.Combine(Environment.CurrentDirectory,input);
        string outputPath = Path.Combine(Environment.CurrentDirectory,output.EndsWith(".gfs") ? output : $"{output}.gfs");
        RevergePackage gfs = RevergePackage.Create(inputPath);
        Writer writer = new(outputPath);
        writer.Write(gfs);
    }
    public static void CreateTFHRES(string? output)
    {
        if(output == null) return;
        string outputPath = Path.Combine(Environment.CurrentDirectory,output.EndsWith(".tfhres") ? output : $"{output}.tfhres");
        Database database = new();
        database.Save(outputPath);
    }
    public static void Extract(string? input,string? output)
    {
        if(input == null || output == null) return;
        string inputPath = Path.Combine(Environment.CurrentDirectory,input);
        string outputPath = Path.Combine(Environment.CurrentDirectory,output);
        if(inputPath.EndsWith(".gfs"))
        {
            GFS.Utils.Extract(inputPath,outputPath);
            return;
        }
        if(inputPath.EndsWith(".tfhres"))
        {
            TFHResource.Utils.Extract(inputPath,outputPath);
            return;
        }
    }
    public void EnableMod(string? id)
    {
        if(id == null) return;
        ModDB.Refresh();
        Mod? mod = ModDB.FindModById(id);
        mod?.Enable();
    }
    public void DisableMod(string? id)
    {
        if(id == null) return;
        ModDB.Refresh();
        Mod? mod = ModDB.FindModById(id);
        mod?.Disable();
    }
}