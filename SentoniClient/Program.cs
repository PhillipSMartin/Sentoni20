using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SentoniClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string dirDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Gargoyle Strategic Investments\\SentoniClient";
            string appDataPath = dirDataPath + "\\TraceListener.log";
            DirectoryInfo dInfo = new DirectoryInfo(dirDataPath);
            if (!dInfo.Exists)
                dInfo.Create();

            TextWriterTraceListener trace = new TextWriterTraceListener(new StreamWriter(appDataPath, false));

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                trace.WriteLine(ex.ToString());
                trace.Flush();
            }
        }
    }
}
