namespace Xcom.aPlay.UI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.libraryViewer1 = new Xcom.aPlay.UI.LibraryViewer();
            this.navigationBar1 = new Xcom.aPlay.UI.NavigationBar();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // libraryViewer1
            // 
            this.libraryViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.libraryViewer1.Location = new System.Drawing.Point(160, 55);
            this.libraryViewer1.Name = "libraryViewer1";
            this.libraryViewer1.Size = new System.Drawing.Size(399, 217);
            this.libraryViewer1.TabIndex = 1;
            // 
            // navigationBar1
            // 
            this.navigationBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.navigationBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(200)))), ((int)(((byte)(175)))), ((int)(((byte)(200)))));
            this.navigationBar1.Libraries = new Xcom.aPlay.Lib.ILibrary[0];
            this.navigationBar1.Location = new System.Drawing.Point(0, 55);
            this.navigationBar1.Name = "navigationBar1";
            this.navigationBar1.Size = new System.Drawing.Size(154, 217);
            this.navigationBar1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(571, 284);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.libraryViewer1);
            this.Controls.Add(this.navigationBar1);
            this.Name = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LibraryViewer libraryViewer1;
        private NavigationBar navigationBar1;
        private System.Windows.Forms.Button button1;
    }
}
