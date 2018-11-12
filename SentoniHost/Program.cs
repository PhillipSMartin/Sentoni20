using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SentoniHost
{
    class Program
    {
        static void Main(string[] args)
        {
             Host host = null;
             CommandLineParameters parms = new CommandLineParameters();

             string dirDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Gargoyle Strategic Investments\\SentoniHost";
             string appDataPath = dirDataPath + "\\TraceListener.log";
             DirectoryInfo dInfo = new DirectoryInfo(dirDataPath);
             if (!dInfo.Exists)
                 dInfo.Create();

             TextWriterTraceListener trace = new TextWriterTraceListener(new StreamWriter(appDataPath, false));

             try
             {
                 if (Gargoyle.Utilities.CommandLine.Utility.ParseCommandLineArguments(args, parms))
                 {
                     host = new Host(parms);
                     if (host.Run())
                     {
                         trace.WriteLine("Sentoni Host terminiated");
                     }
                     else
                     {
                         trace.WriteLine("Sentoni Host failed - see error log");
                     }
                 }
                 else
                 {
                     // display usage message
                     string errorMessage = Gargoyle.Utilities.CommandLine.Utility.CommandLineArgumentsUsage(typeof(CommandLineParameters));

                     trace.WriteLine(errorMessage);
                 }
             }
             catch (Exception ex)
             {
                 trace.WriteLine(ex.ToString());
             }
            finally
            {
                trace.Flush();
                if (host != null)
                {
                    host.Dispose();
                    host = null;
                }
            }
        }
    }

}
