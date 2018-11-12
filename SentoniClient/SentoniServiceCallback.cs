using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SentoniClient
{
    class SentoniServiceCallback : SentoniServiceReference.ISentoniServiceCallback
    {
        Form1 m_form;

        public SentoniServiceCallback(Form1 form)
        {
            m_form = form;
        }

        #region ISentoniServiceCallback Members

        public void PostPortfolio(SentoniServiceReference.Portfolio portfolio)
        {
            try
            {
                if (portfolio == null)
                {
                    m_form.RefreshPositions(null, "No portfolio returned by Sentoni Service");
                    m_form.RefreshRisk(null, "No portfolio returned by Sentoni Service");
                }
                else
                {
                    Snapshot snapshot = new Snapshot()
                    {
                        AccountName = portfolio.AccountName,
                        Positions = BuildPortfolioTable(portfolio),
                        AccountData = BuildAccountTable(portfolio),
                        Indices = BuildIndicesTable(portfolio),
                        SnapshotType = portfolio.SnapshotType,
                        TimeStamp = portfolio.TimeStamp,
                        SavedSnapshots = BuildSnapshotArray(portfolio),
                        QuoteServiceHost = portfolio.QuoteServiceHost,
                        QuoteServiceStoppedTime = portfolio.QuoteServiceStoppedTime,
                        UnsubscribedSymbols = portfolio.UnsubscribedSymbols
                    };

                    m_form.RefreshSnapshot(snapshot, portfolio.ErrorMessage);
                    m_form.RefreshPositions(snapshot, portfolio.ErrorMessage);
                    m_form.RefreshRisk(snapshot, portfolio.ErrorMessage);
                    m_form.LoadComboboxSnapshots(snapshot);
                    m_form.LoadComboboxIndices(snapshot);
                }
            }
            catch (Exception ex)
            {
                m_form.ShowPortfolioError(5, "Error posting portfolio", ex);
                m_form.ShowRiskError(5, "Error posting portfolio", ex);
            }
        }
  
        public void PostBlotter(SentoniServiceReference.Blotter blotter)
        {
            try
            {
                if (blotter == null)
                {
                    m_form.ShowError(7, "No blotter returned by Sentoni Service");
                }
                else
                {
                    AccountDataSet.BlotterDataTable blotterTable = new AccountDataSet.BlotterDataTable();
                    if (blotter.Trades != null)
                    {
                        foreach (SentoniServiceReference.Trade blotterRow in blotter.Trades)
                        {
                            AccountDataSet.BlotterRow newRow = blotterTable.NewBlotterRow();
                            newRow.AccountName = blotterRow.AccountName;
                            newRow.Symbol = blotterRow.Symbol;
                            newRow.OptionType = blotterRow.OptionType;
                            newRow.TradeType = blotterRow.TradeType;
                            newRow.Quantity = blotterRow.Quantity;
                            newRow.Price = blotterRow.Price;
                            newRow.Cost = blotterRow.Cost;
                            newRow.Multiplier = blotterRow.Multiplier;
                            newRow.IsStock = blotterRow.IsStock;
                            newRow.IsOption = blotterRow.IsOption;
                            newRow.IsFuture = blotterRow.IsFuture;
                            newRow.TradeTime = blotterRow.TradeDateTime.TimeOfDay;
                            newRow.TradeSource = blotterRow.TradeSource ?? "";
                            newRow.TradeId = blotterRow.TradeId;
                            newRow.CurrentMarketPrice = blotterRow.CurrentMarketPrice;
                            newRow.PandL = blotterRow.PandL;


                            if (blotterRow.ExpirationDate.HasValue)
                                newRow.ExpirationDate = blotterRow.ExpirationDate.Value;
                            else
                                newRow.SetExpirationDateNull();

                            if (blotterRow.StrikePrice.HasValue)
                                newRow.StrikePrice = blotterRow.StrikePrice.Value;
                            else
                                newRow.SetStrikePriceNull();

                            if (newRow.IsOption)
                            {
                                newRow.UnderlyingSymbol = blotterRow.UnderlyingSymbol;
                            }
                            else
                            {
                                newRow.UnderlyingSymbol = blotterRow.Symbol;
                             }

                            blotterTable.Rows.Add(newRow);
                        }
                    }

                    m_form.RefreshBlotter(blotter.AccountName, blotter.TradeDate, blotterTable, blotter.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                m_form.ShowBlotterError(5, "Error posting blotter", ex);
            }
        }

        public void PostAccountSummaries(SentoniServiceReference.AccountSummary[] accountSummaries)
        {
            try
            {
                if (accountSummaries == null)
                {
                    m_form.RefreshAccountSummaries(null, "No summaries returned by Sentoni Service");
                }
                else
                {
                    AccountDataSet.AccountSummaryDataTable summariesTable = new AccountDataSet.AccountSummaryDataTable();
                    foreach (var accountSummary in accountSummaries)
                    {
                        var summariesRow = summariesTable.NewAccountSummaryRow();
                        summariesRow.AccountName = accountSummary.AccountName;
                        summariesRow.ReturnOnEquity = accountSummary.ReturnOnEquity;
                        summariesRow.Benchmark = accountSummary.Benchmark;
                        if (accountSummary.BenchmarkReturn.HasValue)
                            summariesRow.BenchmarkReturn = accountSummary.BenchmarkReturn.Value;
                        if (accountSummary.TargetReturn.HasValue)
                            summariesRow.TargetReturn = accountSummary.TargetReturn.Value;
                        summariesRow.OptionPandL = accountSummary.OptionPandL;
                        summariesRow.StockPandL = accountSummary.StockPandL;
                        summariesRow.FuturesPandL = accountSummary.FuturesPandL;
                        summariesRow.TotalPandL = accountSummary.TotalPandL;
                        summariesRow.MinDelta = accountSummary.MinDelta;
                        summariesRow.MaxDelta = accountSummary.MaxDelta;
                        summariesRow.TargetDelta = accountSummary.TargetDelta;
                        summariesRow.TotalDeltaPct = accountSummary.TotalDeltaPct;
                        summariesRow.PutsToTrade = accountSummary.PutsToTrade;
                        summariesRow.PutsOutOfBounds = accountSummary.PutsOutOfBounds;
                        summariesRow.TradingComplete = accountSummary.TradingComplete;
                         if (accountSummary.NextTradeTime.HasValue)
                            summariesRow.NextTradeTime = accountSummary.NextTradeTime.Value;
                         summariesRow.GrossReturnMTD = accountSummary.GrossReturnMTD;
                         summariesRow.NetReturnMTD = accountSummary.NetReturnMTD;
                         if (accountSummary.BenchmarkReturnMTD.HasValue)
                             summariesRow.BenchmarkReturnMTD = accountSummary.BenchmarkReturnMTD.Value;
                        summariesTable.AddAccountSummaryRow(summariesRow);

                    }
                    m_form.RefreshAccountSummaries(summariesTable, "");
                }
            }
            catch (Exception ex)
            {
                m_form.ShowAccountSummariesError(5, "Error posting account summary", ex);
            }
        }

        public void PostTradingSchedule(SentoniServiceReference.TradingSchedule[] tradingSchedule)
        {
            try
            {
                if (tradingSchedule == null)
                {
                    m_form.RefreshTradingSchedule(null, "No trading schedule returned  by Sentoni Service");
                }
                else
                {
                    m_form.RefreshTradingSchedule(tradingSchedule, "");
                }
            }
            catch (Exception ex)
            {
                m_form.ShowTradingScheduleError(5, "Error posting account summary", ex);
            }
        }
        #endregion

        #region Private methods

        private static SnapshotId[] BuildSnapshotArray(SentoniServiceReference.Portfolio portfolio)
        {
            SnapshotId[] snapshotArray = null;
            if (portfolio.SnapshotType == SentoniServiceReference.SnapshotType.Current)
            {
                List<SnapshotId> snapshotList = new List<SnapshotId>();
                snapshotList.Add(new SnapshotId()
                {
                    Id = 0,
                    SnapshotType = SentoniServiceReference.SnapshotType.Current
                });

                if (portfolio.Snapshots != null)
                {
                    foreach (SentoniServiceReference.Snapshot savedSnapshot in portfolio.Snapshots)
                    {
                        snapshotList.Add(new SnapshotId()
                        {
                            Id = (short)savedSnapshot.SnapshotId,
                            SnapshotType = savedSnapshot.SnapshotType,
                            TimeStamp = savedSnapshot.TimeStamp
                        });
                    }
                }
                snapshotArray = snapshotList.ToArray();
            }
            return snapshotArray;
        }

        private static AccountDataSet.PortfolioDataTable BuildPortfolioTable(SentoniServiceReference.Portfolio portfolio)
        {
            AccountDataSet.PortfolioDataTable portfolioTable = new AccountDataSet.PortfolioDataTable();
            if (portfolio.Positions != null)
            {
                foreach (SentoniServiceReference.Position portfolioRow in portfolio.Positions)
                {
                    AccountDataSet.PortfolioRow newRow = portfolioTable.NewPortfolioRow();

                    newRow.AccountName = portfolioRow.Account ?? portfolio.AccountName;
                    newRow.Symbol = portfolioRow.Symbol;
                    newRow.SubscriptionStatus = portfolioRow.SubscriptionStatus;
                    newRow.SOD_Position = portfolioRow.SODPosition;
                    newRow.SOD_Price = portfolioRow.SODPrice;
                    newRow.SOD_Market_Value = portfolioRow.SODMarketValue;
                    newRow.Change_in_Position = portfolioRow.ChangeInPosition;
                    newRow.NettingAdjustment = portfolioRow.NettingAdjustment;
                    newRow.Change_in_Cost = portfolioRow.ChangeInCost;
                    newRow.Current_Position = portfolioRow.CurrentPosition;
                    newRow.Current_Price = portfolioRow.CurrentPrice;
                    newRow.Current_Cost = portfolioRow.CurrentCost;
                    newRow.Closed = portfolioRow.Closed;
                    newRow.OptionType = portfolioRow.OptionType;
                    newRow.Multiplier = portfolioRow.Multiplier;
                    newRow.IsStock = portfolioRow.IsStock;
                    newRow.IsOption = portfolioRow.IsOption;
                    newRow.IsFuture = portfolioRow.IsFuture;
                    newRow.IsOutOfBounds = portfolioRow.IsOutOfBounds;

                    if (portfolioRow.ExpirationDate.HasValue)
                        newRow.ExpirationDate = portfolioRow.ExpirationDate.Value;
 
                    if (portfolioRow.Open.HasValue)
                        newRow.Open = portfolioRow.Open.Value;

                    if (portfolioRow.PrevClose.HasValue)
                        newRow.PrevClose = portfolioRow.PrevClose.Value;

                    if (portfolioRow.LastPrice.HasValue)
                        newRow.LastPrice = portfolioRow.LastPrice.Value;

                    if (portfolioRow.Bid.HasValue)
                        newRow.Bid = portfolioRow.Bid.Value;

                    if (portfolioRow.Ask.HasValue)
                        newRow.Ask = portfolioRow.Ask.Value;

                    if (portfolioRow.Mid.HasValue)
                        newRow.Mid = portfolioRow.Mid.Value;

                    if (portfolioRow.ClosingPrice.HasValue)
                        newRow.ClosingPrice = portfolioRow.ClosingPrice.Value;

                    if (portfolioRow.UpdateTime.HasValue)
                        newRow.UpdateTime = portfolioRow.UpdateTime.Value;

                    newRow.Current_Market_Value = portfolioRow.CurrentMarketValue;
                    newRow.P_and_L = portfolioRow.PandL;

                    // option-only fields
                    if (newRow.IsOption)
                    {
                        newRow.UnderlyingSymbol = portfolioRow.UnderlyingSymbol;

                        if (portfolioRow.Delta.HasValue)
                            newRow.Delta = portfolioRow.Delta.Value;
                        if (portfolioRow.DeltaUSD.HasValue)
                            newRow.DeltaUSD = portfolioRow.DeltaUSD.Value;
                        if (portfolioRow._100DeltaUSD.HasValue)
                            newRow._100DeltaUSD = portfolioRow._100DeltaUSD.Value;
                        if (portfolioRow.Gamma.HasValue)
                            newRow.Gamma = portfolioRow.Gamma.Value;
                        if (portfolioRow.GammaUSD.HasValue)
                            newRow.GammaUSD = portfolioRow.GammaUSD.Value;
                        if (portfolioRow.Theta.HasValue)
                            newRow.Theta = portfolioRow.Theta.Value;
                        if (portfolioRow.ThetaAnnualized.HasValue)
                            newRow.ThetaAnnualized = portfolioRow.ThetaAnnualized.Value;
                        if (portfolioRow.Vega.HasValue)
                            newRow.Vega = portfolioRow.Vega.Value;
                        if (portfolioRow.ImpliedVol.HasValue)
                            newRow.ImpliedVol = portfolioRow.ImpliedVol.Value;
                        if (portfolioRow.StrikePrice.HasValue)
                            newRow.StrikePrice = portfolioRow.StrikePrice.Value;
                        if (portfolioRow.TimePremium.HasValue)
                            newRow.TimePremium = portfolioRow.TimePremium.Value;
                    }
                    else
                    {
                        newRow.UnderlyingSymbol = portfolioRow.Symbol;
                        if (newRow.PrevClose != 0)
                        newRow.Performance = newRow.Current_Price / newRow.PrevClose - 1;
                    }

                    portfolioTable.Rows.Add(newRow);
                }
            }
            return portfolioTable;
        }

        private static AccountDataSet.AccountDataDataTable BuildAccountTable(SentoniServiceReference.Portfolio portfolio)
        {
            AccountDataSet.AccountDataDataTable accountDataTable = new AccountDataSet.AccountDataDataTable();
            AccountDataSet.AccountDataRow accountDataRow = accountDataTable.NewAccountDataRow();

            if (portfolio.AccountData != null)
            {
                accountDataRow.EquityType = portfolio.AccountData.EquityType;
                accountDataRow.ClientType = portfolio.AccountData.ClientType;
                accountDataRow.IsTest = portfolio.AccountData.IsTest;
                accountDataRow.BaseEquity = portfolio.AccountData.BaseEquity;
                accountDataRow.BaseCash = portfolio.AccountData.BaseCash;
                accountDataRow.BaseDate = portfolio.AccountData.BaseDate;
                accountDataRow.InflowsSinceBaseDate = portfolio.AccountData.InflowsSinceBaseDate;
                accountDataRow.TodaysInflows = portfolio.AccountData.TodaysInflows;
                accountDataRow.PandLSinceBaseDate = portfolio.AccountData.PandLSinceBaseDate;
                accountDataRow.StartOfDayMarketValue = portfolio.AccountData.StartOfDayMarketValue;
                accountDataRow.AvailableCash = portfolio.AccountData.AvailableCash;
                accountDataRow.StartOfDayEquity = portfolio.AccountData.StartOfDayEquity;
                accountDataRow.CurrentMarketValue = portfolio.AccountData.CurrentMarketValue;
                accountDataRow.CurrentCash = portfolio.AccountData.CurrentCash;
                accountDataRow.CurrentEquity = portfolio.AccountData.CurrentEquity;
                accountDataRow.MinDelta = portfolio.AccountData.MinDelta;
                accountDataRow.MaxDelta = portfolio.AccountData.MaxDelta;
                accountDataRow.TargetDelta = portfolio.AccountData.TargetDelta;
                accountDataRow.DeltaGoal = portfolio.AccountData.DeltaGoal;
                accountDataRow.Leverage = portfolio.AccountData.Leverage;
                accountDataRow.CurrentLeverage = portfolio.AccountData.CurrentLeverage;
                accountDataRow.PutsPctTarget = portfolio.AccountData.PutsPctTarget;
                accountDataRow.Option_P_and_L = portfolio.AccountData.OptionPandL;
                accountDataRow.Stock_P_and_L = portfolio.AccountData.StockPandL;
                accountDataRow.Futures_P_and_L = portfolio.AccountData.FuturesPandL;
                accountDataRow.Total_P_and_L = portfolio.AccountData.TotalPandL;
                accountDataRow.DollarDeltasTraded = portfolio.AccountData.DollarDeltasTraded;
                accountDataRow.DeltaPctTraded = portfolio.AccountData.DeltaPctTraded;
                accountDataRow.PutsTraded = portfolio.AccountData.PutsTraded;
                accountDataRow.PutsOutOfBounds = portfolio.AccountData.PutsOutOfBounds;
                accountDataRow.PutsOutOfMoneyThreshold = portfolio.AccountData.PutsOutOfMoneyThreshold;
                accountDataRow.TradingComplete = portfolio.AccountData.TradingComplete;
                accountDataRow.MaximumCaks = portfolio.AccountData.MaximumCaks;
                accountDataRow.PortfolioPercentage = portfolio.AccountData.PortfolioPercentage;

                if (portfolio.AccountData.NextTradeTime.HasValue)
                    accountDataRow.NextTradeTime = portfolio.AccountData.NextTradeTime.Value;
                else
                    accountDataRow.SetNextTradeTimeNull();
            }

            accountDataTable.AddAccountDataRow(accountDataRow);
            return accountDataTable;
        }

        private static AccountDataSet.IndicesDataTable BuildIndicesTable(SentoniServiceReference.Portfolio portfolio)
        {
            AccountDataSet.IndicesDataTable indicesTable = new AccountDataSet.IndicesDataTable();

            if (portfolio.Indices != null)
            {
                foreach (SentoniServiceReference.Index indexRow in portfolio.Indices)
                {

                    AccountDataSet.IndicesRow newRow = indicesTable.NewIndicesRow();

                    newRow.Symbol = indexRow.Symbol;
                    newRow.Weight = indexRow.Weight;
                    newRow.TargetValue = indexRow.TargetValue;
                    newRow.TotalDeltaPct = indexRow.TotalDeltaPct;
                    newRow.CallDeltaPct = indexRow.CallDeltaPct;
                    newRow.PutDeltaPct = indexRow.PutDeltaPct;
                    newRow.FaceValuePutsPct = indexRow.FaceValuePutsPct;
                    newRow.ShortPct = indexRow.ShortPct;
                    newRow.TimePremium = indexRow.TimePremium;
                    newRow.Caks = indexRow.Caks;
                    newRow.GammaPct = indexRow.GammaPct;
                    newRow.ThetaAnnualized = indexRow.ThetaAnnualized;
                    newRow.DeltasToTrade = indexRow.DeltasToTrade;
                    if (portfolio.Indices.Length > 2)
                    {
                        newRow.PutsToRebalance = indexRow.PutsToRebalance;
                    }
                    newRow.PutsToTrade = indexRow.PutsToTrade;

                    if (indexRow.LastPrice.HasValue)
                        newRow.LastPrice = indexRow.LastPrice.Value;
                    if (indexRow.PrevClose.HasValue)
                        newRow.PrevClose = indexRow.PrevClose.Value;

                    indicesTable.Rows.Add(newRow);
                }
            }
            return indicesTable;
        }
        #endregion
    }
}
