using System;
using Gtk;
using aPlay;
namespace aPlay_GUI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			
			MainWindow win = new MainWindow ();
			win.Show ();

			Application.Run ();
		}
	}
}

