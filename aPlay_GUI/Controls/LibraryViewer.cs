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
using System.Collections.Generic;
namespace aPlay_GUI
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LibraryViewer : Gtk.Bin
	{
        Category _category;
		ListStore store;
		public LibraryViewer ()
		{
			this.Build ();
            _category = Category.Music;
			Init();
		}
		
		void Init()
		{


            TreeViewColumn col = null;
            Type[] types = new Type[_category.Fields.Length];
            for(int i = 0; i < types.Length;i++)
            {
                 col = new TreeViewColumn();
                col.Title = _category.Fields[i].DisplayName;
                col.SortIndicator = true;
                col.SortColumnId = i;
                treeview2.AppendColumn(col);
                types[i] = _category.Fields[i].DataType;
            }
            
			ListStore list = new ListStore(types);
			treeview2.Model = list;
           
            for (int i = 0; i <treeview2.Columns.Length; i++)
            {
                col = treeview2.Columns[i];

                CellRendererText render = new CellRendererText();
                col.PackStart(render, true);
                col.AddAttribute(render, "text", i);
            }

			
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
                List<object> values = new List<object>();
                for (int i = 0; i < _category.Fields.Length; i++)
                {
                    values.Add(item[_category.Fields[i]]);
                }
				store.AppendValues(values.ToArray());
			}
			
		}
		protected virtual void OnTreeview2RowActivated (object o, Gtk.RowActivatedArgs args)
		{
		
			TreeIter iter;
			store.GetIter (out iter, args.Path);
            string path = "";
            for(int i = 0; i < treeview2.Columns.Length;i++)
            {
                if(treeview2.Columns[i].Title == "Path")
                {
                    path = store.GetValue(iter,i) as string;
                    break;
                }

            }
		
			aPlay.Media.Player player = new aPlay.Media.Player(path);
			player.Play();
		}
		
		
	}
}

