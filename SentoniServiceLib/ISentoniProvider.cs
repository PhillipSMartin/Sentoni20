using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniServiceLib
{
    public interface ISentoniProvider
    {
        bool ProvideHeartBeat();
        string[] ProvideAccountList();
        AccountSummary[] ProvideAccountSummaries();
        Portfolio ProvidePortfolio(string acctName, bool bPositionsForAllAccounts);
        string ProvidePortfolioSnapshot(short snapshotId);
        Blotter ProvideBlotter(string acctName, DateTime tradeDate);
        TradingSchedule[] ProvideTradingSchedule();
        void SwitchQuoteFeed(string hostName);

        void SavePortfolioSnapshot(Portfolio portfolio, string xml);
        void ReportError(string msg, Exception ex);
    }
}
