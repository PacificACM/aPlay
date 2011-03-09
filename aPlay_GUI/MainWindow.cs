// 
//  MainWindow.cs
//  
//  Author:
//       Cameron Lucas <c_lucas3@u.pacific.edu>
// 
//  Copyright (c) 2011 Cameron
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
using System;
using Gtk;
using aPlay;
using aPlay.Lib;
using aPlay.FileIO;
using aPlay.Media;
using System.IO;
public partial class MainWindow : Gtk.Window
{
	aPlayApp _app;
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
        //Spawn aPlay App instance
		_app = new aPlayApp();
		_app.Start();
        
	 	ILibraryReader r = _app.Libraries[0].GetItemsByCat(Category.Music);
		
		libraryviewer1.SetReader(r);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
	{
		MediaFinder f = new MediaFinder(_app.Libraries[0], new DirectoryInfo[]{_app.Settings.MainMusicFolderPath});
        f.ScanForNewFiles(false, null);
		ILibraryReader r=	_app.Libraries[0].GetItemsByCat(Category.Music);
		
		libraryviewer1.SetReader(r);
		
	}	 	
	

	
	
	
}

