using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    public class PositionGridStrike : PositionGridItem
    {
        public decimal StrikePrice { get; set; }

        public PositionGridStrike(decimal strikePrice, StrikeTypeEnum strikeType) : base(strikeType)
        {
            StrikePrice = strikePrice;
        }

        public override string ToString()
        {
            return String.Format("{0:f4}", StrikePrice);
        }
    }
}
