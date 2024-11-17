using System;
using Eto.Forms;
using ThemModdingHerds.VelvetBeautifier.GUI;
namespace GUI.Mac
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			new Application(Eto.Platforms.Mac64).Run(new MainForm());
		}
	}
}
