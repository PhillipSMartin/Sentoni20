using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace SentoniServiceLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SentoniService : ISentoniService
    {
        // link to methods to retrieve data
        public static ISentoniProvider Provider { get; set; }

        #region ISentoniService Members

        public bool HeartBeat()
        {
            try
            {
                return Provider.ProvideHeartBeat();
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.HeartBeat()", ex);
                throw new FaultException(ex.Message);
            }
        }

        public string[] GetAccountList()
        {
            try
            {
                return Provider.ProvideAccountList();
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.GetAccountList()", ex);
                throw new FaultException(ex.Message);
            }
        }

        public void GetAccountSummaries()
        {
            AccountSummary[] accountSummaries = null;
            ISentoniServiceCallback callback = null;

            try
            {
                callback = OperationContext.Current.GetCallbackChannel<ISentoniServiceCallback>();
                accountSummaries = Provider.ProvideAccountSummaries();

            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.GetAccountSummaries", ex);
            }
            finally
            {
                try
                {
                    callback.PostAccountSummaries(accountSummaries);
                }
                catch (Exception ex)
                {
                    Provider.ReportError("Exception in ISentoniServiceCallback.PostAccountSummaries", ex);
                }
            }
        }

        public void GetPortfolio(string acctName, bool bPositionsForAllAccounts)
        {
            Portfolio portfolio = null;
            ISentoniServiceCallback callback = null;

            try
            {
                if (!String.IsNullOrEmpty(acctName))
                {
                    callback = OperationContext.Current.GetCallbackChannel<ISentoniServiceCallback>();
                    portfolio = Provider.ProvidePortfolio(acctName, bPositionsForAllAccounts);
                }
                if (portfolio == null)
                {
                    portfolio = new Portfolio()
                    {
                        AccountName = acctName ?? "",
                        ErrorMessage = String.Format("Account name {0} not found", acctName ?? "")
                    };
                }
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.GetPortfolio()", ex);
                portfolio = new Portfolio()
                {
                    AccountName = acctName ?? "",
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                try
                {
                    callback.PostPortfolio(portfolio);
                }
                catch (Exception ex)
                {
                    Provider.ReportError("Exception in ISentoniServiceCallback.PostPortfolio", ex);
                }
            }
        }


        public void GetPortfolioSnapshot(short snapshotId)
        {
            Portfolio portfolio = null;
            ISentoniServiceCallback callback = null;

            try
            {
                callback = OperationContext.Current.GetCallbackChannel<ISentoniServiceCallback>();
                string snapshot = Provider.ProvidePortfolioSnapshot(snapshotId);
                if (snapshot == null)
                {
                    portfolio = new Portfolio()
                    {
                        ErrorMessage = String.Format("Snapshot id {0} not found", snapshotId)
                    };
                }
                else
                {
                    portfolio = XmlToPortfolio(snapshot);
                }
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.GetPortfolioSnapshot()", ex);
                portfolio = new Portfolio()
                {
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                try
                {
                    callback.PostPortfolio(portfolio);
                }
                catch (Exception ex)
                {
                    Provider.ReportError("Exception in ISentoniServiceCallback.PostPortfolio", ex);
                }
            }
        }

        public void GetBlotter(string acctName, DateTime tradeDate)
        {
            Blotter blotter = null;
            ISentoniServiceCallback callback = null;
            tradeDate = tradeDate.Date;

            try
            {
                callback = OperationContext.Current.GetCallbackChannel<ISentoniServiceCallback>();
                blotter = Provider.ProvideBlotter(acctName, tradeDate);
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.GetBlotter()", ex);
                blotter = new Blotter()
                {
                    AccountName = acctName,
                    TradeDate = tradeDate,
                    ErrorMessage = ex.Message
                };
            }
            finally
            {
                try
                {
                    callback.PostBlotter(blotter);
                }
                catch (Exception ex)
                {
                    Provider.ReportError("Exception in ISentoniServiceCallback.PostBlotter", ex);
                }
            }
        }


        public void GetTradingSchedule()
        {
            TradingSchedule[] tradingSchedules = null;
            ISentoniServiceCallback callback = null;

            try
            {
                callback = OperationContext.Current.GetCallbackChannel<ISentoniServiceCallback>();
                tradingSchedules = Provider.ProvideTradingSchedule();

            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.GetTradingSchedule", ex);
            }
            finally
            {
                try
                {
                     callback.PostTradingSchedule(tradingSchedules);
                }
                catch (Exception ex)
                {
                    Provider.ReportError("Exception in ISentoniServiceCallback.PostTradingSchedule", ex);
                }
            }
        }

        public void TakeSnapshot(string acctName)
        {
 
            try
            {
                 Portfolio   portfolio = Provider.ProvidePortfolio(acctName, false);
                 portfolio.SnapshotType = SnapshotType.User;

                 if (portfolio != null)
                 {
                     Provider.SavePortfolioSnapshot(portfolio, PortfolioToXml(portfolio));
                 }
 
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.TakeSnapshot", ex);
            }
        }

        public void SwitchQuoteFeed(string hostName)
        {
            try
            {
                Provider.SwitchQuoteFeed(hostName);
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.SwitchQuoteFeed", ex);
            }
        }
        #endregion

        #region Serialization
        public static string PortfolioToXml(Portfolio portfolio)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Portfolio));
                using (StringWriter stringWriter = new StringWriter())
                {
                    XmlTextWriter writer = new XmlTextWriter(stringWriter);
                    serializer.WriteObject(writer, portfolio);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.PortfolioToXml", ex);
                return null;
            }
        }

        public static Portfolio XmlToPortfolio(string xml)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Portfolio));
                using (StringReader stringReader = new StringReader(xml))
                {
                    XmlTextReader reader = new XmlTextReader(stringReader);
                    return (Portfolio)serializer.ReadObject(reader);
                }
            }
            catch (Exception ex)
            {
                Provider.ReportError("Exception in SentoniService.XmlToPortfolio", ex);
                return null;
            }
        }
        #endregion

    }
}
