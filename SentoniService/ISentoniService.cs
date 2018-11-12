using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SentoniService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISentoniService
    {
        [OperationContract]
        AccountSummary GetAccountSummary(string acctName);
        [OperationContract]
        AccountSummary[] GetAllAccountSummaries();
    }

    [DataContract]
    public class AccountData
    {
        [DataMember]
 	 	public string AcctName { get; set; }
        [DataMember]
        public string EquityType { get; set; }
        [DataMember]
        public string ClientType { get; set; }
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
        public double MarketValue { get; set; }
        [DataMember]
        public double Cash { get; set; }
        [DataMember]
        public double StartOfDayEquity { get; set; }
        [DataMember]
        public double MinDelta { get; set; }
        [DataMember]
        public double MaxDelta { get; set; }
        [DataMember]
        public double TargetDelta { get; set; }
   }

    [DataContract]
    public class Trades
    {
        [DataMember]
        public DateTime ImportDate { get; set; }
        [DataMember]
        public string AcctName { get; set; }
        [DataMember]
        public string TradeType { get; set; }
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public DateTime ExpirationDate { get; set; }
        [DataMember]
        public decimal StrikePrice { get; set; }
        [DataMember]
        public string OptionType { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public bool IsStock { get; set; }
        [DataMember]
        public bool IsOption { get; set; }
        [DataMember]
        public bool IsFuture { get; set; }
        [DataMember]
        public int SODPosition { get; set; }
        [DataMember]
        public double SODMarketValue { get; set; }
        [DataMember]
        public int ChangeInPosition { get; set; }
        [DataMember]
        public double ChangeInCost { get; set; }
    }

    [DataContract]
    public class Portfolio
    {
        [DataMember]
        public string AcctName { get; set; }
        [DataMember]
        public string Symbol { get; set; }
        [DataMember]
        public string SubscriptionStatus { get; set; }
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
        public double CurrentCost { get; set; }
        [DataMember]
        public double LastPrice { get; set; }
        [DataMember]
        public double Bid { get; set; }
        [DataMember]
        public double Ask { get; set; }
        [DataMember]
        public bool Closed { get; set; }
        [DataMember]
        public double ClosingPrice { get; set; }
        [DataMember]
        public double Delta { get; set; }
        [DataMember]
        public double Gamma { get; set; }
        [DataMember]
        public double Theta { get; set; }
        [DataMember]
        public double Vega { get; set; }
        [DataMember]
        public double ImplliedVol { get; set; }
        [DataMember]
        public DateTime ExpirationDate { get; set; }
        [DataMember]
        public decimal StrikePrice { get; set; }
        [DataMember]
        public string OptionType { get; set; }
        [DataMember]
        public bool IsStock { get; set; }
        [DataMember]
        public bool IsOption { get; set; }
        [DataMember]
        public bool IsFuture { get; set; }
        [DataMember]
        public DateTime UpdateTime { get; set; }
      }

    [DataContract]
    public class AccountSummary
    {
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public AccountData AccountData { get; set; }
        [DataMember]
        public Trades[] Trades { get; set; }
        [DataMember]
        public Portfolio[] Portfolio { get; set; }

        internal AccountSummary(string acctName)
        {
            AccountName = acctName;
            AccountData = new AccountData()
            {
                AcctName = acctName,
                EquityType = "Monthly",
                ClientType = "Managed",
                IsTest = false,
                BaseEquity = 130692825.94,
                BaseCash = 0,
                BaseDate = new DateTime(2017, 07, 03),
                InflowsSinceBaseDate = 0,
                TodaysInflows = 0,
                PandLSinceBaseDate = 717158.190000007,
                MarketValue = 181438269.78,
                Cash = -50028285.65,
                StartOfDayEquity = 131409984.13,
                MinDelta = 0.35,	
                MaxDelta = 0.65,
                TargetDelta =  0.5      
            };
        }
    }
}
