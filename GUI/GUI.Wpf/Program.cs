using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI;
namespace GUI.Wpf
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			new Application(Eto.Platforms.Wpf).Run(new MainForm());
		}
	}
}
