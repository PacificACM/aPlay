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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using aPlay.Lib;
using aPlay.Media;

namespace aPlay.UI
{
   partial class LibraryViewer : UserControl
    {
        public LibraryViewer()
        {
            InitializeComponent();
        }

        public void SetReader(ILibraryReader reader)
        {
            this.listBox1.Items.Clear();

            try
            {
                while (reader.MoveNext())
                {
                    listBox1.Items.Add(reader.Current.Path);
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Player p = new Player(listBox1.SelectedItem.ToString());
                p.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\r\n"+Application.StartupPath);
            }
        }
    }
}
