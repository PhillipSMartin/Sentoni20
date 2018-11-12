using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    public class PositionGridItem
    {
        public enum StrikeTypeEnum { NORMAL, ATTHEMONEY, DOWN10PERCENT, DOWN2PERCENT };

        public StrikeTypeEnum StrikeType = StrikeTypeEnum.NORMAL;

        public PositionGridItem(StrikeTypeEnum strikeType)
        {
            StrikeType = strikeType;
        }

        public virtual bool IsNegative { get { return false; } }
    }

}
