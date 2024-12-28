using System;
using Eto.Forms;
using Eto.Drawing;
using ThemModdingHerds.VelvetBeautifier.Utilities;
using ThemModdingHerds.VelvetBeautifier.GUI.Items;
using MenuBar = ThemModdingHerds.VelvetBeautifier.GUI.Items.MenuBar;
using ToolBar = ThemModdingHerds.VelvetBeautifier.GUI.Items.ToolBar;
using System.Threading;

namespace ThemModdingHerds.VelvetBeautifier.GUI;
public partial class MainForm : Form
{
	public ModListView ModList {get => (ModListView)Content;}
	public MainForm()
	{
		Title = Velvet.Velvetify(Velvet.NAME);
		ClientSize = new(600,400);
		Icon = Utils.WindowIcon;
		Menu = new MenuBar(this);
		ToolBar = new ToolBar(this);
		Content = new ModListView(this);
		Load += OnLoadWindow;
	}
    protected void OnLoadWindow(object? sender,EventArgs e)
    {
		JoinThread(ModLoaderTool.Run);
		ModList.RefreshModList();
    }
	public void JoinThread(ThreadStart start)
	{
		Enabled = false;
		Utils.JoinThread(start);
		Enabled = true;
	}
	public void JoinThread(ParameterizedThreadStart start)
	{
		Enabled = false;
		Utils.JoinThread(start);
		Enabled = true;
	}
}
