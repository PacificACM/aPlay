using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace aPlay.UI
{
   
    public partial class BaseWindow : Form
    {
        
        

        public class Margins
        {
            public int Left, Right,
                       Top, Bottom;

            public Margins(int left, int top, int right, int bottom)
            {
                Left = left; Top = top;
                Right = right; Bottom = bottom;
            }
        }

        Margins _borderMargins = new Margins(0,0,0,20);

        public BaseWindow()
        {
            InitializeComponent();
        }



        public Margins BorderMargins
        {
           get
           {
               return _borderMargins;
           }
           set
           {
               _borderMargins = value;
               updateMargins();
           }
        }




        private void MainWindow_Load(object sender, EventArgs e)
        {
            
            this.BackColor = Color.Black;
            updateMargins();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
         
            if (this.DesignMode)
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.ActiveCaption), 0, 0, this.Width, _borderMargins.Top);
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.ActiveCaption), 0, 0, _borderMargins.Left, this.Height);
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.ActiveCaption), this.Width-_borderMargins.Right,0, _borderMargins.Right, this.Height);
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.ActiveCaption), 0, this.Height - _borderMargins.Bottom, this.Width, _borderMargins.Bottom);
            }
           // e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(204,255,0,255)), 2, 20, 300, 200);
        }


        void updateMargins()
        {

            if(!this.DesignMode&&Environment.OSVersion.Platform == PlatformID.Win32NT &&Environment.OSVersion.Version.Major>=6&&Platform.Windows.DWM.DwmIsCompositionEnabled())
            {
                Platform.Windows.DWM.MARGINS margins = new aPlay.Platform.Windows.DWM.MARGINS(_borderMargins.Left,_borderMargins.Top,_borderMargins.Right,_borderMargins.Bottom);
                Platform.Windows.DWM.DwmExtendFrameIntoClientArea(this.Handle,margins);
            }
        }

    }
}
