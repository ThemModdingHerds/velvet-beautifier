using System;
using Eto.Forms;
using Eto.Drawing;
using ThemModdingHerds.VelvetBeautifier.Utilities;
using System.Reflection;
using System.IO;
using ThemModdingHerds.VelvetBeautifier.GUI.Items;
using MenuBar = ThemModdingHerds.VelvetBeautifier.GUI.Items.MenuBar;
using ToolBar = ThemModdingHerds.VelvetBeautifier.GUI.Items.ToolBar;
using ThemModdingHerds.VelvetBeautifier.GUI.Commands.Help;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public partial class MainForm : Form
{
	public ModLoaderTool ModLoaderTool {get;}
	public ModListView ModList {get => (ModListView)Content;}
	public MainForm()
	{
		ModLoaderTool = new(Environment.GetCommandLineArgs());
		SetupResult setup = ModLoaderTool.Setup();
		if(setup != SetupResult.Success && setup != SetupResult.NotRequired)
		{
			switch(setup)
			{
				case SetupResult.OldConfig:
					VelvetEto.ShowMessageBox("Setup failed","Your config file is old! Delete it and let it create a new",MessageBoxType.Error);
					break;
				case SetupResult.BackupFail:
					VelvetEto.ShowMessageBox("Setup failed","Some of your games files have been tampered with! Redownload the game's asset to fix this problem",MessageBoxType.Error);
					break;
			}
			Environment.Exit(1);
		}
		ModLoaderTool.CommandLine.Process();
		Title = Velvet.Velvetify(Velvet.NAME);
		ClientSize = new(600,400);
		Icon = Utils.WindowIcon;
		Menu = new MenuBar(this);
		ToolBar = new ToolBar(this);
		Content = new ModListView(this);
	}
}
