using System.Text.RegularExpressions;
using ThemModdingHerds.VelvetBeautifier.Modding;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Patches;
public static class PatchManager
{
    public static void Revert(Game game)
    {
        foreach(Mod mod in ModDB.EnabledMods)
        foreach(Patch patch in mod.GetPatches())
        {
            if(!patch.ExistsTarget(game)) continue;
            string targetfilepath = patch.GetFullTargetPath(game);
            BackupManager.Revert(targetfilepath);
        }
    }
    public static void Apply(Game game)
    {
        Revert(game);
        foreach(Mod mod in ModDB.EnabledMods)
        foreach(Patch patch in mod.GetPatches())
        {
            if(!patch.ExistsTarget(game)) continue;
            string targetfilepath = patch.GetFullTargetPath(game);
            if(!BackupManager.ExistsBackup(targetfilepath))
                BackupManager.MakeBackup(targetfilepath);
            if(patch is TextPatch text)
                Apply(game,text);
            if(patch is BinaryPatch binary)
                Apply(game,binary,Path.Combine(mod.PatchesFolder,binary.FilePath));
        }
    }
    private static void Apply(Game game,TextPatch patch)
    {
        string targetfilepath = patch.GetFullTargetPath(game);
        string content = File.ReadAllText(targetfilepath);
        string patched = Regex.Replace(content,patch.Match,patch.Value);
        File.WriteAllText(targetfilepath,patched);
        Velvet.Info($"applied text patch to {patch.Target}");
    }
    private static void Apply(Game game,BinaryPatch patch,string inputfilepath)
    {
        if(!File.Exists(inputfilepath))
        {
            Velvet.Warn($"{inputfilepath} does not exist! Patch will not be applied for {patch.Target}");
            return;
        }
        string targetfilepath = patch.GetFullTargetPath(game);
        if(patch.IsFullReplace)
        {
            File.Copy(inputfilepath,targetfilepath,true);
            Velvet.Info($"replaced {patch.Target} fully with {inputfilepath}");
            return;
        }
        long offset = patch.Offset ?? throw new Exception("impossible");
        string temp = FileSystem.CreateTempFile();
        FileStream tempStream = File.OpenWrite(temp);

        FileStream targetStream = File.OpenRead(targetfilepath);
        targetStream.CopyTo(tempStream);
        targetStream.Close();

        tempStream.Seek(offset,SeekOrigin.Begin);
        byte[] input = File.ReadAllBytes(inputfilepath);
        tempStream.Write(input);
        tempStream.Close();

        File.Copy(temp,targetfilepath,true);
        Velvet.Info($"replaced {patch.Target}@0x{offset:X04} with {input.Length} bytes");
    }
}