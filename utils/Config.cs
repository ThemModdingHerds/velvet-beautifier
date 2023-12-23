using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier;
public class Config
{
    public static readonly string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory,"config.json");
    public static Config Current {get;} = Read();
    public string TfhPath {get; set;} = Utils.GetDefaultTFHPath();
    public string ModsFolder {get; set;} = Path.Combine(Environment.CurrentDirectory,"mods");
    public string BackupFolder {get; set;} = Path.Combine(Environment.CurrentDirectory,"backup");
    public static Config Read(string path)
    {
        if(!Exists(path))
        {
            Velvet.ConsoleWriteLine("Config does not exist, creating one...");
            Create(path);
        }
        Config? config = JsonSerializer.Deserialize<Config>(File.ReadAllText(path)) ?? throw new Exception("couldn't read config file");
        Velvet.ConsoleWriteLine("Read Config at " + path);
        return config;
    }
    public static Config Read()
    {
        return Read(CONFIG_PATH);
    }
    public static bool Exists(string path)
    {
        return File.Exists(path);
    }
    public static bool Exists()
    {
        return Exists(CONFIG_PATH);
    }
    public static void Create(string path)
    {
        Write(path,new());
    }
    public static void Create()
    {
        Create(CONFIG_PATH);
    }
    public static void Write(string path,Config config)
    {
        Velvet.ConsoleWriteLine("Writing config to " + path);
        StreamWriter file = File.CreateText(path);
        string json = JsonSerializer.Serialize(config);
        file.Write(json);
        file.Close();
    }
    public static void Write(Config config)
    {
        Write(CONFIG_PATH,config);
    }
    public void Save(string path)
    {
        Write(path,this);
    }
    public void Save()
    {
        Write(this);
    }
    public bool ExistsTFHFolder()
    {
        return Directory.Exists(TfhPath);
    }
    public bool ExistsModsFolder()
    {
        return Directory.Exists(ModsFolder);
    }
    public bool ExistsBackupFolder()
    {
        return Directory.Exists(BackupFolder);
    }
    public string GetTFHResourcesFolder()
    {
        return Path.Combine(TfhPath,"Scripts","src","Farm","resources");
    }
    public string GetTFHResourcesFolder(string path)
    {
        return Path.Combine(GetTFHResourcesFolder(),path);
    }
    public string GetData01Folder()
    {
        return Path.Combine(TfhPath,"data01");
    }
    public string GetData01Folder(string path)
    {
        return Path.Combine(GetData01Folder(),path);
    }
}