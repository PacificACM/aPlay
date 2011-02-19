using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xcom.aPlay.Lib;
using System.IO;
using Xcom.aPlay.Util;
using System.Windows.Forms;
using aPlay;
using System.Threading;
using Xcom.aPlay.UI;

namespace Xcom.aPlay
{
    public class aPlayApp
    {

#region private members
        List<ILibrary> _libraries = new List<ILibrary>();
        Settings _settings;

        Thread _uiThread;

#endregion 

        /// <summary>
        /// Starts the applications logic. Loads libraries, ect.
        /// </summary>
        public void Start()
        {
            //Load the app settings
            _settings = new Settings();
            _settings.Load();

            try
            {
                FileInfo dataFile = new FileInfo(_settings.LibraryDatabaseFilePath);
                if(!dataFile.Directory.Exists )
                {
                    dataFile.Directory.Create();
                }
            }
            catch(Exception e)
            {
                //Well this puts us in a pickle
                System.Windows.Forms.MessageBox.Show("Unable to start αPlay! Could not find nor create αPlay directory.\r\nError: " + e.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            //Load main library
            Library mainLib = new Library(_settings.LibraryDatabaseFilePath);
            _libraries.Add(mainLib);

            //Start UI thread
            _uiThread = new Thread(new ThreadStart(delegate {Application.Run(new MainWindow(this));}));
            _uiThread.SetApartmentState(ApartmentState.STA);
            _uiThread.IsBackground = false;
            _uiThread.Start();

        }

        public ILibrary[] Libraries
        {
            get
            {
                return _libraries.ToArray();
            }
        }


        public Settings Settings
        {
            get
            {
                return _settings;
            }
        }

    }
}
