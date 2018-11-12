using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GargoyleTaskLib;
using SentoniServiceLib;
using log4net;
using Gargoyle.Common;
using Gargoyle.Utils.DBAccess;
using System.Threading;
using PositionMonitorLib;
using LoggingUtilitiesLib;

namespace SentoniHost
{
    public class Host : IDisposable
    {
        private bool m_bTaskStarted;
        private bool m_bTaskFailed;
        private bool m_bWaiting;
        private string m_lastErrorMessage;
        private CommandLineParameters m_parms;

        private ILog m_logger = LogManager.GetLogger(typeof(Host));
        private TimeSpan m_stopTime;
        private AutoResetEvent m_waitHandle = new AutoResetEvent(false);

        private System.Data.SqlClient.SqlConnection m_hugoConnection;
        PositionMonitorUtilities m_positionMonitor = new PositionMonitorUtilities();

        public Host(CommandLineParameters parms)
        {
            m_parms = parms;
        }

        public string QuoteServerHost
        {
            get
            {
                return m_positionMonitor.QuoteServerHost;
            }
            set
            {
                m_positionMonitor.QuoteServerHost = value.Trim();
            }
        }
        public void StopQuoteFeed()
        {
            m_positionMonitor.StopQuoteFeed();
        }

        public bool Run()
        {
            m_bTaskFailed = true;
            try
            {
                // initialize logging
                log4net.Config.XmlConfigurator.Configure();
                TaskUtilities.OnInfo += new LoggingEventHandler(Utilities_OnInfo);
                TaskUtilities.OnError += new LoggingEventHandler(Utilities_OnError);
                LoggingUtilities.OnInfo += new LoggingEventHandler(Utilities_OnDebug); // SQL Commands go to Debug log
                LoggingUtilities.OnError += new LoggingEventHandler(Utilities_OnError);
                m_positionMonitor.OnInfo += new LoggingEventHandler(Utilities_OnInfo);
                m_positionMonitor.OnError += new LoggingEventHandler(Utilities_OnError);
                m_positionMonitor.OnDebug += new LoggingEventHandler(Utilities_OnDebug);
                m_positionMonitor.OnMonitorStopped += new ServiceStoppedEventHandler(Utilities_OnMonitorStopped);

                if (Initialize())
                {
                    using (var serviceHost = new ServiceHost(typeof(SentoniService)))
                    {
                        m_positionMonitor.QuoteServerHost = m_parms.QuoteServerHost.Trim();
                        m_positionMonitor.QuoteServerPort = m_parms.QuoteServerPort;
                        m_positionMonitor.RefreshMs = m_parms.RefreshMs;
                        m_positionMonitor.AccountLimit = m_parms.AccountLimit;

                        if (m_positionMonitor.StartMonitor())
                        {
                            bool quoteServerIsUp = m_positionMonitor.IsUsingQuoteFeed;
                            foreach (AccountPortfolio account in m_positionMonitor.GetAllAccountPortfolios())
                            {
                                OnInfo("Starting account " + account.AccountName);
                                account.Start();

                                // connect to quote server - if is down, don't bother trying with subsequent accounts
                                if (quoteServerIsUp)
                                    quoteServerIsUp = account.StartSubscriber();
                            }

                            var dataProvider = new DataProvider(m_positionMonitor, this);
                            SentoniService.Provider = dataProvider;
                            m_positionMonitor.SnapshotProvider = dataProvider;
                            serviceHost.Open();
                            OnInfo("Host started");

                            m_bTaskFailed = false;  // set up was successful - we now wait for the timer to expire or for a post from an event handler
                            m_bWaiting = true;
                            bool timedOut = WaitForCompletion();
                            m_bWaiting = false;

                            serviceHost.Close();
                            OnInfo("Host closed");
                            m_bTaskFailed = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                m_bTaskFailed = true;
                OnError("Error in Run method", ex, true);
            }
            finally
            {
                m_positionMonitor.StopMonitor();

                if (m_bTaskStarted)
                {
                    m_bTaskStarted = !EndTask(m_parms.TaskName, !m_bTaskFailed);
                }
            }
            return !m_bTaskFailed;

        }

        private bool Initialize()
        {
            if (!m_parms.GetStopTime(out m_stopTime))
            {
                OnInfo("Invalid stop time specified", true);
                return false;
            }

            DBAccess dbAccess = DBAccess.GetDBAccessOfTheCurrentUser(m_parms.ProgramName);
            m_hugoConnection = dbAccess.GetConnection("Hugo");
            if (m_hugoConnection == null)
            {
                OnInfo("Unable to connect to Hugo", true);
                return false;
            }

            if (!m_positionMonitor.Init(m_hugoConnection))
            {
                OnInfo("Unable to start position monitor", true);
                return false;
            }


            if (!String.IsNullOrEmpty(m_parms.TaskName.Trim()))
            {
                switch (StartTask(m_parms.TaskName))
                {
                    case 0:
                        m_bTaskStarted = true;
                        break;

                    case 1:
                        m_bTaskStarted = false;
                        OnInfo("Task not started because it is a holiday");
                        return false;

                    default:
                        // if task wasn't started (which probably means taskname was not in the table), so be it - no need to abort
                        m_bTaskStarted = false;
                        break;
                }
            }

            return true;
        }

        // returns true if terminated because we reached stopTime, false if terminated early
        private bool WaitForCompletion()
        {
            DateTime stopDateTime = DateTime.Today + m_stopTime;

            int tickTime = (int)(stopDateTime - DateTime.Now).TotalMilliseconds;
            if (tickTime <= 900000)
                tickTime = 900000;  // make sure stop time is at least 15 minutes from now

            OnInfo(String.Format("Waiting for {0} ms", tickTime));
            if (WaitAny(tickTime, m_waitHandle))
            {
                OnInfo("Job terminated early");
                return false;
            }
            else
            {
                OnInfo("Job terminated on schedule");
                return true;
            }
        }

        private bool WaitAny(int millisecondsTimeout, params System.Threading.WaitHandle[] successConditionHandles)
        {
            int n;
            if (millisecondsTimeout == 0)
                n = System.Threading.WaitHandle.WaitAny(successConditionHandles);
            else
                n = System.Threading.WaitHandle.WaitAny(successConditionHandles, millisecondsTimeout);
            if (n == System.Threading.WaitHandle.WaitTimeout)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Task Management
        // returns 0 if task is successfully started
        // returns 1 if we should not start the task because it should not be run today (because this task should not run on a holiday, for example)
        // returns 4 if we cannot find the task name
        // returns some number higher than 4 on an unexpected failure
        private int StartTask(string taskName)
        {
            try
            {
                using (TaskUtilities taskUtilities = new TaskUtilities(m_hugoConnection, m_parms.Timeout))
                {
                    return taskUtilities.TaskStarted(taskName, null);
                }
            }
            catch (Exception ex)
            {
                OnError("Unable to start task", ex);
                return 16;
            }
        }

        private bool EndTask(string taskName, bool succeeded)
        {
            try
            {
                using (TaskUtilities taskUtilities = new TaskUtilities(m_hugoConnection, m_parms.Timeout))
                {
                    if (succeeded)
                        return (0 != taskUtilities.TaskCompleted(taskName, ""));
                    else
                        return (0 != taskUtilities.TaskFailed(taskName, m_lastErrorMessage));
                }
            }
            catch (Exception ex)
            {
                OnError("Unable to stop task", ex);
                return false;
            }
        }
        #endregion

        #region Logging
        private void Utilities_OnError(object sender, LoggingEventArgs e)
        {
            OnError(e.Message, e.Exception);
        }

        private void Utilities_OnInfo(object sender, LoggingEventArgs e)
        {
            OnInfo(e.Message);
        }
        private void Utilities_OnDebug(object sender, LoggingEventArgs e)
        {
            OnDebug(e.Message);
        }
        public void OnDebug(string msg)
        {
            Console.WriteLine(msg);

            if (m_logger != null)
            {
                lock (m_logger)
                {
                    m_logger.Debug(msg);
                }
            }

        }
        public void OnInfo(string msg, bool updateLastErrorMsg = false)
        {
            Console.WriteLine(msg);

            if (updateLastErrorMsg)
                m_lastErrorMessage = msg;
            if (m_logger != null)
            {
                lock (m_logger)
                {
                    m_logger.Info(msg);
                }
            }

        }
        public void OnError(string msg, Exception e, bool updateLastErrorMsg = false)
        {
            Console.WriteLine(msg);
            if (e != null)
            {
                Console.WriteLine(e.Message);
            }

            if (updateLastErrorMsg && e != null)
                m_lastErrorMessage = msg + "->" + e.Message;
            else
                m_lastErrorMessage = msg;

            if (m_logger != null)
            {
                lock (m_logger)
                {
                    m_logger.Error(msg, e);
                }
            }
        }
        void Utilities_OnMonitorStopped(object sender, ServiceStoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                OnError(e.Message, e.Exception);
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (m_bWaiting)
            {
                m_waitHandle.Set();
                System.Threading.Thread.Sleep(m_parms.Timeout * 2);
            }

            if (m_bTaskStarted)
                m_bTaskStarted = !EndTask(m_parms.TaskName, !m_bTaskFailed);

            if (disposing)
            {
                if (m_waitHandle != null)
                {
                    m_waitHandle.Dispose();
                    m_waitHandle = null;
                }

                if (m_positionMonitor != null)
                {
                    m_positionMonitor.Dispose();
                    m_positionMonitor = null;
                }

                if (m_hugoConnection != null)
                {
                    m_hugoConnection.Dispose();
                    m_hugoConnection = null;
                }
            }
        }
        #endregion
    }
}
