using System;
using Gtk;
using aPlay;
using aPlay;
using aPlay.Lib;
using aPlay.FileIO;
using aPlay.Media;
using System.IO;
public partial class MainWindow : Gtk.Window
{
	[TreeNode(ListOnly = true)]
	class Test: TreeNode
	{
		MediaItem m;
		public Test(MediaItem m)
		{
			
			this.m = m;
			Name = m.Path;
		}
		
		[TreeNodeValue(Column=0)]
		public string Name{get; set;}
	}
	aPlayApp app;
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		app = new aPlayApp();
		app.Start();
	 	ILibraryReader r=	app.Libraries[0].GetItemsByCat(Category.Music);
		
		libraryviewer1.SetReader(r);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
	{
		MediaFinder f = new MediaFinder(app.Libraries[0], new DirectoryInfo[]{app.Settings.MainMusicFolderPath});
        f.ScanForNewFiles(false, null);
		ILibraryReader r=	app.Libraries[0].GetItemsByCat(Category.Music);
		
		libraryviewer1.SetReader(r);
		
	}	 	
	

	
	
	
}

