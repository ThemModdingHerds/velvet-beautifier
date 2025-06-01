using ThemModdingHerds.VelvetBeautifier;
using ThemModdingHerds.VelvetBeautifier.Tool.CLI;
using ThemModdingHerds.VelvetBeautifier.Utilities;

CommandLine.AddHandler(new GuiCommand());

ModLoaderTool.Init();
CommandLine.Process();