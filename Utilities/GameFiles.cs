namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class GameFiles
{
    public static List<string> TFHResources {get;} = [
        "arizonaopening_prod.tfhres",
        "canyon_prod.tfhres",
        "highlands_prod.tfhres",
        "inttests_prod.tfhres",
        "lobbymaps_prod.tfhres",
        "prologue_prod.tfhres",
        "reine_prod.tfhres",
        "resources_prod.tfhres",
        "testroomd_prod.tfhres"
    ];
    public static List<string> Data01 {get;} = [
        "ai-libs.gfs",
        "characters-art.gfs",
        "characters-art-pt.gfs",
        "characters-foits.gfs",
        "characters-win.gfs",
        "dev.gfs",
        "levels.gfs",
        "sprites.gfs",
        "trials.gfs",
        "ui.gfs",
        "ui-win.gfs"
    ];
    public static List<string> All {get;} = [..TFHResources,..Data01];
}