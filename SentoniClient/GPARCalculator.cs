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
    public partial class GPARCalculator : Form
    {
        public GPARCalculator()
        {
            InitializeComponent();
        }

        private class CallPortfolio
        {
            public string symbol;
            public int position;
            public double delta;
            public double weight;
            public Label tradeVolume;
         }

        public double Equity
        {
            set
            {
                textBoxEquity.Text = string.Format("{0:C2}", value);
            }
            get
            {
                double equity;
                return double.TryParse(textBoxEquity.Text.Replace("$","").Replace(",",""), out equity) ? equity : 0;
            }
        }
        public double Multiplier
        {
            set
            {
                textBoxMultiplier.Text = string.Format("{0}", value);
            }
        }
        public double TargetDelta
        {
            set
            {
                textBoxTargetDelta.Text = string.Format("{0}", value);
            }
            get
            {
                double targetDelta;
                return double.TryParse(textBoxTargetDelta.Text, out targetDelta) ? targetDelta : 0;
            }
        }

        public double TotalDeltaPct { get; set; }

        public double IndexPrice
        {
            set
            {
                textBoxIndexPrice.Text = string.Format("{0}", value);
                OTMStrike = .98 * value;
            }
            get
            {
                double indexPrice;
                return double.TryParse(textBoxIndexPrice.Text, out indexPrice) ? indexPrice : 1;
            }
        }
        public double OTMStrike
        {
            set
            {
                labelOTMStrike.Text = string.Format("{0}", Math.Round(value, 0));
            }
        }
        public int NumVerticals
        {
            set
            {
                textBoxNumVerticals.Text = string.Format("{0}", value);
            }
        }
        public double VerticalDelta
        {
            set
            {
                textBoxVerticalDelta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }
        public double OTMPutDelta
        {
            set
            {
                textBoxOTMPutDelta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }
        public double DeltasToTrade
        {
            set
            {
                int deltasToTrade = (int)Math.Round(value, 0);
                textBoxDeltasToTrade.Text = string.Format("{0}", Math.Abs(deltasToTrade));
                if (deltasToTrade > 0)
                    radioButtonBuy.Checked = true;
                else if (deltasToTrade < 0)
                    radioButtonSell.Checked = true;
                else
                    radioButtonRoll.Checked = true;
            }
        }

        // use OSI symbol to set property
        public string Call1
        {
            set
            {
                labelCall1.Text = FormatCallLabel(value);
            }
        }
        public string Call2
        {
            set
            {
                labelCall2.Text = FormatCallLabel(value);
            }
        }
        public string Call3
        {
            set
            {
                labelCall3.Text = FormatCallLabel(value);
            }
        }
        public string Call4
        {
            set
            {
                labelCall4.Text = FormatCallLabel(value);
            }
        }
        public string Call5
        {
            set
            {
                labelCall5.Text = FormatCallLabel(value);
            }
        }
        public int Call1Position
        {
            set
            {
                textBoxCall1Position.Text = string.Format("{0}", value);
            }
        }
        public int Call2Position
        {
            set
            {
                textBoxCall2Position.Text = string.Format("{0}", value);
            }
        }
        public int Call3Position
        {
            set
            {
                textBoxCall3Position.Text = string.Format("{0}", value);
            }
        }
        public int Call4Position
        {
            set
            {
                textBoxCall4Position.Text = string.Format("{0}", value);
            }
        }
        public int Call5Position
        {
            set
            {
                textBoxCall5Position.Text = string.Format("{0}", value);
            }
        }
        public double Call1Delta
        {
            set
            {
                textBoxCall1Delta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }
        public double Call2Delta
        {
            set
            {
                textBoxCall2Delta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }
        public double Call3Delta
        {
            set
            {
                textBoxCall3Delta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }
        public double Call4Delta
        {
            set
            {
                textBoxCall4Delta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }
        public double Call5Delta
        {
            set
            {
                textBoxCall5Delta.Text = string.Format("{0}", Math.Round(value, 3));
            }
        }

        private string FormatCallLabel(string value)
        {
            return string.Format("{0} 20{1}-{2}-{3} {4:d} Calls", value.Substring(0, 6).Trim(), value.Substring(6, 2), value.Substring(8, 2), value.Substring(10, 2), int.Parse(value.Substring(13, 5)));
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double equity;
                double multiplier;
                double targetDelta;
                double indexPrice;
                double callDelta = 0;
                double putDelta = 0;
                double otmPutDelta = 0;
                double numVerticals = 0;
                double verticalDelta;
                double deltasToTrade = 0;
 
                labelCallsToSell.Text = "";
                labelPutsToSell.Text = "";
                labelPutsToBuy.Text = "";
                labelMessage.Text = "";
                labelCall1Trade.Text = "";
                labelCall2Trade.Text = "";
                labelCall3Trade.Text = "";
                labelCall4Trade.Text = "";
                labelCall5Trade.Text = "";

                #region verify inputs
                if (!double.TryParse(textBoxEquity.Text.Replace("$", "").Replace(",", ""), out equity))
                {
                    labelMessage.Text = "Invalid equity";
                    return;
                }
                if ((equity <= 0))
                {
                    labelMessage.Text = "Equity must be greater than 0";
                    return;
                }

                if (!double.TryParse(textBoxMultiplier.Text, out multiplier))
                {
                    labelMessage.Text = "Invalid multiplier";
                    return;
                }
                if ((multiplier <= 0))
                {
                    labelMessage.Text = "Multiplier must be greater than 0";
                    return;
                }

                if (!double.TryParse(textBoxTargetDelta.Text, out targetDelta))
                {
                    labelMessage.Text = "Invalid target delta percent";
                    return;
                }
                if ((targetDelta <= -1) || (targetDelta >= 1))
                {
                    labelMessage.Text = "Target delta percent must be between -1 and 1";
                    return;
                }

                if (!double.TryParse(textBoxIndexPrice.Text, out indexPrice))
                {
                    labelMessage.Text = "Invalid index price";
                    return;
                }
                if ((indexPrice <= 0))
                {
                    labelMessage.Text = "Index price must be greater than 0";
                    return;
                }
                OTMStrike = .98 * indexPrice;

                if (textBoxATMCallDelta.Enabled)
                {
                    if (!double.TryParse(textBoxATMCallDelta.Text, out callDelta))
                    {
                        labelMessage.Text = "Invalid at-the-money call delta";
                        return;
                    }
                    if ((callDelta >= .9) || (callDelta <= .1))
                    {
                        labelMessage.Text = "At-the-money call delta must be between .10 and .90";
                        return;
                    }
                }

                if (textBoxATMPutDelta.Enabled)
                {
                    if (!double.TryParse(textBoxATMPutDelta.Text, out putDelta))
                    {
                        labelMessage.Text = "Invalid at-the-money put delta";
                        return;
                    }
                    if ((putDelta <= -.9) || (putDelta >= -.1))
                    {
                        labelMessage.Text = "At-the-money put delta must be between -.10 and -.90";
                        return;
                    }
                }

                if (textBoxATMPutDelta.Enabled)
                {
                    if (!double.TryParse(textBoxOTMPutDelta.Text, out otmPutDelta))
                    {
                        labelMessage.Text = "Invalid out-of-the-money put delta";
                        return;
                    }
                    if ((otmPutDelta <= -1) || (otmPutDelta >= 0))
                    {
                        labelMessage.Text = "Out-of-the-money put delta must be between -1 and 0";
                        return;
                    }
                }

                if (textBoxNumVerticals.Enabled)
                {
                    if (!double.TryParse(textBoxNumVerticals.Text, out numVerticals))
                    {
                        labelMessage.Text = "Invalid number of put verticals";
                        return;
                    }
                    if (numVerticals <= 0)
                    {
                        labelMessage.Text = "Number of put verticals must be greater than zero";
                        return;
                    }
                    if (!double.TryParse(textBoxVerticalDelta.Text, out verticalDelta))
                    {
                        labelMessage.Text = "Invalid delta for put vertical";
                        return;
                    }
                    if ((verticalDelta <= -1) || (verticalDelta >= 1))
                    {
                        labelMessage.Text = "Out-of-the-money put delta must be between -1 and 1";
                        return;
                    }
                }

                if (textBoxDeltasToTrade.Enabled)
                {
                    if (!double.TryParse(textBoxDeltasToTrade.Text, out deltasToTrade))
                    {
                        labelMessage.Text = "Invalid number of deltas to trade";
                        return;
                    }
                    if (deltasToTrade <= 0)
                    {
                        labelMessage.Text = "Deltas to trade must be greater than zero";
                        return;
                    }
                }

                #endregion

                if (radioButtonFirstTrade.Checked)
                {
                    numVerticals = (int)Math.Round(equity * multiplier / (indexPrice * 100));
                    double currentDollarDelta = indexPrice * numVerticals * 100 * (otmPutDelta - putDelta);
                    deltasToTrade = (targetDelta * equity - currentDollarDelta) / (indexPrice * 100);
                    labelPutsToSell.Text = labelPutsToBuy.Text = numVerticals.ToString();

                    int callsToSell = (int)Math.Round(Math.Abs(deltasToTrade / callDelta));
                    labelCallsToSell.Text = callsToSell.ToString();
                }
                if (radioButtonRoll.Checked)
                {
                    double currentDollarDelta = indexPrice * numVerticals * 100 * (otmPutDelta - putDelta);
                    deltasToTrade = (targetDelta * equity - currentDollarDelta) / (indexPrice * 100);
                    labelPutsToSell.Text = numVerticals.ToString();

                    int callsToSell = (int)Math.Round(Math.Abs(deltasToTrade / callDelta));
                    labelCallsToSell.Text = callsToSell.ToString();
                }
                if (radioButtonSell.Checked)
                {
                    int callsToSell = (int)Math.Round(Math.Abs(deltasToTrade / (100 *callDelta)));
                    labelCallsToSell.Text = callsToSell.ToString();
                }
                if (radioButtonBuy.Checked)
                {
                    List<CallPortfolio> callPortofio = new List<CallPortfolio>();
                    if (!BuildCallPortfolio(callPortofio))
                        return;

                    foreach (CallPortfolio callPosition in callPortofio)
                    {
                        int callsToSell = (int)Math.Round(callPosition.weight * Math.Abs(deltasToTrade / (100 * callPosition.delta)));
                        callPosition.tradeVolume.Text = callsToSell.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                labelMessage.Text = "Error: " + ex.Message;
            }
        }

        private bool BuildCallPortfolio(List<CallPortfolio> callPositions)
        {
            double totalCallPosition = 0;
            if (!BuildCallPosition(callPositions, labelCall1, textBoxCall1Position, textBoxCall1Delta, labelCall1Trade))
                return false;

            if (!BuildCallPosition(callPositions, labelCall2, textBoxCall2Position, textBoxCall2Delta, labelCall2Trade))
                return false;

            if (!BuildCallPosition(callPositions, labelCall3, textBoxCall3Position, textBoxCall3Delta, labelCall3Trade))
                return false;

            if (!BuildCallPosition(callPositions, labelCall4, textBoxCall4Position, textBoxCall4Delta, labelCall4Trade))
                return false;

            if (!BuildCallPosition(callPositions, labelCall5, textBoxCall5Position, textBoxCall5Delta, labelCall5Trade))
                return false;

            foreach (CallPortfolio callPosition in callPositions)
            {
                totalCallPosition += callPosition.position;
            }

            if (totalCallPosition != 0)
            {
                foreach (CallPortfolio callPosition in callPositions)
                {
                    callPosition.weight = callPosition.position / totalCallPosition;
                }
            }
            return true;
        }

        private bool BuildCallPosition(List<CallPortfolio> callPositions, Label callLabel, TextBox inputCallPosition, TextBox inputCallDelta, Label inputTradeVolume)
        {
            CallPortfolio callPosition = new CallPortfolio() { symbol = callLabel.Text, tradeVolume = inputTradeVolume };
            if (!string.IsNullOrEmpty(inputCallPosition.Text))
            {
                int position;
                if (!int.TryParse(inputCallPosition.Text,  out position))
                {
                    labelMessage.Text = String.Format("Invalid {0} position", callPosition.symbol);
                    return false;
                }
                if (position == 0)
                    return true;
                if (position > 0)
                {
                    labelMessage.Text = String.Format("{0} position must be non-positive", callPosition.symbol);
                    return false;
                }
                callPosition.position = position;

                double delta;
                if (!double.TryParse(inputCallDelta.Text, out delta))
                {
                    labelMessage.Text = String.Format("Invalid {0} delta", callPosition.symbol);
                    return false;
                }
                if ((delta < 0) || (delta > 1))
                {
                    labelMessage.Text = String.Format("{0} delta must be between 0 and 1", callPosition.symbol);
                    return false;
                }
                callPosition.delta = delta;

                callPositions.Add(callPosition);
            }

            return true;
        }

        private void radioButtonTradeType_CheckedChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            if (radioButtonFirstTrade.Checked)
            {
                textBoxCall1Position.Enabled = textBoxCall1Delta.Enabled = false;
                textBoxCall2Position.Enabled = textBoxCall2Delta.Enabled = false;
                textBoxCall3Position.Enabled = textBoxCall3Delta.Enabled = false;
                textBoxCall4Position.Enabled = textBoxCall4Delta.Enabled = false;
                textBoxCall5Position.Enabled = textBoxCall5Delta.Enabled = false;
                textBoxNumVerticals.Enabled = textBoxVerticalDelta.Enabled = false;

                textBoxATMCallDelta.Enabled = textBoxATMPutDelta.Enabled = textBoxOTMPutDelta.Enabled = true;
                textBoxDeltasToTrade.Enabled = false;
            }
            else if (radioButtonRoll.Checked)
            {
                textBoxCall1Position.Enabled = textBoxCall1Delta.Enabled = false;
                textBoxCall2Position.Enabled = textBoxCall2Delta.Enabled = false;
                textBoxCall3Position.Enabled = textBoxCall3Delta.Enabled = false;
                textBoxCall4Position.Enabled = textBoxCall4Delta.Enabled = false;
                textBoxCall5Position.Enabled = textBoxCall5Delta.Enabled = false;
                textBoxNumVerticals.Enabled = textBoxVerticalDelta.Enabled = true;

                textBoxATMCallDelta.Enabled = textBoxATMPutDelta.Enabled = textBoxOTMPutDelta.Enabled = true;
                textBoxDeltasToTrade.Enabled = false;
            }
            else if (radioButtonBuy.Checked)
            {
                textBoxCall1Position.Enabled = textBoxCall1Delta.Enabled = !string.IsNullOrEmpty(labelCall1.Text);
                textBoxCall2Position.Enabled = textBoxCall2Delta.Enabled = !string.IsNullOrEmpty(labelCall2.Text);
                textBoxCall3Position.Enabled = textBoxCall3Delta.Enabled = !string.IsNullOrEmpty(labelCall3.Text);
                textBoxCall4Position.Enabled = textBoxCall4Delta.Enabled = !string.IsNullOrEmpty(labelCall4.Text);
                textBoxCall5Position.Enabled = textBoxCall5Delta.Enabled = !string.IsNullOrEmpty(labelCall5.Text);
                textBoxNumVerticals.Enabled = textBoxVerticalDelta.Enabled = false;

                textBoxATMCallDelta.Enabled = textBoxATMPutDelta.Enabled = textBoxOTMPutDelta.Enabled = false;
                textBoxDeltasToTrade.Enabled = true;
            }
            else if (radioButtonSell.Checked)
            {
                textBoxCall1Position.Enabled = textBoxCall1Delta.Enabled = false;
                textBoxCall2Position.Enabled = textBoxCall2Delta.Enabled = false;
                textBoxCall3Position.Enabled = textBoxCall3Delta.Enabled = false;
                textBoxCall4Position.Enabled = textBoxCall4Delta.Enabled = false;
                textBoxCall5Position.Enabled = textBoxCall5Delta.Enabled = false;
                textBoxNumVerticals.Enabled = textBoxVerticalDelta.Enabled = false;

                textBoxATMCallDelta.Enabled = true;
                textBoxATMPutDelta.Enabled = textBoxOTMPutDelta.Enabled = false;
                textBoxDeltasToTrade.Enabled = true;
            }
        }

        private void AdjustDeltasToTrade()
        {
            try
            {
                DeltasToTrade = (TargetDelta - TotalDeltaPct) * Equity / IndexPrice;
            }
            catch (Exception)
            {
                DeltasToTrade = 0;
            }
        }

        private void textBoxTargetDelta_TextChanged(object sender, EventArgs e)
        {
            AdjustDeltasToTrade();
        }

        private void textBoxEquity_TextChanged(object sender, EventArgs e)
        {
            AdjustDeltasToTrade();
        }

        private void textBoxIndexPrice_TextChanged(object sender, EventArgs e)
        {
            AdjustDeltasToTrade();
        }
    }
}
