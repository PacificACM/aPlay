using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xcom.aPlay.Lib;
using System.Drawing.Drawing2D;

namespace Xcom.aPlay.UI
{
     partial class NavigationBar : UserControl
    {


         public class NavigationItemSelectedEventArgs : EventArgs
         {
         
             Category _c;
             ILibrary _lib;
             public ILibraryReader LibReader
             {
                 get
                 {
                     return _lib.GetItemsByCat(_c);
                 }
             }
             public NavigationItemSelectedEventArgs(ILibrary lib,Category c)
             {
                 _lib = lib;
                 _c = c;
             }
         }
         /// <summary>
         /// Gets raised when a navigation item gets selected.
         /// </summary>
         public event EventHandler<NavigationItemSelectedEventArgs> NavigationItemedSelected;

        ILibrary[] _libs = new ILibrary[0];

        

        public NavigationBar()
        {

            InitializeComponent();
        }
        public ILibrary[] Libraries
        {
            get
            {
                return _libs;
            }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { Libraries = value; }));
                }
                else
                {
                    _libs = value;
                    this.Update();
                }

            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Category)
            {
                ILibrary l = e.Node.Parent.Tag as ILibrary;
                if (NavigationItemedSelected != null)
                {
                    
                    NavigationItemedSelected(this, new NavigationItemSelectedEventArgs(l,e.Node.Tag as Category));
                }
               
            }
        }




        protected override void OnPaint(PaintEventArgs e)
        {
            this.BackColor = Color.FromArgb(190,200,175,200);
            //base.OnPaint(e);
    
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, Width, Height);

            Font f_libName = new Font("Arial", 12, FontStyle.Bold);
            Font f_catName = new Font("Arial", 12);
            int current_pos = 0;
            int libNameHeight = (int)f_libName.GetHeight(e.Graphics);
            int libCatNameHeight =(int) f_catName.GetHeight(e.Graphics);
            for (int i = 0; i < _libs.Length; i++)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(127,Color.PowderBlue)), 0, current_pos, this.Width, libNameHeight+1);
                e.Graphics.DrawString(_libs[i].Name, f_libName, new SolidBrush(Color.Navy), 2, current_pos + 1);
                current_pos += 1 + libNameHeight;
                for (int k = 0; k < _libs[i].Categories.Length; k++)
                {
                    //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(127,Color.White)), 0, current_pos, this.Width, libCatNameHeight+1);
                           GraphicsPath p = new GraphicsPath();
                    //p.AddString(_libs[i].Categories[k].Name, f_catName,(int) FontStyle.Regular
                    e.Graphics.DrawString(_libs[i].Categories[k].Name, f_catName, new SolidBrush(Color.Gray), 7, current_pos + 1);
                    current_pos += 1 + libCatNameHeight;
                }
                current_pos += libNameHeight / 2;
            }
        }

        private void NavigationBar_Load(object sender, EventArgs e)
        {

        }
    }
}
