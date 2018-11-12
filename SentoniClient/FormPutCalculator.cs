using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SentoniClient
{
    public partial class FormPutCalculator : Form
    {
        private Snapshot m_snapshot;

         private double m_desiredDeltaPercent = 0;
         private double m_currentDeltaPercent = 0;
         private double m_newDeltaPercent = 0;

        private double m_maxNotionalValue = 0;
        private double m_currentNotionalValue = 0;
        private double m_newNotionalValue = 0;
 
        private double m_deltaOfPutToBuy = 0;
        private double m_strikePrice = 0;
        private double m_multiplier = 100;
        private int m_contractsToBuy = 0;
 
        private double m_currentDollarDeltas = 0;
        private double m_currentEquity = 0;
        private double m_indexPrice = 0;
        private bool m_loadedSuccessfully = false;
        private Action<string, Exception> m_errorHandler;

        public FormPutCalculator(Snapshot snapshot,  Action<string, Exception> errorHandler)
        {
            if (snapshot == null)
                throw new ArgumentNullException("snapshot");
            m_snapshot = snapshot;
            m_errorHandler = errorHandler;

            InitializeComponent();
        }

        private void FormPutCalculator_Load(object sender, EventArgs e)
        {
            try
            {
                m_loadedSuccessfully = true;

                if (m_snapshot.Indices.Rows.Count > 0)  // must have at least one index and a total
                {
                    AccountDataSet.IndicesRow totalRow = m_snapshot.Indices[m_snapshot.Indices.Rows.Count - 1];
                    textBoxCurrentDelta.Text = String.Format("{0:f3}", m_currentDeltaPercent = totalRow.TotalDeltaPct);
                }
                else
                {
                    ShowError("No delta percent supplied");
                    m_loadedSuccessfully = false;
                }

                if (m_snapshot.AccountData.Count > 0)
                {
                    m_desiredDeltaPercent = m_snapshot.AccountData[0].DeltaGoal;
                    if (m_desiredDeltaPercent < .0001)
                        m_desiredDeltaPercent = m_snapshot.AccountData[0].TargetDelta;
                    textBoxDesiredDelta.Text = String.Format("{0:f3}", m_desiredDeltaPercent);
  
                    m_currentEquity = m_snapshot.AccountData[0].CurrentEquity;
                    textBoxMaxNotionalValue.Text = String.Format("{0:N0}", m_maxNotionalValue = m_snapshot.AccountData[0].BaseEquity);
                }
                else
                {
                    ShowError("No account info supplied");
                    m_loadedSuccessfully = false;
                }

                if (m_snapshot.Positions != null)
                {
                    List<AccountDataSet.PortfolioRow> rows = new List<AccountDataSet.PortfolioRow>(m_snapshot.Positions.Where(x =>
                        (!x.IsOptionTypeNull())
                        && (x.OptionType == "Put")
                        && (x.AccountName == m_snapshot.AccountName)
                        && (x.Current_Position != -x.NettingAdjustment)));
                    rows.Sort(new SortByDelta());
                    dataGridView1.DataSource = rows;
                }
                else
                {
                    ShowError("No positions supplied");
                    m_loadedSuccessfully = false;
                }

                double changeInDelta = 0;
                m_loadedSuccessfully &= CalculateRisk(ref m_currentNotionalValue, ref changeInDelta, ref m_indexPrice);
                textBoxCurrentNotionalValue.Text = String.Format("{0:N0}", m_currentNotionalValue);
            }
            catch (Exception ex)
            {
                ShowError("Error loading info", ex);
                m_loadedSuccessfully = false;
            }
        }

        #region Parse DataGridViewRows
        private class SortByDelta : IComparer<AccountDataSet.PortfolioRow>
        {
            public int Compare(AccountDataSet.PortfolioRow x, AccountDataSet.PortfolioRow y)
            {
                if (x == null)
                {
                    return (y == null) ? 0 : -1;
                }
                else
                {
                    return (y == null) ? 1 : y.Delta.CompareTo(x.Delta);
                }
            }
        }

        public double GetDelta(DataGridViewRow row)
        {
            return (double)row.Cells[deltaDataGridViewTextBoxColumn.Index].Value;
        }

        public double GetStrikePrice(DataGridViewRow row)
        {
            return (double)(decimal)row.Cells[strikePriceDataGridViewTextBoxColumn.Index].Value;
        }

        public int GetNumContractsToSell(DataGridViewRow row)
        {
            if (row.Cells[NumberToSell.Index].Value == null)
                return 0;
            else
                return (int)row.Cells[NumberToSell.Index].Value;
        }
 
        public int GetOldPosition(DataGridViewRow row)
        {
            return (int)row.Cells[CurrentPosition.Index].Value + (int)row.Cells[NettingAdjustment.Index].Value;
        }

        public int GetNewPosition(DataGridViewRow row)
        {
            return GetOldPosition(row) - GetNumContractsToSell(row);
        }

        public short GetMultiplier(DataGridViewRow row)
        {
            return (short)row.Cells[multiplierDataGridViewTextBoxColumn.Index].Value;
        }

        public double GetOldNotionalValue(DataGridViewRow row)
        {
            return GetStrikePrice(row) * GetOldPosition(row) * GetMultiplier(row);
        }

        public double GetNewNotionalValue(DataGridViewRow row)
        {
            return GetStrikePrice(row) * GetNewPosition(row) * GetMultiplier(row);
        }

        public double GetDeltasTraded(DataGridViewRow row)
        {
            return - GetDelta(row) * GetNumContractsToSell(row) * GetMultiplier(row);
        }

        public double GetIndexPrice(DataGridViewRow row)
        {
            return (double)row.Cells[_100DeltaUSD.Index].Value / (GetOldPosition(row) * GetMultiplier(row));
        }
        #endregion

        private bool CalculateRisk(ref double notionalValue, ref double changeInDelta, ref double indexPrice)
        {
            try
            {
                 int numberOfPutsToBuy = int.Parse(textBoxNumberToBuy.Text);
                 notionalValue = m_strikePrice * numberOfPutsToBuy * m_multiplier;
                 changeInDelta = m_deltaOfPutToBuy * numberOfPutsToBuy * m_multiplier;
                 indexPrice = 0;

                 foreach (DataGridViewRow row in dataGridView1.Rows)
                 {
                     notionalValue += GetNewNotionalValue(row);
                     changeInDelta += GetDeltasTraded(row);

                     if (indexPrice == 0)
                     {
                         indexPrice = GetIndexPrice(row);
                     }
                 }

                return true;
            }
            catch (Exception ex)
            {
                ShowError("Error calculating risk", ex);
                return false;
            }

        }

        private bool ReduceNotionalValue(double requiredReduction)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    int position = GetNewPosition(row);
                    if (position > 0)
                    {
                        int numberToSell = (int)Math.Ceiling(requiredReduction / (GetStrikePrice(row) * GetMultiplier(row)));
                        row.Cells[NumberToSell.Index].Value = GetNumContractsToSell(row) + Math.Min(numberToSell, position);
                        if (numberToSell <= position)
                            return true;
                        requiredReduction -= position * GetStrikePrice(row) * GetMultiplier(row);
                    }
                }

                ShowError("Unable to satisfy constraints");
                return false;
            }
            catch (Exception ex)
            {
                ShowError("Error calculating risk", ex);
                return false;
            }
      }

        public void ShowError(string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (ex == null)
                    labelStatus.Text = msg;
                else
                    labelStatus.Text = String.Format("{0}->{1}", msg, ex.Message);
                labelStatus.ForeColor = System.Drawing.Color.Red;

                if ((m_errorHandler != null)  && !String.IsNullOrEmpty(msg))
                    m_errorHandler(msg, ex);
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        private void buttonVerify_Click(object sender, EventArgs e)
        {
            buttonCalculate.Enabled = false;

            textBoxNumberToBuy.Text = "0";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[NumberToSell.Index].Value = 0;
            }

            double changeInDelta = 0;
            double indexPrice = 0;
            if (!CalculateRisk(ref m_newNotionalValue, ref changeInDelta, ref indexPrice))
            {
                return;
            }
            textBoxNewNotionalValue.Text = String.Format("{0:N0}", m_newNotionalValue);

            if (!double.TryParse(textBoxMaxNotionalValue.Text, out m_maxNotionalValue))
            {
                ShowError("Invalid max notional value");
                return;
            }
            if (m_maxNotionalValue <= 0)
            {
                ShowError("Invalid max notional value");
                return;
            }

            if (!double.TryParse(textBoxCurrentDelta.Text, out m_currentDeltaPercent))
            {
                ShowError("Invalid current delta");
                return;
            }
            if ((m_currentDeltaPercent < 0) || (m_currentDeltaPercent > 1))
            {
                ShowError("Invalid current delta");
                return;
            }
            m_currentDollarDeltas = m_currentEquity * m_currentDeltaPercent;

            if (!double.TryParse(textBoxDesiredDelta.Text, out m_desiredDeltaPercent))
            {
                ShowError("Invalid desired delta");
                return;
            }
            if ((m_desiredDeltaPercent < 0) || (m_desiredDeltaPercent > 1))
            {
                ShowError("Invalid desired delta");
                return;
            }

            if (!double.TryParse(textBoxDeltaOfPutToBuy.Text, out m_deltaOfPutToBuy))
            {
                ShowError("Invalid delta of put to buy");
                return;
            }
            m_deltaOfPutToBuy = -Math.Abs(m_deltaOfPutToBuy);
            if ((m_deltaOfPutToBuy >= 0) || (m_desiredDeltaPercent < -1))
            {
                ShowError("Invalid delta of put to buy");
                return;
            }

            if (!double.TryParse(textBoxStrikePrice.Text, out m_strikePrice))
            {
                ShowError("Invalid strike price");
                return;
            }
            if (m_strikePrice <= 0)
            {
                ShowError("Invalid strike price");
                return;
            }

            if (!double.TryParse(textBoxMultiplier.Text, out m_multiplier))
            {
                ShowError("Invalid shares per contract");
                return;
            }
            if (m_multiplier <= 0)
            {
                ShowError("Invalid shares per contract");
                return;
            }

            if (m_indexPrice <= 0)
            {
                ShowError("Invalid implied index price");
                return;
            }

            ShowError("");
            buttonCalculate.Enabled = true;
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                buttonCalculate.Enabled = false;
                bool bReductionWorked = true;
                double changeInDelta = 0;
                double indexPrice = 0;

                double deltasToBuy = (m_currentDeltaPercent - m_desiredDeltaPercent) * m_currentEquity / (m_indexPrice * m_multiplier);
                int contractsToBuy = (int)Math.Round(-deltasToBuy / m_deltaOfPutToBuy);

                while (bReductionWorked && contractsToBuy != m_contractsToBuy)
                {
                    m_contractsToBuy = contractsToBuy;
                    DisplayNumberToTrade(m_contractsToBuy);

                    if (!CalculateRisk(ref m_newNotionalValue, ref changeInDelta, ref indexPrice))
                    {
                        return; // CalculateRisk will have displayed an error message
                    }
                    m_newDeltaPercent = m_currentDeltaPercent + (changeInDelta * m_indexPrice) / m_currentEquity;
                    textBoxNewDelta.Text = String.Format("{0:f3}", m_newDeltaPercent);
                    textBoxNewNotionalValue.Text = String.Format("{0:N0}", m_newNotionalValue);

                    if (m_newNotionalValue > m_maxNotionalValue)
                    {
                        bReductionWorked = ReduceNotionalValue(m_newNotionalValue - m_maxNotionalValue);
                        if (!CalculateRisk(ref m_newNotionalValue, ref changeInDelta, ref indexPrice))
                        {
                            return; // CalculateRisk will have displayed an error message
                        }
                        m_newDeltaPercent = m_currentDeltaPercent + (changeInDelta * m_indexPrice) / m_currentEquity;
                        textBoxNewDelta.Text = String.Format("{0:f3}", m_newDeltaPercent);
                        textBoxNewNotionalValue.Text = String.Format("{0:N0}", m_newNotionalValue);
                        deltasToBuy += (m_newDeltaPercent - m_desiredDeltaPercent) * m_currentEquity / (m_indexPrice * m_multiplier);
                        contractsToBuy = (int)Math.Round(-deltasToBuy / m_deltaOfPutToBuy);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Error calculating trade", ex);
            }

        }

        private void DisplayNumberToTrade(int contractsToBuy)
        {
            textBoxNumberToBuy.Text = String.Format("{0}", Math.Abs(contractsToBuy));
            if (contractsToBuy < 0)
            {
                labelTradeVolume.Text = "Number to Sell";
                labelTradeVolume.ForeColor = Color.Red;
                textBoxNumberToBuy.ForeColor = Color.Red;
            }
            else
            {
                labelTradeVolume.Text = "Number to Buy";
                labelTradeVolume.ForeColor = Color.Black;
                textBoxNumberToBuy.ForeColor = Color.Black;
            }
        }
    }
}
