using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gargoyle.Utilities.CommandLine;

namespace SentoniHost
{
    public class CommandLineParameters
    {

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "pname", Description = "Name of program to specify to DBAccess")]
        public string ProgramName = "SentoniHost";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "tname", Description = "Task name for reporting completion - single space to ignore")]
        public string TaskName = "SentoniHost";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, Description = "Time app should automatically stop, expressed as a string hh:mm")]
        public string StopTime = "17:30";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "qh", Description = "Machine QuoteListener is running on - single space if we don't want to connect")]
        public string QuoteServerHost = "Garbloom.GSI.com";

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "qp", Description = "Port QuoteListener is running on")]
        public int QuoteServerPort = 20000;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, Description = "Time in milleseconds before events time out")]
        public int Timeout = 10000;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, Description = "Frequency with which we request refreshes from the data base")]
        public int RefreshMs = 10000;

        [CommandLineArgumentAttribute(CommandLineArgumentType.AtMostOnce, ShortName = "al", Description = "Maximum number of accounts to load (to facilitate testing)")]
        public int AccountLimit = 0;    // 0 means no limit

        public bool GetStopTime(out TimeSpan stopTime)
        {
            return TimeSpan.TryParse(StopTime, out stopTime);
        }
    }
}
