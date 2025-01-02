using ThemModdingHerds.VelvetBeautifier.GameNews;
using ThemModdingHerds.VelvetBeautifier.GFS;
using ThemModdingHerds.VelvetBeautifier.Levels;
using ThemModdingHerds.VelvetBeautifier.Patches;
using ThemModdingHerds.VelvetBeautifier.TFHResource;
using ThemModdingHerds.VelvetBeautifier.Utilities;
using GameBananaMod = ThemModdingHerds.VelvetBeautifier.GameBanana.Mod;

namespace ThemModdingHerds.VelvetBeautifier.Modding;
public static class ModDB
{
    public const string FOLDERNAME = "mods";
    public static string Folder => Path.Combine(Velvet.AppDataFolder,FOLDERNAME);
    public static List<Mod> Mods {get => ReadFolder();}
    public static List<Mod> EnabledMods {get => [..Mods.Where((mod) => mod.Enabled)];}
    public static List<Mod> DisabledMods {get => [..Mods.Where((mod) => !mod.Enabled)];}
    public static void Init()
    {
        Velvet.Info($"creating mods folder at {Folder}...");
        Directory.CreateDirectory(Folder);
    }
    private static List<Mod> ReadFolder()
    {
        List<Mod> mods = [];

        if(Directory.Exists(Folder))
        foreach(string modfolder in Directory.GetDirectories(Folder))
        {
            if(Mod.IsMod(modfolder))
            {
                mods.Add(new(modfolder));
                continue;
            }
        }
        return mods;
    }
    public static ModInstallResult InstallMod(GameBanana.Argument argument)
    {
        return InstallGameBananaMod(argument.GetId());
    }
    public static ModInstallResult InstallMod(GameBananaMod mod)
    {
        Velvet.Info($"installing GameBanana Mod {mod.ModName}...");
        string update = mod.DownloadLatestUpdate();
        return InstallMod(update);
    }
    public static ModInstallResult InstallGameBananaMod(int id)
    {
        GameBananaMod? mod = GameBananaMod.Fetch(id);
        if(mod == null) return ModInstallResult.Failed;
        return InstallMod(mod);
    }
    public static ModInstallResult InstallMod(Mod mod)
    {
        ModInstallResult result = ModInstallResult.Ok;
        Velvet.Info($"installing mod '{mod.Info.Id}'...");
        if(ContainsMod(mod))
        {
            Velvet.Warn($"mod '{mod.Info.Id}' already exists, reinstalling...");
            UninstallMod(mod);
            result = ModInstallResult.AlreadyExists;
        }
        string folderpath = Path.Combine(Folder,mod.Info.Id);
        Directory.CreateDirectory(folderpath);
        FileSystem.CopyFolder(mod.Folder,folderpath);
        return result;
    }
    public static ModInstallResult InstallMod(Stream stream)
    {
        return InstallMod(ArchiveUtils.ExtractArchive(stream));
    }
    public static ModInstallResult InstallMod(string? str)
    {
        if(str == null) return ModInstallResult.Invalid;
        if(int.TryParse(str,out int id))
            return InstallGameBananaMod(id);
        if(GameBanana.Utils.ValidUrl(str))
        {
            Velvet.Info($"installing GameBanana 1-Click installer: {str}");
            return InstallMod(GameBanana.Argument.Parse(str));
        }
        if(Url.IsUrl(str))
        {
            Velvet.Info($"installing remote file at {str}...");
            string file = DownloadManager.GetTemp(str);
            ModInstallResult result = InstallMod(file);
            if(File.Exists(file)) File.Delete(file);
            return result;
        }
        if(Mod.IsMod(str))
        {
            Velvet.Info($"installing folder at {str}...");
            return InstallMod(new Mod(str));
        }
        if(File.Exists(str))
        {
            Velvet.Info($"installing file at {str}...");
            string? temp = ArchiveUtils.ExtractArchive(str);
            ModInstallResult result = InstallMod(temp);
            if(temp != null && File.Exists(temp))
                File.Delete(temp);
            return result;
        }
        return ModInstallResult.Invalid;
    }
    public static bool UninstallMod(string? id)
    {
        if(id == null) return false;
        Mod? mod = FindModById(id);
        if(mod == null) return false;
        UninstallMod(mod);
        return true;
    }
    public static void UninstallMod(Mod mod)
    {
        if(!ContainsMod(mod)) return;
        Velvet.Info($"uninstalling {mod.Info.Id}...");
        Directory.Delete(mod.Folder,true);
    }
    public static Mod? FindModByName(string name)
    {
        foreach(Mod mod in Mods)
            if(mod.Info.Name == name)
                return mod;
        return null;
    }
    public static Mod? FindModById(string? id)
    {
        if(id == null) return null;
        foreach(Mod mod in Mods)
            if(mod.Info.Id == id)
                return mod;
        return null;
    }
    public static bool ContainsMod(string id)
    {
        foreach(Mod mod in Mods)
            if(mod.Info.Id == id)
                return true;
        return false;
    }
    public static bool ContainsMod(Mod mod)
    {
        return ContainsMod(mod.Info.Id);
    }
    public static ModInstallResult Create(ModInfo info)
    {
        Mod mod = Mod.Create(info);
        return InstallMod(mod);
    }
    public static ModInstallResult Create(string? id)
    {
        if(id == null) return ModInstallResult.Invalid;
        return Create(new ModInfo(){Id = id});
    }
    public static bool HasRevergePackageMods()
    {
        foreach(Mod mod in EnabledMods)
            if(mod.HasRevergePackages)
                return true;
        return false;
    }
    public static bool HasTFHResourceMods()
    {
        foreach(Mod mod in EnabledMods)
            if(mod.HasTFHResources)
                return true;
        return false;
    }
    public static bool HasLevelPackMods()
    {
        foreach(Mod mod in EnabledMods)
            if(mod.HasLevels)
                return true;
        return false;
    }
    public static void Apply(Game? game)
    {
        if(game == null || !game.Valid()) return;
        Velvet.Info($"applying mods to {game.ExecutableName}...");
        RevergePackageManager.Apply(game);
        LevelManager.Apply(game);
        TFHResourceManager.Apply(game);
        GameNewsManager.Apply(game);
        PatchManager.Apply(game);   
    }
    public static void Apply()
    {
        Velvet.Info($"applying {EnabledMods.Count} mods...");
        Apply(ModLoaderTool.Client);
        Apply(ModLoaderTool.Server);
        Velvet.Info("mods have been applied!");
    }
    public static bool Exists() => Directory.Exists(Folder);
    public static void Clear()
    {
        if(Directory.Exists(Folder))
            Directory.Delete(Folder,true);
        Directory.CreateDirectory(Folder);
    }
}