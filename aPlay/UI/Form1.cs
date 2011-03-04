using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using aPlay.Lib;
using System.IO;
using aPlay.FileIO;
using aPlay.UI;

namespace aPlay
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ILibrary prim;
        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] b = new byte[8];
            b[0] = 25;
            

            //prim = new aPlay.Lib.Library("lib.db");
            //navigationBar1.Libraries = new ILibrary[] { prim };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MediaFinder m = new MediaFinder(this.prim, new DirectoryInfo[] { new DirectoryInfo(@"D:\Cameron\My Music\") });
            m.ScanForNewFiles(false, new MediaFinder.ScanProgress(p));
            MessageBox.Show("Done");
        }
        void p(float f, string s)
        {
            if (f >= 0)
                progressBar1.Value = (int)f;
            label1.Text = s;
        }
        private void navigationBar1_NavigationItemedSelected(object sender, aPlay.UI.NavigationBar.NavigationItemSelectedEventArgs e)
        {
            //libraryViewer1.SetReader(e.LibReader);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new MainWindow().ShowDialog();
        }

 
    }
}
