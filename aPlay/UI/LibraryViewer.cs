using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xcom.aPlay.Lib;
using Xcom.aPlay.Media;

namespace Xcom.aPlay.UI
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
