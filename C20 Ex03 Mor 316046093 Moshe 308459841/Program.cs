using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace C20_Ex03_Mor_316046093_Moshe_308459841
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(FormFaceBook.Instance);
        }
    }
}
