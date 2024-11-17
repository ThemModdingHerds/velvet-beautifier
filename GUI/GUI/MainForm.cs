using System;
using Eto.Forms;
using Eto.Drawing;
using ThemModdingHerds.VelvetBeautifier.Utilities;
using System.Reflection;
using System.IO;
using ThemModdingHerds.VelvetBeautifier.GUI.Items;
using MenuBar = ThemModdingHerds.VelvetBeautifier.GUI.Items.MenuBar;
using ToolBar = ThemModdingHerds.VelvetBeautifier.GUI.Items.ToolBar;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public partial class MainForm : Form
{
	public ModLoaderTool ModLoaderTool {get;}
	public ModListView ModList {get;}
	public ModView ModView {get;}
	public MainForm()
	{
		ModLoaderTool = new(Environment.GetCommandLineArgs());
		SetupResult setup = ModLoaderTool.Setup();
		if(setup != SetupResult.Success && setup != SetupResult.NotRequired)
		{
			VelvetEto.ShowMessageBox("Setup failed",$"Reason: {setup}",MessageBoxType.Error);
			Environment.Exit(1);
		}
		ModLoaderTool.CommandLine.Process();
		ModList = new ModListView(this);
		ModView = new ModView(this);
		Title = Velvet.Velvetify(Velvet.NAME);
		ClientSize = new(600,400);
		Icon = Utils.WindowIcon;
		Menu = new MenuBar(this);
		ToolBar = new ToolBar(this);
		TableLayout layout = new()
		{
			Rows = {
				new TableRow([ModList,ModView])
			}
		};
	Content = layout;
	}
}
