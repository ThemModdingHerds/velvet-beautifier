using System;
using Eto.Forms;

using ThemModdingHerds.VelvetBeautifier.GUI;

namespace GUI.Gtk
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			new Application(Eto.Platforms.Gtk).Run(new MainForm());
		}
	}
}
