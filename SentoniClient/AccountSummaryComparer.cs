using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    class AccountSummaryComparer : IComparer<SentoniServiceReference.AccountSummary>
    {

        public int Compare(SentoniServiceReference.AccountSummary x, SentoniServiceReference.AccountSummary y)
        {
            if (x == null)
            {
                return (y == null) ? 0 : -1;
            }
            else
            {
                return (y == null) ? 1 : x.AccountName.CompareTo(y.AccountName);
            }
        }
    }
}
