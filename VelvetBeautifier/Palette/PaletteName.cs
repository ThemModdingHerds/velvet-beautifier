// Move this to a seperate library in the future
namespace ThemModdingHerds.VelvetBeautifier.Palette;
public class PaletteName(string name,string backer,string file)
{
    public string Name {get;set;} = name;
    public string Backer {get;set;} = backer;
    public string FileName {get;set;} = file;
    public override string ToString()
    {
        return $"{{name = \"{Name}\", backer = \"{Backer}\", file = \"{FileName}\"}}";
    }
    public static void Write(string file,IEnumerable<PaletteName> paletteNames)
    {
        int count = 1;
        string content = string.Join('\n',[
            "palette_table =",
            "{",
            ..(from palette in paletteNames select $"    [{count++}] = {palette}"),
            "}",
            "palette_names = {}",
            "backer_names = {}",
            "palette_files = {}",
            "for i, v in pairs(palette_table) do",
            "    palette_names[#palette_names + 1] = v.name",
            "    backer_names[#backer_names + 1] = v.backer",
            "    palette_files[#palette_files + 1] = v.file",
            "end"
        ]);
        File.WriteAllText(file,content);
    }
}