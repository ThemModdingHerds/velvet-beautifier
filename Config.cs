using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier;
public class Config
{
    public static readonly string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory,"config.json");
    public string TfhPath {get; set;} = Utils.GetDefaultTFHPath();
    public string ModsFolder {get; set;} = Path.Combine(Environment.CurrentDirectory,"mods");
    public static Config Read(string path)
    {
        if(!Exists(path))
            Create(path);
        Config? config = JsonSerializer.Deserialize<Config>(File.ReadAllText(path)) ?? throw new Exception("couldn't read config file");
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
}