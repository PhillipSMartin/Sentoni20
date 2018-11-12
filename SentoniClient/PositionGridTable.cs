using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    public class PositionGridTable : DataTable
    {
        private decimal m_atmStrike;
        private decimal m_down10PercentStrike;
        private decimal m_down2PercentStrike;
        private string m_indexSymbol;
        private decimal m_strikeIncrement;

        public PositionGridTable(AccountDataSet.IndicesRow index, double strikeIncrement)
        {
            m_indexSymbol = index.Symbol;
            m_strikeIncrement = (decimal)strikeIncrement;
            m_atmStrike = (decimal)(Math.Round(index.LastPrice / strikeIncrement) * strikeIncrement);
            m_down10PercentStrike = (decimal)(Math.Round(index.LastPrice * .9 / strikeIncrement) * strikeIncrement);
            m_down2PercentStrike = (decimal)(Math.Round(index.LastPrice * .98 / strikeIncrement) * strikeIncrement);

            Columns.Add(new DataColumn("Strike", typeof(PositionGridStrike)));
            PrimaryKey = new DataColumn[1] { Columns[0] };
        }

        public void BuildTable(Snapshot snapshot)
        {
            Clear();
            if (snapshot != null)
            {
                if (snapshot.Positions != null)
                {
                    List<DateTime> expirations = new List<DateTime>();
                    List<decimal> strikes = new List<decimal>();
                    decimal minStrike = m_down10PercentStrike;
                    decimal maxStrike = 0;

                    foreach (AccountDataSet.PortfolioRow row in snapshot.Positions.Rows)
                    {
                        if (row.IsOption && (row.UnderlyingSymbol == m_indexSymbol))
                        {
                            decimal strike = row.StrikePrice;
                            DateTime expiration = row.ExpirationDate;

                            if (!expirations.Contains(expiration))
                            {
                                expirations.Add(expiration);
                            }
                            if (!strikes.Contains(strike))
                            {
                                strikes.Add(strike);
                                if (strike < minStrike)
                                    minStrike = strike;
                                if (strike > maxStrike)
                                    maxStrike = strike;
                            }
                        }
                    }

                    if ((expirations.Count > 0) && (strikes.Count > 0))
                    {
                        expirations.Sort();

                        for (decimal strike = minStrike; strike < maxStrike; strike += m_strikeIncrement)
                        {
                            if (!strikes.Contains(strike))
                                strikes.Add(strike);
                        }
                        strikes.Sort();

                        foreach (DateTime expiration in expirations)
                        {
                            Columns.Add(new DataColumn(expiration.ToShortDateString(), typeof(PositionGridPosition)));
                        }
                        foreach (decimal strike in strikes)
                        {
                            DataRow row = NewRow();
                            PositionGridItem.StrikeTypeEnum strikeType = PositionGridItem.StrikeTypeEnum.NORMAL;
                            if (strike == m_atmStrike)
                                strikeType = PositionGridItem.StrikeTypeEnum.ATTHEMONEY;
                            else if (strike == m_down10PercentStrike)
                                strikeType = PositionGridItem.StrikeTypeEnum.DOWN10PERCENT;
                            else if (strike == m_down2PercentStrike)
                                strikeType = PositionGridItem.StrikeTypeEnum.DOWN2PERCENT;

                            row[0] = new PositionGridStrike(strike, strikeType);
                            for (int n = 1; n <= expirations.Count; n++)
                            {
                                row[n] = new PositionGridPosition(strikeType);
                            }
                            Rows.Add(row);
                        }

                        foreach (AccountDataSet.PortfolioRow row in snapshot.Positions.Rows)
                        {
                            if (row.IsOption && (row.UnderlyingSymbol == m_indexSymbol))
                            {
                                PositionGridPosition item = Find(row.StrikePrice, row.ExpirationDate);
                                if (item == null)
                                    throw new Exception(String.Format("Cannot find cell for strike price {0} and expiration {1}", row.StrikePrice, row.ExpirationDate));

                                item.Position += row.Current_Position;
                                item.Delta = row.Delta;
                            }
                        }
                    }
                }
            }

        }
        private PositionGridPosition Find(decimal strikePrice, DateTime expirationDate)
        {
            foreach (DataRow row in Rows)
            {
                if (strikePrice == ((PositionGridStrike)row[0]).StrikePrice)
                {
                    return (PositionGridPosition)row[expirationDate.ToShortDateString()];
                }
            }
            return null;
        }
    }
}
