using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SentoniServiceLib
{
    [ServiceContract(CallbackContract = typeof(ISentoniServiceCallback))]
    public interface ISentoniService
    {
        [OperationContract]
        bool HeartBeat();

        [OperationContract]
        string[] GetAccountList();

        [OperationContract(IsOneWay = true)]
        void GetAccountSummaries();

        [OperationContract(IsOneWay = true)]
        void GetPortfolio(string acctName, bool bPositionsForAllAccounts);

        [OperationContract(IsOneWay = true)]
        void GetPortfolioSnapshot(short snapshotId);

        [OperationContract(IsOneWay = true)]
        void GetBlotter(string acctName, DateTime tradeDate);

        [OperationContract(IsOneWay = true)]
        void GetTradingSchedule();

        [OperationContract(IsOneWay = true)]
        void TakeSnapshot(string acctName);

        [OperationContract(IsOneWay = true)]
        void SwitchQuoteFeed(string hostName);

    }

    public interface ISentoniServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void PostAccountSummaries(AccountSummary[] accountSummaries);

        [OperationContract(IsOneWay = true)]
        void PostPortfolio(Portfolio portfolio);

        [OperationContract(IsOneWay = true)]
        void PostBlotter(Blotter blotter);

        [OperationContract(IsOneWay = true)]
        void PostTradingSchedule(TradingSchedule[] tradingSchedule);
    }

    [DataContract]
    public class AccountData
    {
        [DataMember]
        public string EquityType { get; set; }
        [DataMember]
        public string ClientType { get; set; }
        [DataMember]
        public string Benchmark { get; set; }
        [DataMember]
        public bool IsTest { get; set; }
        [DataMember]
        public double BaseEquity { get; set; }
        [DataMember]
        public double BaseCash { get; set; }
        [DataMember]
        public DateTime BaseDate { get; set; }
        [DataMember]
        public double InflowsSinceBaseDate { get; set; }
        [DataMember]
        public double TodaysInflows { get; set; }
        [DataMember]
        public double PandLSinceBaseDate { get; set; }
        [DataMember]
        public double StartOfDayMarketValue { get; set; }
        [DataMember]
        public double StartOfDayEquity { get; set; }
        [DataMember]
        public double AvailableCash { get; set; }
        [DataMember]
        public double CurrentMarketValue { get; set; }
        [DataMember]
        public double CurrentOLAPMarketValue { get; set; }
        [DataMember]
        public double CurrentEquity { get; set; }
        [DataMember]
        public double CurrentCash { get; set; }
        [DataMember]
        public double MinDelta { get; set; }
        [DataMember]
        public double MaxDelta { get; set; }
        [DataMember]
        public double TargetDelta { get; set; }
        [DataMember]
        public double DeltaGoal { get; set; }
        [DataMember]
        public decimal Leverage { get; set; }
        [DataMember]
        public decimal PutsPctTarget { get; set; }
        [DataMember]
        public TimeSpan? NextTradeTime { get; set; }
        [DataMember]
        public double OptionPandL { get; set; }
        [DataMember]
        public double StockPandL { get; set; }
        [DataMember]
        public double FuturesPandL { get; set; }
        [DataMember]
        public double TotalPandL { get; set; }
        [DataMember]
        public double DollarDeltasTraded { get; set; }
        [DataMember]
        public double DeltaPctTraded { get; set; }
        [DataMember]
        public int PutsTraded { get; set; }
        [DataMember]
        public bool PutsOutOfBounds { get; set; }
        [DataMember]
        public double PutsOutOfMoneyThreshold { get; set; }
        [DataMember]
        public bool TradingComplete { get; set; }
        [DataMember]
        public double MaximumCaks { get; set; }
        [DataMember]
        public double PortfolioPercentage { get; set; }
        [DataMember]
        public double CurrentLeverage { get; set; }
    }

    [DataContract]
    public class Trade
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public decimal? StrikePrice { get; set; }
        [DataMember]
        public string OptionType { get; set; }
        [DataMember]
        public string TradeType { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public double Cost { get; set; }
        [DataMember]
        public string UnderlyingSymbol { get; set; }
        [DataMember]
        public short Multiplier { get; set; }
        [DataMember]
        public bool IsStock { get; set; }
        [DataMember]
        public bool IsOption { get; set; }
        [DataMember]
        public bool IsFuture { get; set; }
        [DataMember]
        public DateTime TradeDateTime { get; set; }
        [DataMember]
        public string TradeSource { get; set; }
        [DataMember]
        public int TradeId { get; set; }
        [DataMember]
        public double CurrentMarketPrice { get; set; }
        [DataMember]
        public double PandL { get; set; }
    }

    [DataContract]
    public class Position
    {
        [DataMember]
        public string Account { get; set; }
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public int SODPosition { get; set; }
        [DataMember]
        public double SODPrice { get; set; }
        [DataMember]
        public double SODMarketValue { get; set; }
        [DataMember]
        public int ChangeInPosition { get; set; }
        [DataMember]
        public double ChangeInCost { get; set; }
        [DataMember]
        public int CurrentPosition { get; set; }
        [DataMember]
        public int NettingAdjustment { get; set; }
        [DataMember]
        public double CurrentCost { get; set; }
        [DataMember]
        public double CurrentPrice { get; set; }
        [DataMember]
        public double CurrentMarketValue { get; set; }
        [DataMember]
        public double PandL { get; set; }
        [DataMember]
        public double? Open { get; set; }
        [DataMember]
        public double? PrevClose { get; set; }
        [DataMember]
        public double? LastPrice { get; set; }
        [DataMember]
        public double? Bid { get; set; }
        [DataMember]
        public double? Ask { get; set; }
        [DataMember]
        public double? Mid { get; set; }
        [DataMember]
        public bool Closed { get; set; }
        [DataMember]
        public double? ClosingPrice { get; set; }
        [DataMember]
        public double? Delta { get; set; }
        [DataMember]
        public double? Gamma { get; set; }
        [DataMember]
        public double? Theta { get; set; }
        [DataMember]
        public double? Vega { get; set; }
        [DataMember]
        public double? ImpliedVol { get; set; }
        [DataMember]
        public double? DeltaUSD { get; set; }
        [DataMember]
        public double? GammaUSD { get; set; }
        [DataMember]
        public double? _100DeltaUSD { get; set; }
        [DataMember]
        public double? ThetaAnnualized { get; set; }
        [DataMember]
        public double? TimePremium { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public decimal? StrikePrice { get; set; }
        [DataMember]
        public string OptionType { get; set; }
        [DataMember]
        public string UnderlyingSymbol { get; set; }
        [DataMember]
        public short Multiplier { get; set; }
        [DataMember]
        public bool IsStock { get; set; }
        [DataMember]
        public bool IsOption { get; set; }
        [DataMember]
        public bool IsFuture { get; set; }
        [DataMember]
        public bool IsOutOfBounds { get; set; }
        [DataMember]
        public string SubscriptionStatus { get; set; }
        [DataMember]
        public TimeSpan? UpdateTime { get; set; }
    }

    // any new columns must go at the end of the class unless we modify SentoniClient accordingly
    [DataContract]
    public class Index
    {
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public decimal Weight { get; set; }
        [DataMember]
        public double? Open { get; set; }
        [DataMember]
        public double? PrevClose { get; set; }
        [DataMember]
        public double? LastPrice { get; set; }
        [DataMember]
        public double TargetValue { get; set; }
        [DataMember]
        public double TotalDeltaPct { get; set; }
        [DataMember]
        public double CallDeltaPct { get; set; }
        [DataMember]
        public double PutDeltaPct { get; set; }
        [DataMember]
        public double FaceValuePutsPct { get; set; }
        [DataMember]
        public double ShortPct { get; set; }
        [DataMember]
        public double TimePremium { get; set; }
        [DataMember]
        public double Caks { get; set; }
        [DataMember]
        public double GammaPct { get; set; }
        [DataMember]
        public double ThetaAnnualized { get; set; }
        [DataMember]
        public double DeltasToTrade { get; set; }
        [DataMember]
        public int PutsToRebalance { get; set; }
        [DataMember]
        public int PutsToTrade { get; set; }
        [DataMember]
        public TimeSpan? UpdateTime { get; set; }
    }

    [DataContract]
    public enum SnapshotType
    {
        [EnumMember]
        Current,
        [EnumMember]
        StartOfTrading,
        [EnumMember]
        EndOfTrading,
        [EnumMember]
        EndOfDay,
        [EnumMember]
        User
    }

    [DataContract]
    public class Snapshot
    {
        [DataMember]
        public int SnapshotId { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public SnapshotType SnapshotType { get; set; }
    }

    [DataContract]
    public class Portfolio
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public SnapshotType SnapshotType { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public TimeSpan? QuoteServiceStoppedTime { get; set; }
        [DataMember]
        public string QuoteServiceHost { get; set; }
        [DataMember]
        public int UnsubscribedSymbols { get; set; }
        [DataMember]
        public AccountData AccountData { get; set; }
        [DataMember]
        public Position[] Positions { get; set; }
        [DataMember]
        public Index[] Indices { get; set; }
        [DataMember]
        public Index Benchmark { get; set; }
        [DataMember]
        public Snapshot[] Snapshots { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class Blotter
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public DateTime TradeDate { get; set; }
        [DataMember]
        public Trade[] Trades { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class AccountSummary
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public double MinDelta { get; set; }
        [DataMember]
        public double MaxDelta { get; set; }
        [DataMember]
        public double TargetDelta { get; set; }
        [DataMember]
        public double TotalDeltaPct { get; set; }
        [DataMember]
        public int PutsToTrade { get; set; }
        [DataMember]
        public TimeSpan? NextTradeTime { get; set; }
        [DataMember]
        public double OptionPandL { get; set; }
        [DataMember]
        public double StockPandL { get; set; }
        [DataMember]
        public double FuturesPandL { get; set; }
        [DataMember]
        public double TotalPandL { get; set; }
        [DataMember]
        public double ReturnOnEquity { get; set; }
        [DataMember]
        public string Benchmark { get; set; }
        [DataMember]
        public double? BenchmarkReturn { get; set; }
        [DataMember]
        public double? TargetReturn { get; set; }
        [DataMember]
        public bool PutsOutOfBounds { get; set; }
        [DataMember]
        public bool TradingComplete { get; set; }
        [DataMember]
        public double GrossReturnMTD { get; set; }
        [DataMember]
        public double NetReturnMTD { get; set; }
        [DataMember]
        public double? BenchmarkReturnMTD { get; set; }
    }

    [DataContract]
    public class TradingSchedule
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public TimeSpan NextTradeTime { get; set; }
        [DataMember]
        public TimeSpan EndTradeTime { get; set; }
        [DataMember]
        public int MinutesToTrade { get; set; }
        [DataMember]
        public bool StartOfTradingSnapshotTaken { get; set; }
        [DataMember]
        public bool EndOfTradingSnapshotTaken { get; set; }
        [DataMember]
        public bool EndOfDaySnapshotTaken { get; set; }
    }
}

