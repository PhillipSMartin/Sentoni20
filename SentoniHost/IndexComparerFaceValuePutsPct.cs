using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SentoniServiceLib;

namespace SentoniHost
{
    class IndexComparerFaceValuePutsPct : IComparer<Index>
    {
        public int Compare(Index x, Index y)
        {
            if (x == null)
            {
                return (y == null) ? 0 : -1;
            }
            else
            {
                return (y == null) ? 1 :  x.FaceValuePutsPct.CompareTo(y.FaceValuePutsPct);
            }
        }
    }
}
