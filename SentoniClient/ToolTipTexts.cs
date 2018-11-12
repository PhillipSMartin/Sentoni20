using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SentoniClient
{
    public class ToolTipTexts
    {
        private static Form1 s_form;

        public ToolTipTexts(Form1 form) 
        {
            s_form = form;
        }

        public static string GetText(Control control, string equityType, string clientType)
        {
            //if (s_form != null)
            //    s_form.ShowInfo("Tooltip requested by " + control.Name + ", EquityType = " + equityType);

            string DeltaDivisor;
            if (clientType == "Basket")
                DeltaDivisor = "OLAP market value";
            else
                DeltaDivisor = "Current Equity";

            switch (control.Name)
            {
                // P and L

                case "label4": // Options
                    return "Current market value of all options positions\n    - SOD market value of all options positions\n    + net cash from options trading";
                case "label5": // Stock
                    return "Current market value of all stock positions\n    - SOD market value of all stock positions\n    + net cash from stock trading or dividends";
                case "label6": // Futures
                    return "Current market value of all futures positions\n    - SOD market value of all futures positions\n    + net cash from futures trading";
                case "label7": // Total
                    return "Current market value of all positions\n    - SOD market value of all positions\n    + net cash from trading or dividends";

                // Equity

                case "label9": // Inflows
                    return "Cash added to account since close of trading yesterday\n   (Can be set in EquityManagerClient)";
                case "label2": // SOD Equity
                    return SODEquity(equityType);
                case "label3": // Cash
                    return Cash(equityType);
                case "label17": // Current Equity
                    return "Start-of-day equity plus today's P&L";
                case "label14": // Cash
                    return CurrentCash(equityType);

                // Risk Parameters

                case "label12": // Min Delta
                    return "The minimum TotalDelta% permissible before we must adjust the portfolio\n   If we have fewer than 9 days to expiration, we raise the minimum minute-by-minute linearly\n   so that we reach 1/3 of the way to the target by expiration";
                case "label16": // Max Delta
                    return "The maximum TotalDelta% permissible before we must adjust the portfolio\n   If we have fewer than 9 days to expiration, we lower the maximum minute-by-minute linearly\n   so that we reach 1/3 of the way to the target by expiration";
                case "label15": // Target Delta
                    return "The TotalDelta% we wish to hold according to the strategy defined for this account";
                case "label11": // Leverage
                    return "The target value of our stock and futures portfolio expressed as a percentage of equity";
                case "label18": // Puts % Target
                    return "The percentage of our equity that should be covered by puts - calculated as (Current Market Value - Current Equity) / Current Equity (or 0 if this is negative)";
                case "label21": // Maximum Caks
                    return "The maximum magnitude permitted for Caks in any index";

                // Risk Totals

                case "labelTotalDeltaPctLabel": // Total Delta %
                    return "OLAP market value (stock plus indices associated with futures)\n   + the dollar deltas of all options,\n   all divided by " + DeltaDivisor;
                case "label27": // Annualized Theta
                    return "Sum of thetas of each option times 360, divided by " + DeltaDivisor;
                case "label36": // Current Leverage
                    return "Market value of stock and futures portfolio (treating short puts as stock) divided by current equity";

                // Trading Goals

                case "labelDeltaGoalLabel": // Delta Goal
                    return "The Total Delta % we wish to achieve.\n   If outside our bounds, calculated as .05 within the bounds";
                case "labelPutsToTradeLabel": // Puts To Trade
                    return "The number of puts we must buy to get within our Puts % Target";
                case "labelNextTradeTimeLabel": //Next Trade Time
                    return "The time our next trading slot for this accouont begins";

                    // Today's Trades
                case "labelDollarDeltasTradedLabel": // Dollar Deltas 
                    return "Dollar Deltas of all calls and puts traded today";
                case "labelDeltaPctTradedLabel": // Delta Pct 
                    return "Total Delta % of all calls and puts traded today";
                case "labelPutsTradedLabel": // Puts
                    return "Net number of puts traded today";

                // Indices

                case "label8": // Weight
                    return "The percentage of " + DeltaDivisor + " we want associated with this index";
                case "label10": // Price
                    return "The last price (or closing price if closed) of this index";
                case "label35": // Short %
                    return "The market value of all options on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "label39": // Time Premium
                    return "The premium over parity of all options on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "label43": // Caks
                    return "The sum of the 100-delta dollar deltas of all calls on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "label13": // Gamma %
                    return "The sum of the gamma deltas of all options on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "labelCallDeltaPct": // Call Delta %
                    return "The sum of the dollar deltas of all calls on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "label31": // Total Delta %
                    return "OLAP market value (stock plus indices associated with futures) times Weight\n   + the dollar deltas of all options on this index,\n   all divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "labelDeltasToTradeLabel": // Deltas to Trade
                    return "The deltas we must buy (or sell) on this index";
                case "labelFaceValuePutsPct": // Face Value Puts %
                    return "The sum of the 100-delta dollar deltas of all puts on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "labelPutDeltaPct": // Put Delta %
                    return "The sum of the dollar deltas of all puts on this index\n   divided by Target Value (i.e., Weight times " + DeltaDivisor + ")";
                case "labelPutsToRebalanceLabel": // Puts to Rebalance
                    return "The number of puts to buy (or sell) on this index when rolling\n   to balance puts properly across all indices";
                case "labelPutsToTradeLabel2": // Puts to Trade
                    return "The number of puts we must buy on this index";

                // Total

                case "labelTargetPerformance":
                    return "Weighted average of returns on each targeted index";
                case "TargetShortPct": // Short %
                    return "The market value of all options\n   divided by " + DeltaDivisor;
                case "TargetTimePremium": // Time Premium
                    return "The premium over parity of all options\n   divided by " + DeltaDivisor;
                case "TargetCaks": // Caks
                    return "The sum of the 100-delta dollar deltas of all calls\n   divided by " + DeltaDivisor;
                case "TargetGammaPct": // Gamma %
                    return "The sum of the gamma deltas of all options\n   divided by " + DeltaDivisor;
                case "TargetCallDeltaPct": // Call Delta %
                    return "The sum of the dollar deltas of all calls\n   divided by " + DeltaDivisor;
                case "TargetTotalDeltaPct": // Total Delta %
                    return "OLAP market value (stock plus indices associated with futures)\n   + the dollar deltas of all options,\n   all divided by " + DeltaDivisor;
                case "TargetFaceValuePutsPct": // Face Value Puts %
                    return "The sum of the 100-delta dollar deltas of all puts\n   divided by " + DeltaDivisor;
                case "TargetPutDeltaPct": // Put Delta %
                    return "The sum of the dollar deltas of all puts\n   divided by " + DeltaDivisor;
                case "TargetPutsToTrade": // Puts to Trade
                    return "The number of puts we must buy in total";

                default:
                    return "No tooltip yet - work in progress";
            }
        }

        private static string SODEquity(string equityType)
        {
            switch (equityType.TrimEnd())
            {
                case "Monthly":
                    return "Equity at the start of the month (supplied by accounting)\n   + inflows during this month\n   + P&L from the first of the month to yesterday's close";
                case "CumulativePlusInflows":
                    return "Equity at inception\n   + inflows since inception\n   + net P&L since inception";
                case "BasePlusInflows":
                    return "Base equity (reset at start of month)\n   + inflows during this month\n   + P&L from the first of the month to yesterday's close";
                case "CashPlusInflows":
                    return "Start-of-day cash (as seen in EquityManagerClient)\n   + today's inflows\n   + start-of-day market value of all positions";
                case "Fixed":
                    return "Fixed amount, not adjusted for performance";
                default:
                    return "Start-of-day equity as shown in EquityManagerClient";
            }
        }

        private static string Cash(string equityType)
        {
            switch (equityType.TrimEnd())
            {
                case "CashPlusInflows":
                    return "Available cash at start of day as seen in EquityManagerClient";
                default:
                    return "Start-of-day equity times leverage\n   - start-of-day market value of all positions";
            }
        }

        private static string CurrentCash(string equityType)
        {
            switch (equityType.TrimEnd())
            {
                case "CashPlusInflows":
                    return "Available cash at start of day minus any cash spent on today's trades";
                default:
                    return "Current equity times leverage\n   - current market value of all positions";
            }
        }
      
    }
}
