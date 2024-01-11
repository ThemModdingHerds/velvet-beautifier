using System.Text.Json;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.Patches;
public static class PatchUtils
{
    public static List<Patch> ReadPatch(string path)
    {
        if(!File.Exists(path))
            throw new VelvetException("PatchUtils.ReadPatch",path + " does not exist");
        return JsonSerializer.Deserialize<List<Patch>>(File.ReadAllText(path)) ?? throw new VelvetException("PatchUtils.ReadPatch","Invalid patch file");
    }
    public static List<Patch> ReadPatches(string directory)
    {
        List<Patch> patches = [];
        if(!Directory.Exists(directory))
            throw new VelvetException("PatchUtils.ReadPatches",directory + " does not exist");
        List<string> patch_files = [..Directory.GetFiles(directory,"*.patch",SearchOption.AllDirectories)];
        foreach(string patch_file in patch_files)
            patches.AddRange(ReadPatch(patch_file));
        
        return patches;
    }
    public static List<IPatchSegment> GetSegments(string path)
    {
        static bool IsDigit(char letter) => '0' >= letter && '9' <= letter;
        List<IPatchSegment> segments = [];
        string text = string.Empty;
        if(path[0] != '/') throw new VelvetException("PatchUtils.GetSegments","path does not start with /");
        for(int index = 0;index < path.Length;index++)
        {
            char? lastLetter = path[index-1];
            char letter = path[index];
            char? nextLetter = path[index+1];
            if(letter == ']') throw new VelvetException("PatchUtils.GetSegments","] before [");
            if(letter == '[' && lastLetter != '\\')
            {
                if(lastLetter != '/') throw new VelvetException("PatchUtils.GetSegments","no / before [. Did you forgot to add a \\?");
                text = string.Empty;
                for(int jndex = index;jndex < path.Length;jndex++)
                {
                    char jetter = path[jndex];
                    if(jetter == ']') break;
                    if(jetter == '[') throw new VelvetException("PatchUtils.GetSegments","[ after [");
                    if(!IsDigit(jetter)) throw new VelvetException("PatchUtils.GetSegments","letter is not a digit");
                    text += jetter;
                }
                if(text == string.Empty) throw new VelvetException("PatchUtils.GetSegments","no number inside brackets");
                int textIndex = int.Parse(text);
                segments.Add(new PatchArrayIndexSegment(textIndex));
                text = string.Empty;
            }
            if(letter == '/')
            {
                if(text != string.Empty) segments.Add(new PatchPropertySegment(text));
                text = string.Empty;
                continue;
            }
            text += letter;
        }
        return segments;
    }
}