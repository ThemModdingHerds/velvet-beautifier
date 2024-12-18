using ThemModdingHerds.VelvetBeautifier.Utilities;
using GameBananaMod = ThemModdingHerds.VelvetBeautifier.GameBanana.Mod;

namespace ThemModdingHerds.VelvetBeautifier.Modding;
public static class ModDB
{
    public const string FOLDERNAME = "mods";
    public static string Folder => Path.Combine(Dotnet.ExecutableFolder,FOLDERNAME);
    public static List<Mod> Mods {get => ReadFolder();}
    public static List<Mod> EnabledMods {get => [..Mods.Where((mod) => mod.Enabled)];}
    public static List<Mod> DisabledMods {get => [..Mods.Where((mod) => !mod.Enabled)];}
    private static List<Mod> ReadFolder()
    {
        List<Mod> mods = [];

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
        Task<string> task = mod.DownloadLatestUpdate();
        task.Wait();
        return InstallMod(task.Result);
    }
    public static ModInstallResult InstallGameBananaMod(int id)
    {
        Task<GameBananaMod?> task = GameBananaMod.Fetch(id);
        task.Wait();
        GameBananaMod? mod = task.Result;
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
        string path = Path.Combine(Folder,mod.Info.Id);
        Directory.CreateDirectory(path);
        FileSystem.CopyFolder(mod.Folder,path);
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
            Task<string> task = DownloadManager.GetTemp(str);
            task.Wait();
            return InstallMod(task.Result);
        }
        if(Mod.IsMod(str))
        {
            Velvet.Info($"installing folder at {str}...");
            return InstallMod(new Mod(str));
        }
        if(File.Exists(str))
        {
            Velvet.Info($"installing file at {str}...");
            return InstallMod(ArchiveUtils.ExtractArchive(str));
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
        Velvet.Warn($"no mod with name '{name}' exists!");
        return null;
    }
    public static Mod? FindModById(string? id)
    {
        if(id == null) return null;
        foreach(Mod mod in Mods)
            if(mod.Info.Id == id)
                return mod;
        Velvet.Warn($"no mod with id '{id}' exists!");
        return null;
    }
    public static bool ContainsMod(string id)
    {
        foreach(Mod mod in Mods)
            if(mod.Info.Id == id)
                return true;
        Velvet.Warn($"no mod with id '{id}' exists!");
        return false;
    }
    public static bool ContainsMod(Mod mod)
    {
        return ContainsMod(mod.Info.Id);
    }
    public static void Clear()
    {
        if(Directory.Exists(Folder))
            Directory.Delete(Folder,true);
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
}