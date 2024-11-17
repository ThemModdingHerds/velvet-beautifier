namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class GameFiles
{
    public static List<GameFile> TFHResources {get;} = [
        new("arizonaopening_prod.tfhres","47004ceca2bcb7e59026d9b12bbc5fb08a5f061e80d1c77964536f79348c6622",true),
        new("canyon_prod.tfhres","033e6c76e15d2581f6978af880c9f0ff554aff4c55fce37ded5d2eaacfbf9ec1",true),
        new("highlands_prod.tfhres","2564182622234220fd972c7dec26e204c753f0a7d4db31d00ae7211c2ace6b7b",true),
        new("inttests_prod.tfhres","6eae4f93f12e207588177186bd7b0d5d2be1658f9581f3c67a8ec55491533fde",true),
        new("lobbymaps_prod.tfhres","164a591ddda761b275c626946299f3982f5ba2795505df46ddfe49f94da79eed",true),
        new("prologue_prod.tfhres","278cfcec117de2ff7d29b0faef6305983aa2a476633687593435ad0faecd0b92",true),
        new("reine_prod.tfhres","d34bb2766e8a8f0e21d2faafd5037e8603540874e3402304f3f160b7f22b65e5",true),
        new("resources_prod.tfhres","be40aa608b857449bd2767b279dccc7a561db2f9875544da7dcbcf867feb8bfb",true),
        new("testroomd_prod.tfhres","02c367d74d5dbd7bf04e647185940aef429ae4c82fd1f2525c29f7aa03a4dac2",true)
    ];
    public static GameFile Executable {get;} = new(Game.CLIENT_NAME,"03e20eece23f39fb1012061a5a7ff92f4b63da23eb8eb641cf6ee379cbce373d",false);
    public static List<GameFile> Data01 {get;} = [
        new("ai-libs.gfs","ec041023c6c6747bbcb9b390f09f9d42ce05d9f8e9a45749d594349053535223",true),
        new("characters-art.gfs","1d9e09ba88c402a7af7f419da31ec26eb52e6ac335223852ae00c098f543684f",true),
        new("characters-art-pt.gfs","83319a9236e15c83bc27cb40cfc6341a0e53143583f973362b3c19187c2626dd",true),
        new("characters-foits.gfs","5c57d152125922c6d47e5e8f6680d88f3e8b65780f0ec574d78e812741c5cd0c",true),
        new("characters-win.gfs","4719ab3ae6f70a7299479d8926658f3df3e2d935617f5284dd79b6e162325191",true),
        new("dev.gfs","3540e6fb79c88fc85f7a660583c9e2da97e27d8360dc1ddc6c067b444c930155",true),
        new("levels.gfs","a35ee62c3173000595028e4510c9d593defb6a88bc5c1d86fca607e2e7045d24",true),
        new("nidra-baihe-texas-art-pt.gfs","2092c8436bd9952647f728966fbc85fdc4652bb832517d0bb9757a0c917810aa",true),
        new("sprites.gfs","033a1343062b5b068c59c6db3dd981e35e1c142f66671954c5482a6859207fd6",true),
        new("trials.gfs","8e4a65f65ae2812c1a8bfd409f58827012b049935127170eb81b1d5913cec3f6",true),
        new("ui.gfs","c74ef5975b5a309829ec704d135505ee72f4546f82cefa6f72cdac5f2c55a5fd",true),
        new("ui-win.gfs","ee8a29fe4a7c190a18e71824ac6e0dfb3158910c925803d7ec9a412d60dc7d49",true)
    ];
    public static List<GameFile> SkullGirlsData01 {get;} = [];
    public static GameFile SkullGirlsExecutable {get;} = new(Game.SKULLGIRLS_NAME,"fb757433d2d11602ded18040078e70319afdae98c35fc22d789b38d012ad400b",false);
}