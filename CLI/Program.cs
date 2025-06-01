using ThemModdingHerds.VelvetBeautifier;
using ThemModdingHerds.VelvetBeautifier.CLI.GUI;
using ThemModdingHerds.VelvetBeautifier.Utilities;

CommandLine.AddHandler(new GuiCommand());

ModLoaderTool.Init();
CommandLine.Process();