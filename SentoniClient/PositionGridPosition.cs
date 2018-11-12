using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    public class PositionGridPosition : PositionGridItem
    {
        private double m_delta;
        public double Delta
        {
            get { return m_delta; }
            set
            {
                if (value < 0)
                    m_delta = 1 + value;
                else
                    m_delta = value;
            }
        }

        public int Position { get; set; }

        public PositionGridPosition(StrikeTypeEnum strikeType)
            : base(strikeType)
        {
        }

        public override string ToString()
        {
            if (Position == 0)
                return "";
            else
                return String.Format("{0} ({1:f3})", Position, Delta);
        }

        public override bool IsNegative
        {
            get
            {
                return Position < 0;
            }
        }
    }
}
