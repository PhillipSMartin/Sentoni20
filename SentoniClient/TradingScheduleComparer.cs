using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    class TradingScheduleComparer : IComparer<SentoniServiceReference.TradingSchedule>
    {

        public int Compare(SentoniServiceReference.TradingSchedule x, SentoniServiceReference.TradingSchedule y)
        {
            if (x == null)
            {
                return (y == null) ? 0 : -1;
            }
            else
            {
                return (y == null) ? 1 : x.NextTradeTime.CompareTo(y.NextTradeTime);
            }
        }
    }
}
