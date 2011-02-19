using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xcom.aPlay.Lib;
using Xcom.aPlay.FileIO;
using System.IO;

namespace Xcom.aPlay.UI
{
    public partial class MainWindow : Xcom.aPlay.UI.BaseWindow
    {
        aPlayApp _app;
        public MainWindow()
        {

            InitializeComponent();
           
        }
        public MainWindow(aPlayApp app):this()
        {
            this._app = app;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.BorderMargins = new Margins(-1, 0, 0, 0);//navigationBar1.Width+3+navigationBar1.Location.X, navigationBar1.Location.Y+2, 0, 0);
            if (this._app != null)
            {
                this.navigationBar1.Libraries = this._app.Libraries;
                this.libraryViewer1.SetReader(this._app.Libraries[0].GetItemsByCat(Category.Music));
            }
           
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MediaFinder f = new MediaFinder(_app.Libraries[0], new DirectoryInfo[]{_app.Settings.MainMusicFolderPath});
            f.ScanForNewFiles(false, null);
        }
    }
}
