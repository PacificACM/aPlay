// 
//  LibraryViewer.cs
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
using aPlay.Lib;
using Gtk;
namespace aPlay_GUI
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LibraryViewer : Gtk.Bin
	{
		ListStore store;
		public LibraryViewer ()
		{
			this.Build ();
		
			Init();
		}
		
		void Init()
		{
			TreeViewColumn title = new TreeViewColumn();
			title.Title = "Title";
			treeview2.AppendColumn(title);
			TreeViewColumn path = new TreeViewColumn();
			path.Title = "Path";
			treeview2.AppendColumn(path);
			
			ListStore list = new ListStore(typeof(string),typeof(string));
			treeview2.Model = list;
			
			CellRendererText titleRender = new CellRendererText();
			title.PackStart(titleRender,true);
			
			CellRendererText pathRender = new CellRendererText();
			
			path.PackStart(pathRender,true);
			
			title.AddAttribute(titleRender,"text",0);
			path.AddAttribute(pathRender,"text",1);
			
			store = list;
		}
		/// <summary>
		/// Sets the active reader for the viewer
		/// </summary>
		/// <param name="lib">
		/// A <see cref="ILibraryReader"/>
		/// </param>
		public void SetReader(ILibraryReader lib)
		{
			store.Clear();
			foreach(MediaItem item in lib)
			{
				store.AppendValues(item.Title,item.Path);
			}
			
		}
		protected virtual void OnTreeview2RowActivated (object o, Gtk.RowActivatedArgs args)
		{
		
			TreeIter iter;
			store.GetIter (out iter, args.Path);
			string path = store.GetValue(iter,1) as string;
			aPlay.Media.Player player = new aPlay.Media.Player(path);
			player.Play();
		}
		
		
	}
}

