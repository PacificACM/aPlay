using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xcom.aPlay;

namespace aPlay
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            aPlayApp a = new aPlayApp();
            a.Start();

        }
    }
}
