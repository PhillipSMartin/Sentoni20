using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PositionMonitorLib;
using SentoniServiceLib;

namespace SentoniHost
{

    public class DataProvider : ISentoniProvider, ISnapshotProvider
    {
        private PositionMonitorUtilities m_positionMonitor;
        private Host m_host;
        private static TimeSpan s_quoteDelayThreshold = new TimeSpan(0, 1, 0);  // if last quote is beyond this threshold, we assume the quote feed is down

        public struct AdjustDeltaSandbox
        {
            public bool skipIndex;
            public double targetDeltaPercent;
            public double dollarDeltasToTrade;
            public double deltasToTrade;
        };

        public  DataProvider(PositionMonitorUtilities positionMonitor, Host host)
        {
            m_positionMonitor = positionMonitor;
            m_host = host;
         }

        #region ISentoniProvider Members

        public bool ProvideHeartBeat()
        {
            try
            {
                return m_positionMonitor.IsMonitoring;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public  string[] ProvideAccountList()
        {
            List<string> accountList = null;

            try
            {
                accountList = new List<string>(m_positionMonitor.GetAllAccountNames());
                accountList.Sort();
             }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

            if (accountList != null)
                return accountList.ToArray();
            else
                throw new FaultException("Position monitor is not running");
        }

        public AccountSummary[] ProvideAccountSummaries()
        {
            List<AccountSummary> accountSummaries = new List<AccountSummary>();

            AccountPortfolio[] portfolios = m_positionMonitor.GetAllAccountPortfolios();
            if (portfolios != null)
            {
                foreach (AccountPortfolio positionMonitorPortfolio in portfolios)
                {
                    Portfolio portfolio = BuildPortfolio(positionMonitorPortfolio, false);
                    AccountSummary accountSummary = new AccountSummary()
                    {
                        AccountName = portfolio.AccountName,
                        MinDelta = portfolio.AccountData.MinDelta,
                        MaxDelta = portfolio.AccountData.MaxDelta,
                        TargetDelta = portfolio.AccountData.TargetDelta,
                        NextTradeTime = portfolio.AccountData.NextTradeTime,
                        OptionPandL = portfolio.AccountData.OptionPandL,
                        StockPandL = portfolio.AccountData.StockPandL,
                        FuturesPandL = portfolio.AccountData.FuturesPandL,
                        TotalPandL = portfolio.AccountData.TotalPandL,
                        Benchmark = portfolio.AccountData.Benchmark,
                        PutsOutOfBounds = portfolio.AccountData.PutsOutOfBounds,
                        TradingComplete = portfolio.AccountData.TradingComplete,
                     };
                    if (portfolio.AccountData.StartOfDayEquity > 0)
                    {
                        accountSummary.ReturnOnEquity = accountSummary.TotalPandL / portfolio.AccountData.StartOfDayEquity;
                        accountSummary.GrossReturnMTD = Math.Exp(positionMonitorPortfolio.Data.LogGrossReturn_MTD + Math.Log(accountSummary.ReturnOnEquity + 1)) - 1;
                        accountSummary.NetReturnMTD = Math.Exp(positionMonitorPortfolio.Data.LogNetReturn_MTD + Math.Log(accountSummary.ReturnOnEquity + 1)) - 1;
                    }
                    if (portfolio.Benchmark != null)
                    {
                        if (portfolio.Benchmark.PrevClose != 0)
                        {
                            accountSummary.BenchmarkReturn = portfolio.Benchmark.LastPrice / portfolio.Benchmark.PrevClose - 1;
                            accountSummary.BenchmarkReturnMTD = Math.Exp(positionMonitorPortfolio.Data.BenchmarkLogReturn_MTD + Math.Log(accountSummary.BenchmarkReturn.Value + 1)) - 1;
                        }
                    }

                    if (portfolio.Indices.Length > 0)
                    {
                        Index totalRow = portfolio.Indices[portfolio.Indices.Length - 1];
                        accountSummary.TotalDeltaPct = totalRow.TotalDeltaPct;
                        accountSummary.PutsToTrade = totalRow.PutsToTrade;
                        if (totalRow.PrevClose != 0)
                            accountSummary.TargetReturn = totalRow.LastPrice / totalRow.PrevClose - 1;
                    }

                    accountSummaries.Add(accountSummary);
                }
            }

            return accountSummaries.ToArray();
        }

        public Portfolio ProvidePortfolio(string accountName, bool bPositionsForAllAccounts)
        {
            Portfolio portfolio = null;
            try
            {
                AccountPortfolio positionMonitorPortfolio = m_positionMonitor.GetAccountPortfolio(accountName);
                if (positionMonitorPortfolio != null)
                {
                    portfolio = BuildPortfolio(positionMonitorPortfolio, bPositionsForAllAccounts);
                }
            }
            catch (Exception ex)
            {
                ReportError("Error providing portfolio", ex);
            }
            return portfolio;
        }

        public string ProvidePortfolioSnapshot(short snapshotId)
        {
            try
            {
                var row = m_positionMonitor.GetPortfolioSnapshot(snapshotId);
                if (row != null)
                 {
                    return row.Snapshot;
                }
            }
            catch (Exception ex)
            {
                ReportError("Error providing snapshot", ex);
            }
            return null;
        }

        public Blotter ProvideBlotter(string acctName, DateTime tradeDate)
        {
            try
            {
                HugoDataSet.TradesRow[] tradeRows;
                if (acctName == null)
                    tradeRows = m_positionMonitor.GetAllAccountTrades(tradeDate);
                else
                    tradeRows = m_positionMonitor.GetAccountTrades(acctName, tradeDate);

                List<Trade> trades = new List<Trade>();
                foreach (HugoDataSet.TradesRow tradeRow in tradeRows)
                {
                    Trade newTrade = new Trade()
                    {
                        AccountName = tradeRow.AcctName,
                        TradeType = tradeRow.TradeType,
                        Symbol = tradeRow.Symbol,
                        ExpirationDate = tradeRow.IsExpirationDateNull() ? new System.Nullable<DateTime>() : tradeRow.ExpirationDate,
                        StrikePrice = tradeRow.IsStrikePriceNull() ? new System.Nullable<decimal>() : tradeRow.StrikePrice,
                        OptionType = tradeRow.IsOptionTypeNull() ? null : tradeRow.OptionType,
                        UnderlyingSymbol = tradeRow.IsUnderlyingSymbolNull() ? null : tradeRow.UnderlyingSymbol,
                        Multiplier = (short)tradeRow.Multiplier,
                        Quantity = tradeRow.Quantity,
                        Price = tradeRow.Price,
                        Cost = tradeRow.Change_in_Cost,
                        IsStock = tradeRow.IsStock == 1,
                        IsOption = tradeRow.IsOption == 1,
                        IsFuture = tradeRow.IsFuture == 1,
                        TradeDateTime = tradeRow.TradeDateTime,
                        TradeSource = tradeRow.TradeSource,
                        TradeId = tradeRow.TradeId,
                     };

                    if (newTrade.TradeType == "Buy" || newTrade.TradeType == "Sell")
                    {
                        double? currentPrice = m_positionMonitor.GetCurrentPrice(tradeRow.AcctName, tradeRow.Symbol);
                        if (currentPrice.HasValue)
                        {
                            newTrade.CurrentMarketPrice = currentPrice.Value;
                            newTrade.PandL = ((newTrade.TradeType == "Buy") ? 1 : -1) * newTrade.CurrentMarketPrice * newTrade.Multiplier * newTrade.Quantity - newTrade.Cost;
                        }
                    }
                    trades.Add(newTrade);
  
                }

                return new Blotter()
                {
                    AccountName = acctName,
                    TradeDate = tradeDate,
                    Trades = trades.ToArray()
                };
            }
            catch (Exception ex)
            {
                ReportError("Error providing blotter", ex);
                return null;
            }
        }

        public void ReportError(string msg, Exception ex)
        {
            m_host.OnError(msg, ex, false);
        }

        public TradingSchedule[] ProvideTradingSchedule()
        {
            List<TradingSchedule> tradingSchedule = new List<TradingSchedule>();

            try
            {
                var tradingScheduleRows = m_positionMonitor.GetTradingScheduleRows();

                if (tradingScheduleRows != null)
                {
                    foreach (var tradingScheduleRow in tradingScheduleRows)
                    {
                        TradingSchedule newRow = new TradingSchedule()
                        {
                            AccountName = tradingScheduleRow.AcctName,
                            NextTradeTime = tradingScheduleRow.StartTradingTime.TimeOfDay,
                            EndTradeTime = tradingScheduleRow.EndTradingTime.TimeOfDay,
                            MinutesToTrade = tradingScheduleRow.TradingSliceTime,
                            StartOfTradingSnapshotTaken = tradingScheduleRow.StartOfTradingSnapshotTaken > 0,
                            EndOfTradingSnapshotTaken = tradingScheduleRow.EndOfTradingSnapshotTaken > 0,
                            EndOfDaySnapshotTaken = tradingScheduleRow.EndOfDaySnapshotTaken > 0
                        };

                        tradingSchedule.Add(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError("Error providing trading schedule", ex);
            }
            return tradingSchedule.ToArray();
        }

        public void SavePortfolioSnapshot(Portfolio portfolio, string xml)
        {
            try
            {
                double deltaPercent = 0;
                double faceValuePutsPercent = 0;
                double returnOnEquity = 0;
                bool isViolation = false;

                if (portfolio.Indices.Length > 0)
                {
                    deltaPercent = portfolio.Indices[portfolio.Indices.Length - 1].TotalDeltaPct;
                    faceValuePutsPercent = portfolio.Indices[portfolio.Indices.Length - 1].FaceValuePutsPct;

                   if ((deltaPercent > portfolio.AccountData.MaxDelta) || (deltaPercent < portfolio.AccountData.MinDelta))
                        isViolation = true;
                }
                if (portfolio.AccountData.StartOfDayEquity > 0)
                {
                    returnOnEquity = portfolio.AccountData.TotalPandL / portfolio.AccountData.StartOfDayEquity;
                }

                m_positionMonitor.InsertPortfolioSnapshot(portfolio.AccountName, portfolio.TimeStamp, portfolio.SnapshotType.ToString(), deltaPercent, faceValuePutsPercent, (double)portfolio.AccountData.PutsPctTarget, returnOnEquity, !isViolation, xml);
            }
            catch (Exception ex)
            {
                ReportError("Error saving snapshot", ex);
            }
        }

        public void SwitchQuoteFeed(string hostName)
        {
            if (m_host.QuoteServerHost != hostName)
            {
                m_host.QuoteServerHost = hostName;
                m_host.StopQuoteFeed();

                // no need to start - will restart automatically
            }
        }
        #endregion

        #region Helper methods
        private Portfolio BuildPortfolio(AccountPortfolio positionMonitorPortfolio, bool bPositionsForAllAccounts)
        {
            try
            {
                List<Index> indices = new List<Index>();
                BuildIndexList(positionMonitorPortfolio.Indices, indices);

                Index totalRow = new Index();
                totalRow.Symbol = "Total";

                Index benchmark = BuildBenchmark(positionMonitorPortfolio.Benchmark);

                int unsubscribedSymbols = 0;

                AccountData accountData = BuildAccountData(positionMonitorPortfolio.Data);
                if (accountData != null)
                {
                    List<Position> positions = BuildPositions(positionMonitorPortfolio.AccountName, positionMonitorPortfolio.Portfolio, indices, totalRow, accountData, bPositionsForAllAccounts, ref unsubscribedSymbols);
                    CompleteAggregation(indices, totalRow, accountData, positionMonitorPortfolio.DividendsReceived);

                    if (!positionMonitorPortfolio.Data.TradingComplete)
                        CalculateTradingGoals(indices, totalRow, accountData);

                    indices.Add(totalRow);

                    // see if quotefeed has stopped
                    DateTime timeStamp = DateTime.Now;
                    TimeSpan? quoteServiceStoppedTime = positionMonitorPortfolio.QuoteServiceStoppedTime;
                    if (!quoteServiceStoppedTime.HasValue)
                    {
                        if (positionMonitorPortfolio.LastQuoteTime.HasValue)
                        {
                            if (timeStamp.TimeOfDay.Subtract(positionMonitorPortfolio.LastQuoteTime.Value) > s_quoteDelayThreshold)
                            {
                                quoteServiceStoppedTime = positionMonitorPortfolio.LastQuoteTime;
                            }
                        }
                    }

                    return new Portfolio()
                    {
                        AccountName = positionMonitorPortfolio.AccountName,
                        SnapshotType = SentoniServiceLib.SnapshotType.Current,
                        TimeStamp = timeStamp,
                        QuoteServiceHost = m_positionMonitor.QuoteServerHost,
                        QuoteServiceStoppedTime = quoteServiceStoppedTime,
                        UnsubscribedSymbols = unsubscribedSymbols,
                        AccountData = accountData,
                        Positions = positions.ToArray(),
                        Indices = indices.ToArray(),
                        Benchmark = benchmark,
                        Snapshots = BuildSnapshots(positionMonitorPortfolio)
                    };
                }
            }
            catch (Exception ex)
            {
                ReportError("Error building portfolio", ex);
            }
            return null;
        }

        private Snapshot[] BuildSnapshots(AccountPortfolio positionMonitorPortfolio)
        {
            List<Snapshot> snapshots = new List<Snapshot>();

            try
            {
                foreach (HugoDataSet.PortfolioSnapshotIdsRow row in positionMonitorPortfolio.SnapshotIds)
                {
                    snapshots.Add(new Snapshot()
                    {
                        SnapshotId = row.SnapshotId,
                        SnapshotType = (SnapshotType)Enum.Parse(typeof(SnapshotType), row.SnapshotType),
                        TimeStamp = row.TimeStamp
                    });
                }
            }
            catch (Exception ex)
            {
                ReportError("Error building snapshots", ex);
            }
            return snapshots.ToArray();
        }

        private AccountData BuildAccountData(HugoDataSet.AccountDataRow dataRow)
        {
            try
            {
                AccountData accountData = new SentoniServiceLib.AccountData()
                {
                    EquityType = dataRow.EquityType,
                    ClientType = dataRow.ClientType,
                    IsTest = dataRow.IsTest,
                    BaseEquity = dataRow.BaseEquity,
                    BaseCash = dataRow.BaseCash,
                    BaseDate = dataRow.BaseDate,
                    InflowsSinceBaseDate = dataRow.InflowsSinceBaseDate,
                    TodaysInflows = dataRow.TodaysInflows,
                    PandLSinceBaseDate = dataRow.PandLSinceBaseDate,
                    StartOfDayMarketValue = dataRow.MarketValue,
                    AvailableCash = dataRow.Available_Cash,
                    CurrentCash = dataRow.Available_Cash - dataRow.Cash, // for now, set to the negative of the unavailable
                    StartOfDayEquity = dataRow.StartOfDay_Equity,
                    MinDelta = dataRow.AdjustedMinDelta,
                    MaxDelta = dataRow.AdjustedMaxDelta,
                    TargetDelta = dataRow.TargetDelta,
                    Leverage = dataRow.Leverage,
                    PutsPctTarget = dataRow.PutsPctTarget,
                    PutsOutOfMoneyThreshold = dataRow.PutsOutOfMoneyThreshold,
                    Benchmark = dataRow.IsBenchmarkNull() ? null : dataRow.Benchmark,
                    TradingComplete = dataRow.TradingComplete,
                    MaximumCaks = dataRow.MaximumCaks,
                    PortfolioPercentage = dataRow.PortfolioPercentage
                };

                HugoDataSet.TradingScheduleRow tradingSchedule = m_positionMonitor.GetNextTimeSliceForAccount(dataRow.AcctName);
                if (tradingSchedule != null)
                    accountData.NextTradeTime = tradingSchedule.StartTradingTime.TimeOfDay;

                return accountData;
            }
            catch (Exception ex)
            {
                ReportError("Error building account data", ex);
                return null;
            }

        }

        private List<Position> BuildPositions(string acctName, HugoDataSet.PortfolioDataTable portfolio, List<Index> indices, Index totalRow, AccountData accountData, bool bPositionsForAllAccounts, ref int unsubscribedSymbols)
        {
            List<Position> positions = new List<Position>();

            try
            {
                unsubscribedSymbols += BuildPositionsForAccount(portfolio, indices, totalRow, accountData, positions);

                if (bPositionsForAllAccounts)
                {
                    foreach (AccountPortfolio account in m_positionMonitor.GetAllAccountPortfolios())
                    {
                        if (account.AccountName != acctName)
                        {
                            unsubscribedSymbols += BuildPositionsForAccount(account.Portfolio, null, null, null, positions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportError("Error building positions", ex);
            }
            return positions;
        }

        private int BuildPositionsForAccount(HugoDataSet.PortfolioDataTable portfolio, List<Index> indices, Index totalRow, AccountData accountData, List<Position> positions)
        {
            int unsubscribedSymbols = 0;

            try
            {
                foreach (HugoDataSet.PortfolioRow portfolioRow in portfolio)
                {
                    Position newRow = new Position();
                    Index indexRow = null;
                    double underlyingPrice = 0;

                    if (!portfolioRow.IsUnderlyingSymbolNull())
                    {
                        if (indices != null)
                        {
                            indexRow = GetIndexData(indices, portfolioRow.UnderlyingSymbol);
                        }
                        if (indexRow != null)
                        {
                            underlyingPrice = indexRow.LastPrice.HasValue ? indexRow.LastPrice.Value : 0;
                        }
                        else
                        {
                            HugoDataSet.PortfolioRow underlyingRow = portfolio.FindByAcctNameSymbol(portfolioRow.AcctName, portfolioRow.UnderlyingSymbol);
                            if (underlyingRow != null)
                                underlyingPrice = underlyingRow.CurrentPrice;
                        }
                    }

                    BuildPositionRow(portfolioRow, accountData, newRow, underlyingPrice);

                    // accountData is null only if we are not interested in calculating totals for this account
                    if (accountData != null)
                    {
                        AggregateAccountTotals(accountData, newRow, underlyingPrice);

                        if (newRow.IsOption)
                            AggregateIndexTotals(newRow, indexRow, totalRow);
                    }

                    if ((newRow.SubscriptionStatus != "Subscribed") && ((newRow.CurrentPosition != 0) || (newRow.SODPosition != 0) || newRow.IsStock))
                        unsubscribedSymbols++;
                    newRow.UpdateTime = portfolioRow.IsUpdateTimeNull() ? new System.Nullable<TimeSpan>() : portfolioRow.UpdateTime;

                    positions.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                ReportError("Error building positions for account", ex);
            }

            return unsubscribedSymbols;
        }

        private void BuildPositionRow(HugoDataSet.PortfolioRow portfolioRow, AccountData accountData, Position newRow, double underlyingPrice)
        {
            int lineNumber = 0;

            try
            {
                lineNumber++; newRow.Account = portfolioRow.AcctName;
                lineNumber++; newRow.Symbol = portfolioRow.Symbol;
                lineNumber++; newRow.SubscriptionStatus = portfolioRow.SubscriptionStatus;
                lineNumber++; newRow.SODPosition = portfolioRow.SOD_Position;
                lineNumber++; newRow.SODPrice = portfolioRow.SOD_Price; // 5
                lineNumber++; newRow.SODMarketValue = portfolioRow.SOD_Market_Value;
                lineNumber++; newRow.ChangeInPosition = portfolioRow.Change_in_Position;
                lineNumber++; newRow.ChangeInCost = portfolioRow.Change_in_Cost;
                lineNumber++; newRow.CurrentPosition = portfolioRow.Current_Position;
                lineNumber++; newRow.NettingAdjustment = portfolioRow.Netting_Adjustment;  // 10
                lineNumber++; newRow.CurrentPrice = portfolioRow.CurrentPrice;
                lineNumber++; newRow.CurrentCost = portfolioRow.Current_Cost;
                lineNumber++; newRow.Closed = portfolioRow.Closed;
                lineNumber++; newRow.OptionType = portfolioRow.IsOptionTypeNull() ? null : portfolioRow.OptionType;
                lineNumber++; newRow.UnderlyingSymbol = portfolioRow.IsUnderlyingSymbolNull() ? null : portfolioRow.UnderlyingSymbol;  // 15
                lineNumber++; newRow.Multiplier = portfolioRow.Multiplier;
                lineNumber++; newRow.IsStock = portfolioRow.IsStock == 1;
                lineNumber++; newRow.IsOption = portfolioRow.IsOption == 1;
                lineNumber++; newRow.IsFuture = portfolioRow.IsFuture == 1;
                lineNumber++; newRow.Open = portfolioRow.IsOpenNull() ? new System.Nullable<double>() : portfolioRow.Open;  // 20
                lineNumber++; newRow.PrevClose = portfolioRow.IsPrevCloseNull() ? new System.Nullable<double>() : portfolioRow.PrevClose;
                lineNumber++; newRow.LastPrice = portfolioRow.IsLastPriceNull() ? new System.Nullable<double>() : portfolioRow.LastPrice;
                lineNumber++; newRow.Bid = portfolioRow.IsBidNull() ? new System.Nullable<double>() : portfolioRow.Bid;
                lineNumber++; newRow.Ask = portfolioRow.IsAskNull() ? new System.Nullable<double>() : portfolioRow.Ask;
                lineNumber++; newRow.ClosingPrice = portfolioRow.IsClosingPriceNull() ? new System.Nullable<double>() : portfolioRow.ClosingPrice;  // 25
                // must use portfolioRow.PriceMultiplier for P&L purposes
                // newRow.Multiplier has been adjusted to reflect deltas on the associated index
                lineNumber++; newRow.CurrentMarketValue = portfolioRow.CurrentPrice * newRow.CurrentPosition * portfolioRow.PriceMultiplier;
                lineNumber++; newRow.PandL = newRow.CurrentMarketValue - newRow.CurrentCost;
                lineNumber++; newRow.ExpirationDate = portfolioRow.IsExpirationDateNull() ? new System.Nullable<DateTime>() : portfolioRow.ExpirationDate;

                lineNumber++; 
                if (newRow.Bid.HasValue && newRow.Ask.HasValue)
                {
                    lineNumber++; newRow.Mid = (newRow.Bid + newRow.Ask) / 2.0;  // 30
                }
                else
                {
                    lineNumber++; newRow.Mid = 0;
                }

                lineNumber++; 
                if (newRow.IsOption)
                {
                    lineNumber++; newRow._100DeltaUSD = (newRow.CurrentPosition + newRow.NettingAdjustment) * newRow.Multiplier * underlyingPrice;
                    lineNumber++; newRow.Delta = portfolioRow.IsDeltaNull() ? new System.Nullable<double>() : portfolioRow.Delta;
                    lineNumber++; newRow.DeltaUSD = portfolioRow.IsDeltaNull() ? new System.Nullable<double>() : portfolioRow.Delta * (newRow.CurrentPosition + newRow.NettingAdjustment) * newRow.Multiplier * underlyingPrice;
                    lineNumber++; newRow.Gamma = portfolioRow.IsGammaNull() ? new System.Nullable<double>() : portfolioRow.Gamma;  // 35
                    lineNumber++; newRow.GammaUSD = portfolioRow.IsGammaNull() ? new System.Nullable<double>() : newRow.Gamma * (newRow.CurrentPosition + newRow.NettingAdjustment) * newRow.Multiplier * underlyingPrice;
                    lineNumber++; newRow.Theta = portfolioRow.IsThetaNull() ? new System.Nullable<double>() : portfolioRow.Theta;
                    lineNumber++; newRow.ThetaAnnualized = portfolioRow.IsThetaNull() ? new System.Nullable<double>() : newRow.Theta * (newRow.CurrentPosition + newRow.NettingAdjustment) * portfolioRow.PriceMultiplier * 360;

                    lineNumber++; newRow.Vega = portfolioRow.IsVegaNull() ? new System.Nullable<double>() : portfolioRow.Vega;
                    lineNumber++; newRow.ImpliedVol = portfolioRow.IsImpliedVolNull() ? new System.Nullable<double>() : portfolioRow.ImpliedVol;  // 40
                    lineNumber++; newRow.StrikePrice = portfolioRow.IsStrikePriceNull() ? new System.Nullable<decimal>() : portfolioRow.StrikePrice;

                    lineNumber++;
                    if (newRow.OptionType == "Call")
                    {
                        lineNumber++; newRow.TimePremium = Math.Max(0, newRow.Mid.Value - Math.Max(0, underlyingPrice * newRow.Multiplier / 100 - (double)newRow.StrikePrice.Value)) * (newRow.CurrentPosition + newRow.NettingAdjustment) * portfolioRow.PriceMultiplier;
                    }
                    else
                    {
                        lineNumber++; newRow.TimePremium = Math.Max(0, newRow.Mid.Value - Math.Max(0, (double)newRow.StrikePrice.Value - underlyingPrice * newRow.Multiplier / 100)) * (newRow.CurrentPosition + newRow.NettingAdjustment) * portfolioRow.PriceMultiplier;
                        lineNumber++;
                        if (accountData != null)
                        {
                            if (((newRow.CurrentPosition + newRow.NettingAdjustment) > 0) && (newRow.Multiplier > 0) && newRow.StrikePrice.HasValue && (accountData.PutsOutOfMoneyThreshold < 1))
                            {
                                lineNumber++; // 45
                                if ((double)newRow.StrikePrice.Value * 100 / newRow.Multiplier < (1 - accountData.PutsOutOfMoneyThreshold) * underlyingPrice)
                                {
                                    lineNumber++; newRow.IsOutOfBounds = true;
                                }
                                else if ((double)newRow.StrikePrice.Value * 100 / newRow.Multiplier > (1 + accountData.PutsOutOfMoneyThreshold) * underlyingPrice)
                                {
                                    lineNumber++; newRow.IsOutOfBounds = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    ReportError(String.Format("Error building position row, Account={0}, Symbol={1}. line number={2}", portfolioRow.AcctName, portfolioRow.Symbol, lineNumber),  ex);
                }
                catch
                {
                    ReportError("Error building position row", ex);
                }
            }

        }

        private void AggregateIndexTotals(Position newRow, Index indexRow, Index totalRow)
        {
            try
            {
                if (newRow.OptionType == "Call")
                {
                    if (indexRow != null)
                    {
                        indexRow.CallDeltaPct += newRow.DeltaUSD.HasValue ? newRow.DeltaUSD.Value : 0;
                        indexRow.Caks += newRow._100DeltaUSD.HasValue ? newRow._100DeltaUSD.Value : 0;
                    }
                    totalRow.CallDeltaPct += newRow.DeltaUSD.HasValue ? newRow.DeltaUSD.Value : 0;
                    totalRow.Caks += newRow._100DeltaUSD.HasValue ? newRow._100DeltaUSD.Value : 0;
                }
                else
                {
                    if (indexRow != null)
                    {
                        indexRow.PutDeltaPct += newRow.DeltaUSD.HasValue ? newRow.DeltaUSD.Value : 0;
                    }
                    totalRow.PutDeltaPct += newRow.DeltaUSD.HasValue ? newRow.DeltaUSD.Value : 0;

                    if (newRow.ExpirationDate.HasValue)
                    {
                        if (newRow.ExpirationDate > DateTime.Now)
                        {
                            if (indexRow != null)
                            {
                                indexRow.FaceValuePutsPct += newRow._100DeltaUSD.HasValue ? newRow._100DeltaUSD.Value : 0;
                            }
                            totalRow.FaceValuePutsPct += newRow._100DeltaUSD.HasValue ? newRow._100DeltaUSD.Value : 0;
                        }
                    }
                }

                if (indexRow != null)
                {
                    indexRow.GammaPct += newRow.GammaUSD.HasValue ? newRow.GammaUSD.Value : 0;
                    indexRow.ThetaAnnualized += newRow.ThetaAnnualized.HasValue ? newRow.ThetaAnnualized.Value : 0;
                    indexRow.TotalDeltaPct += newRow.DeltaUSD.HasValue ? newRow.DeltaUSD.Value : 0;
                    indexRow.ShortPct += newRow.CurrentMarketValue;
                    indexRow.TimePremium += newRow.TimePremium.HasValue ? newRow.TimePremium.Value : 0;
                }

                totalRow.GammaPct += newRow.GammaUSD.HasValue ? newRow.GammaUSD.Value : 0;
                totalRow.ThetaAnnualized += newRow.ThetaAnnualized.HasValue ? newRow.ThetaAnnualized.Value : 0;
                totalRow.TotalDeltaPct += newRow.DeltaUSD.HasValue ? newRow.DeltaUSD.Value : 0;
                totalRow.ShortPct += newRow.CurrentMarketValue;
                totalRow.TimePremium += newRow.TimePremium.HasValue ? newRow.TimePremium.Value : 0;
            }
            catch (Exception ex)
            {
                ReportError("Error aggregating index totals", ex);
            }
        }

        private void AggregateAccountTotals(AccountData accountData, Position newRow, double underlyingPrice)
        {
            try
            {
                if (newRow.IsOption)
                {
                    accountData.OptionPandL += newRow.PandL;
                    if (newRow.Delta.HasValue && (underlyingPrice != 0))
                        accountData.DollarDeltasTraded += newRow.Delta.Value * newRow.ChangeInPosition * newRow.Multiplier * underlyingPrice;
                    if (newRow.OptionType == "Put")
                    {
                        accountData.PutsTraded += newRow.ChangeInPosition;
                        accountData.PutsOutOfBounds |= newRow.IsOutOfBounds;
                        if (newRow.CurrentPosition < 0)
                            // short puts are treated as synthetic stock (long calls are assumed)
                            accountData.CurrentLeverage -= newRow.CurrentPosition * newRow.Multiplier * underlyingPrice;
                    }
                }
                else if (newRow.IsStock)
                {
                    accountData.StockPandL += newRow.PandL;
                    accountData.CurrentOLAPMarketValue += newRow.CurrentMarketValue;
                }
                else if (newRow.IsFuture)
                {
                    accountData.FuturesPandL += newRow.PandL;

                    // if possible, use value of index, not the future itself, in calculating the OLAP Market Value
                    if (underlyingPrice == 0)
                    {
                        accountData.CurrentOLAPMarketValue += newRow.CurrentPosition + newRow.NettingAdjustment;
                    }
                    else
                    {
                        accountData.CurrentOLAPMarketValue += (newRow.CurrentPosition + newRow.NettingAdjustment) * newRow.Multiplier * underlyingPrice;
                    }
                }
                accountData.TotalPandL += newRow.PandL;
                accountData.CurrentMarketValue += newRow.CurrentMarketValue;
            }
            catch (Exception ex)
            {
                ReportError("Error aggregating account totals", ex);
            }
        }

        private void CompleteAggregation(List<Index> indices, Index totalRow, AccountData accountData, double dividendsReceived)
        {
            try
            {
                accountData.CurrentEquity = accountData.StartOfDayEquity + accountData.TotalPandL;

                // CurrentLeverage has synthetic stock due to short puts - add OLAP then calculate leverage
                accountData.CurrentLeverage += accountData.CurrentOLAPMarketValue;
                accountData.CurrentLeverage /= accountData.CurrentEquity;
                
                // CurrentCash is currently set to the negative of the unavailable cash
                accountData.CurrentCash += (accountData.CurrentEquity - dividendsReceived) * (double)accountData.Leverage - accountData.CurrentMarketValue;

                // calculate PutsPctTarget - this used to be a fixed number. it is now calculated as below
                accountData.PutsPctTarget = (decimal)Math.Round(Math.Max(0, (accountData.CurrentMarketValue - accountData.CurrentEquity) / accountData.CurrentEquity), 3);

                totalRow.LastPrice = totalRow.PrevClose = 100;
                foreach (Index indexRow in indices)
                {
                    indexRow.TargetValue = (double)indexRow.Weight * DeltaDivisor(accountData);
                    if (indexRow.TargetValue > 0)
                    {
                        indexRow.CallDeltaPct /= indexRow.TargetValue;
                        indexRow.PutDeltaPct /= indexRow.TargetValue;
                        indexRow.TotalDeltaPct = ((double)indexRow.Weight * accountData.CurrentOLAPMarketValue + indexRow.TotalDeltaPct) / indexRow.TargetValue;
                        indexRow.GammaPct = indexRow.GammaPct / indexRow.TargetValue;
                        indexRow.FaceValuePutsPct /= indexRow.TargetValue;
                        indexRow.ShortPct /= indexRow.TargetValue;
                        indexRow.TimePremium /= indexRow.TargetValue;
                        indexRow.Caks /= indexRow.TargetValue;

                        if (indexRow.LastPrice > 0)
                        {
                            indexRow.PutsToRebalance = (int)Math.Round(((double)accountData.PutsPctTarget - indexRow.FaceValuePutsPct) * indexRow.TargetValue / (100 * indexRow.LastPrice.Value), 0);
                        }

                    }
                    if (indexRow.PrevClose != 0)
                        totalRow.LastPrice += (double)indexRow.Weight * (indexRow.LastPrice / indexRow.PrevClose - 1) * totalRow.PrevClose;
                }

                double deltaDivisor = DeltaDivisor(accountData);
                if (deltaDivisor > 0)
                {
                    totalRow.CallDeltaPct /= deltaDivisor;
                    totalRow.PutDeltaPct /= deltaDivisor;
                    totalRow.TotalDeltaPct = (accountData.CurrentOLAPMarketValue + totalRow.TotalDeltaPct) / deltaDivisor;
                    totalRow.GammaPct = totalRow.GammaPct / deltaDivisor;
                    totalRow.FaceValuePutsPct /= deltaDivisor;
                    totalRow.ShortPct /= deltaDivisor;
                    totalRow.TimePremium /= deltaDivisor;
                    totalRow.Caks /= deltaDivisor;
                    totalRow.ThetaAnnualized /= deltaDivisor;
                    accountData.DeltaPctTraded = accountData.DollarDeltasTraded / deltaDivisor;
                }
            }
            catch (Exception ex)
            {
                ReportError("Error completing aggregation of account totals", ex);
            }
        }

        private void CalculateTradingGoals(List<Index> indices, Index totalRow, AccountData accountData)
        {
            try
            {
                // see if we need to trade deltas
                if ((totalRow.TotalDeltaPct > accountData.MaxDelta) || (totalRow.TotalDeltaPct < accountData.MinDelta))
                {
                    bool buy;
                    if (totalRow.TotalDeltaPct > accountData.MaxDelta)
                    {
                        buy = false;
                        accountData.DeltaGoal = Math.Max(accountData.MaxDelta - .05, accountData.TargetDelta);
                    }
                    else
                    {
                        buy = true;
                        accountData.DeltaGoal = Math.Min(accountData.MinDelta + .05, accountData.TargetDelta);
                    }

                    AdjustDeltaSandbox[] sandBox = CalculateDeltasToTrade(buy, accountData.DeltaGoal, indices);
                    int index = 0;
                    foreach (AdjustDeltaSandbox sandBoxRow in sandBox)
                    {
                        indices[index++].DeltasToTrade = sandBoxRow.deltasToTrade;
                    }
                }

                // see if we need to trade puts
                if ((totalRow.FaceValuePutsPct < (double)accountData.PutsPctTarget))
                {
                    totalRow.PutsToTrade = CalculatePutsToTrade(((double)accountData.PutsPctTarget - totalRow.FaceValuePutsPct) * accountData.CurrentEquity, indices);
                }
            }
            catch (Exception ex)
            {
                ReportError("Error calculating trading goals", ex);
            }
        }
        private void BuildIndexList(HugoDataSet.IndicesRow[] indicesTable, List<Index> indices)
        {
            foreach (HugoDataSet.IndicesRow indicesRow in indicesTable)
            {
                try
                {
                    indices.Add(new Index()
                    {
                        Symbol = indicesRow.Symbol,
                        Weight = indicesRow.Weight,
                        Open = indicesRow.IsOpenNull() ? 0.0 : indicesRow.Open,
                        PrevClose = indicesRow.IsPrevCloseNull() ? 0.0 : indicesRow.PrevClose,
                        LastPrice = indicesRow.IsLastPriceNull() ? 0.0 : indicesRow.CurrentPrice, // this will use ClosingPrice rather than Last if we are closed
                        UpdateTime = indicesRow.IsUpdateTimeNull() ? new System.Nullable<TimeSpan>() : indicesRow.UpdateTime,
                    });
                }
                catch (Exception ex)
                {
                    ReportError("Error building index list", ex);
                }
            }
        }

        private Index BuildBenchmark(HugoDataSet.IndicesRow benchmark)
        {
            try
            {
                if (benchmark != null)
                    return new Index()
                   {
                       Symbol = benchmark.Symbol,
                       Open = benchmark.IsOpenNull() ? 0.0 : benchmark.Open,
                       PrevClose = benchmark.IsPrevCloseNull() ? 0.0 : benchmark.PrevClose,
                       LastPrice = benchmark.IsLastPriceNull() ? 0.0 : benchmark.CurrentPrice, // this will use ClosingPrice rather than Last if we are closed
                       UpdateTime = benchmark.IsUpdateTimeNull() ? new System.Nullable<TimeSpan>() : benchmark.UpdateTime,
                   };
            }
            catch (Exception ex)
            {
                ReportError("Error building benchmark", ex);
            }
            return null;
        }

        private static Index GetIndexData(List<Index> indices, string underlyingSymbol)
        {
            foreach (Index row in indices)
            {
                if (row.Symbol == underlyingSymbol)
                    return row;
            }

            return null;
        }

        // added 2018-01-01 - for Basket clients, use CurrentOLAPMarketValue instead of CurrentEquity in calculating Greek percentages
        private static double DeltaDivisor(AccountData accountData)
        {
            if (accountData.ClientType == "Basket")
                return accountData.CurrentOLAPMarketValue;
            else
                return accountData.CurrentEquity;
        }
        #endregion

        #region Trading Calculations
        // Public so they can be called by DeltasToTradeTester
        public static AdjustDeltaSandbox[] CalculateDeltasToTrade(bool buy, double deltaGoal, List<Index> indices)
        {
            int numberOfIndices = indices.Count;
            AdjustDeltaSandbox[] sandBox = new AdjustDeltaSandbox[numberOfIndices];

            int buyOrSell = buy ? 1 : -1;
            double adjustedDeltaGoal = deltaGoal;
            bool solutionFound = false;

            // first guess of which indices to skip (those beyond the target)
            for (int index = 0; index < numberOfIndices; index++)
            {
                sandBox[index].targetDeltaPercent = indices[index].TotalDeltaPct;
                if (buyOrSell * indices[index].TotalDeltaPct >= buyOrSell * adjustedDeltaGoal)
                {
                    sandBox[index].skipIndex = true;
                }
                else
                {
                    sandBox[index].skipIndex = false;
                }
            }

            while (!solutionFound)
            {
                solutionFound = true;
                double overshotPercentage = 0;
                double overshotDeltas = 0;

                for (int index = 0; index < numberOfIndices; index++)
                {
                    sandBox[index].targetDeltaPercent = indices[index].TotalDeltaPct;
                    if (sandBox[index].skipIndex)
                    {
                        overshotPercentage += (double)indices[index].Weight;
                        overshotDeltas += (double)indices[index].Weight * indices[index].TotalDeltaPct;
                    }
                }

                // shouldn't happen, but just to avoid division by 0
                if (overshotPercentage >= 1)
                    return sandBox;

                adjustedDeltaGoal = (deltaGoal - overshotDeltas) / (1 - overshotPercentage);
                for (int index = 0; index < numberOfIndices; index++)
                {
                    if (!sandBox[index].skipIndex)
                    {
                        sandBox[index].targetDeltaPercent = adjustedDeltaGoal;
                        sandBox[index].dollarDeltasToTrade = (adjustedDeltaGoal - indices[index].TotalDeltaPct) * indices[index].TargetValue;

                        // we if end up buying an index when we are supposed to be selling (or vice versa), mark this index to skip and re-solve
                        if (sandBox[index].dollarDeltasToTrade * buyOrSell < 0)
                        {
                            sandBox[index].skipIndex = true;
                            solutionFound = false;
                            break;
                        }

                        if (indices[index].LastPrice.HasValue)
                        {
                            if (indices[index].LastPrice.Value > 0)
                            {
                                sandBox[index].deltasToTrade = sandBox[index].dollarDeltasToTrade / indices[index].LastPrice.Value;
                            }
                        }
                    }
                }
            }

            return sandBox;
        }

        public static int CalculatePutsToTrade(double faceValuePutsToBuy, List<Index> inputIndices)
        {
            List<Index> indices = new List<Index>(inputIndices);
            indices.Sort(new IndexComparerFaceValuePutsPct());
            int putsToTrade = 0;

            foreach (Index index in indices)
            {
                if ((index.LastPrice.Value > 0) && (index.PutsToRebalance > 0))
                {
                    int thisIndexPutsToTrade = (int)Math.Round(faceValuePutsToBuy / (100 * index.LastPrice.Value), 0);
                    if (thisIndexPutsToTrade <= index.PutsToRebalance)
                    {
                        index.PutsToTrade = thisIndexPutsToTrade;
                        putsToTrade += thisIndexPutsToTrade;
                        return putsToTrade;
                    }
                    else
                    {
                        index.PutsToTrade = index.PutsToRebalance;
                        putsToTrade += index.PutsToRebalance;
                        faceValuePutsToBuy -= index.PutsToTrade * 100 * index.LastPrice.Value;
                    }
                }
            }


            return putsToTrade;
        }

 
        #endregion

        #region ISnapshotProvider Members

        public DateTime EndOfDay
        {
            get { return m_positionMonitor.EndOfDay.Value; }
        }

        public void TakeSnapshot(string acctName, string snapshotType)
        {
            try
            {
                m_host.OnInfo(String.Format("Taking {0} snapshot for {1}", snapshotType, acctName));

                Portfolio portfolio = ProvidePortfolio(acctName, false);
                portfolio.SnapshotType = (SnapshotType)Enum.Parse(typeof(SnapshotType), snapshotType);

                if (portfolio != null)
                {
                    SavePortfolioSnapshot(portfolio, SentoniService.PortfolioToXml(portfolio));
                }

            }
            catch (Exception ex)
            {
                ReportError(String.Format("Unable to take {0} snapsshot for {1}", snapshotType, acctName), ex);
            }
        }

        #endregion
    }
}
