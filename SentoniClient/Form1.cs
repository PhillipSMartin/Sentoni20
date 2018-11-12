using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using SentoniClient.SentoniServiceReference;


// next error number: 22

// TODO -
//      no refresh on either portfolio page when looking at snapshot
//      combo box with snapshot ids - not refreshed from snapshot

namespace SentoniClient
{
    public partial class Form1 : Form
    {
        private const string PUTS_OUTOFBOUNDS_TEXT = "Puts are more than {0:p0} away";
        private SentoniServiceClient m_proxy = null;
        private bool m_formClosing = false;
        private bool m_viewingSnapshotList = false;
        private bool m_errorCheckingHeartbeat = false;
        private int m_hearbeatCallInProgress = 0;

        private enum ServiceStatus { Unstarted, Up, Down };
        private ServiceStatus m_serviceStatus = ServiceStatus.Unstarted;
        private SentoniServiceCallback m_callback;
        private InstanceContext m_context;

        private List<TradingSchedule> m_tradingScheduleTable;

        private ILog m_logger = LogManager.GetLogger(typeof(Form1));
        private ToolTipTexts m_toolTipTexts;
        private ToolTip m_toolTip = new ToolTip();
        private ContextMenuStrip m_menu = new ContextMenuStrip();
        private ToolStripMenuItem m_newTradeCommand;
        private ToolStripMenuItem m_editTradeCommand;
        private ToolStripMenuItem m_changeAccountCommand;

        #region Risk cells
        private Cell m_cellPerformance;
        private Cell m_cellTargetPerformance;
        private Cell m_cellInflows;
        private Cell m_cellSODEquity;
        private Cell m_cellAvailableCash;
        private Cell m_cellCurrentEquity;
        private Cell m_cellCurrentCash;
        private Cell m_cellOptionPandL;
        private Cell m_cellStockPandL;
        private Cell m_cellFuturesPandL;
        private Cell m_cellTotalPandL;
        private Cell m_cellMinDelta;
        private Cell m_cellMaxDelta;
        private Cell m_cellTargetDelta;
        private Cell m_cellLeverage;
        private Cell m_cellCurrentLeverage;
        private Cell m_cellPutsPctTarget;
        private Cell m_cellMaximumCaks;
        private Cell m_cellDeltaGoal;
        private Cell m_cellTotalDeltaPct;
        private Cell m_cellPutsToTrade;
        private Cell m_cellNextTradeTime;
        private Cell m_cellDollarDeltasTraded;
        private Cell m_cellDeltaPctTraded;
        private Cell m_cellPutsTraded;
        private Cell[][] m_indexLabels;
        private Cell[] m_totalLabels;
        private Cell[] m_caksLabels;
        #endregion

        Snapshot m_snapshot;
        StressTest m_stressTest = new StressTest();

        public Form1()
        {
            InitializeComponent();
            ClearPortfolio();
            AccountName = "";
            BlotterTradeDate = DateTime.Today;

            m_cellPerformance = new Cell(labelPerformance, "{0:p2}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen);
            m_cellTargetPerformance = new Cell(labelTargetPerformance, "{0:p2}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen);
            m_cellInflows = new Cell(labelInflows, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellSODEquity = new Cell(labelSODEquity, "{0:C}");
            m_cellAvailableCash = new Cell(labelSODCash, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellCurrentEquity = new Cell(labelCurrentEquity, "{0:C}");
            m_cellCurrentCash = new Cell(labelCurrentCash, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellOptionPandL = new Cell(labelOptionPandL, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellStockPandL = new Cell(labelStockPandL, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellFuturesPandL = new Cell(labelFuturesPandL, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellTotalPandL = new Cell(labelTotalPandL, "{0:C}", 0, System.Drawing.Color.Red, System.Drawing.Color.Black);
            m_cellMinDelta = new Cell(labelMinDelta, "{0:p1}");
            m_cellMaxDelta = new Cell(labelMaxDelta, "{0:p1}");
            m_cellTargetDelta = new Cell(labelTargetDelta, "{0:p0}");
            m_cellLeverage = new Cell(labelLeverage, "{0:p0}");
            m_cellCurrentLeverage = new Cell(labelCurrentLeverage, "{0:p0}");
            m_cellPutsPctTarget = new Cell(labelPutsPctTarget, "{0:p1}");
            m_cellMaximumCaks = new Cell(labelMaximumCaks, "{0:F2}");
            m_cellDeltaGoal = new Cell(labelDeltaGoal, "{0:p1}", null, System.Drawing.Color.DarkGreen, System.Drawing.Color.Red, false,
                new Label[] { labelDeltaGoalLabel });
             m_cellTotalDeltaPct = new Cell(labelTotalDeltaPct, "{0:p1}", null, System.Drawing.Color.DarkGreen, System.Drawing.Color.Red, true,
                new Label[] { labelTotalDeltaPctLabel, labelDeltasToTradeLabel });
            m_cellPutsToTrade = new Cell(labelPutsToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.DarkGreen, false,
                new Label[] { labelPutsToTradeLabel, labelPutsToTradeLabel2 });
            m_cellNextTradeTime = new Cell(labelNextTradeTime, "{0:t}", null, System.Drawing.Color.Black, System.Drawing.Color.DarkGreen, false,
                new Label[] { labelNextTradeTimeLabel });
            m_cellDollarDeltasTraded = new Cell(labelDollarDeltasTraded, "{0:C0}", null, System.Drawing.Color.DarkGreen, System.Drawing.Color.Red, false,
                new Label[] { labelDollarDeltasTradedLabel });
            m_cellDeltaPctTraded = new Cell(labelDeltaPctTraded, "{0:p1}", null, System.Drawing.Color.DarkGreen, System.Drawing.Color.Red, false,
                 new Label[] { labelDeltaPctTradedLabel });
            m_cellPutsTraded = new Cell(labelPutsTraded, "{0:F0}", null, System.Drawing.Color.DarkGreen, System.Drawing.Color.Red, false,
                  new Label[] { labelPutsTradedLabel });

            m_caksLabels = new Cell[] {
                new Cell(labelIndex1Caks, "{0:F2}", null, System.Drawing.Color.Red, System.Drawing.Color.Black),
                new Cell(labelIndex2Caks, "{0:F2}", null, System.Drawing.Color.Red, System.Drawing.Color.Black),
                new Cell(labelIndex3Caks, "{0:F2}", null, System.Drawing.Color.Red, System.Drawing.Color.Black),
                new Cell(labelIndex4Caks, "{0:F2}", null, System.Drawing.Color.Red, System.Drawing.Color.Black)
            };

            m_indexLabels = new Cell[][] {
                new Cell[] { new Cell(labelIndex1Symbol, "{0}"), new Cell(labelIndex1Weight, "{0:p1}"), null, null, new Cell(labelIndex1Price, "{0:F2}"), null,
                    new Cell(labelIndex1TotalDeltaPct, "{0:p1}"), new Cell(labelIndex1CallDeltaPct, "{0:p1}"), new Cell(labelIndex1PutDeltaPct, "{0:p1}"),
                    new Cell(labelIndex1FaceValuePutsPct, "{0:p1}"), new Cell(labelIndex1ShortPct, "{0:p1}"), new Cell(labelIndex1TimePremium, "{0:p2}"),
                    m_caksLabels[0], new Cell(labelIndex1GammaPct, "{0:p1}"), null, 
                    new Cell(labelIndex1DeltasToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false), 
                    new Cell(labelIndex1PutsToRebalance, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false),
                    new Cell(labelIndex1PutsToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false) },
                new Cell[] { new Cell(labelIndex2Symbol, "{0}"), new Cell(labelIndex2Weight, "{0:p1}"), null, null, new Cell(labelIndex2Price, "{0:F2}"), null,
                    new Cell(labelIndex2TotalDeltaPct, "{0:p1}"), new Cell(labelIndex2CallDeltaPct, "{0:p1}"), new Cell(labelIndex2PutDeltaPct, "{0:p1}"),
                    new Cell(labelIndex2FaceValuePutsPct, "{0:p1}"), new Cell(labelIndex2ShortPct, "{0:p1}"), new Cell(labelIndex2TimePremium, "{0:p2}"),
                    m_caksLabels[1], new Cell(labelIndex2GammaPct, "{0:p1}"), null, 
                    new Cell(labelIndex2DeltasToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false), 
                    new Cell(labelIndex2PutsToRebalance, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false),
                    new Cell(labelIndex2PutsToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false) },
                new Cell[] { new Cell(labelIndex3Symbol, "{0}"), new Cell(labelIndex3Weight, "{0:p1}"), null, null, new Cell(labelIndex3Price, "{0:F2}"), null,
                    new Cell(labelIndex3TotalDeltaPct, "{0:p1}"), new Cell(labelIndex3CallDeltaPct, "{0:p1}"), new Cell(labelIndex3PutDeltaPct, "{0:p1}"),
                    new Cell(labelIndex3FaceValuePutsPct, "{0:p1}"), new Cell(labelIndex3ShortPct, "{0:p1}"), new Cell(labelIndex3TimePremium, "{0:p2}"),
                    m_caksLabels[2], new Cell(labelIndex3GammaPct, "{0:p1}"), null, 
                    new Cell(labelIndex3DeltasToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false), 
                    new Cell(labelIndex3PutsToRebalance, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false),
                    new Cell(labelIndex3PutsToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false) },
               new Cell[] { new Cell(labelIndex4Symbol, "{0}"), new Cell(labelIndex4Weight, "{0:p1}"), null, null, new Cell(labelIndex4Price, "{0:F2}"), null,
                    new Cell(labelIndex4TotalDeltaPct, "{0:p1}"), new Cell(labelIndex4CallDeltaPct, "{0:p1}"), new Cell(labelIndex4PutDeltaPct, "{0:p1}"),
                    new Cell(labelIndex4FaceValuePutsPct, "{0:p1}"), new Cell(labelIndex4ShortPct, "{0:p1}"), new Cell(labelIndex4TimePremium, "{0:p2}"),
                    m_caksLabels[3], new Cell(labelIndex4GammaPct, "{0:p1}"), null, 
                    new Cell(labelIndex4DeltasToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false), 
                    new Cell(labelIndex4PutsToRebalance, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false),
                    new Cell(labelIndex4PutsToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false) }
            };

            m_totalLabels =
                new Cell[] { new Cell(labelTargetSymbol, "{0}"), null, null, null, null, null,
                    new Cell(TargetTotalDeltaPct, "{0:p1}"), new Cell(TargetCallDeltaPct, "{0:p1}"), new Cell(TargetPutDeltaPct, "{0:p1}"),
                    new Cell(TargetFaceValuePutsPct, "{0:p1}"), new Cell(TargetShortPct, "{0:p1}"), new Cell(TargetTimePremium, "{0:p2}"),
                    new Cell(TargetCaks, "{0:F2}"), new Cell(TargetGammaPct, "{0:p1}"), new Cell(labelAnnualizedTheta, "{0:p1}"), null, null,
                    new Cell(TargetPutsToTrade, "{0:F0}", 0, System.Drawing.Color.Red, System.Drawing.Color.ForestGreen, false) };

        }

        #region Properties
        private string AccountName { get; set; }

        private short m_currentSnapshotId;
        private short CurrentSnapshotId
        {
            get { return m_currentSnapshotId; }
            set
            {
                m_currentSnapshotId = value;
                if (m_currentSnapshotId == 0)
                {
                    checkBoxPositionsForAllAccounts.Enabled = true;
                }
                else
                {
                    checkBoxPositionsForAllAccounts.Enabled = false;
                    checkBoxPositionsForAllAccounts.Checked = false;
                }
            }
        }

        private DateTime TradeDate
        {
            get
            {
                return blotterDateTimePicker.Value.Date;
            }
        }

        private string PortfolioAccountName
        {
            get
            {
                return labelPortfolioAccount.Text;
            }
            set
            {
                labelPortfolioAccount.Text = value;
            }
        }

        private string RiskAccountName
        {
            get
            {
                return labelRiskAccount.Text;
            }
            set
            {
                labelRiskAccount.Text = value;
            }
        }

        private string EquityType { get; set; }
 
        private string ClientType { get; set; }

        private string BlotterAccountName
        {
            get
            {
                return labelBlotterAccount.Text;
            }
            set
            {
                labelBlotterAccount.Text = value;
            }
        }

        private DateTime BlotterTradeDate { get; set; }
        #endregion

        #region Public Methods
        public void LoadComboboxSnapshots(Snapshot snapshot)
        {
            Action a = delegate
           {
               if ((snapshot.SavedSnapshots != null) & (!m_viewingSnapshotList) && (CurrentSnapshotId == 0))
               {
                   comboBoxSnapshots.BeginUpdate();
                   comboBoxSnapshots.Items.Clear();
                   comboBoxSnapshots.Items.AddRange(snapshot.SavedSnapshots);
                   if (comboBoxSnapshots.Items.Count > 0)
                       comboBoxSnapshots.SelectedIndex = 0;
                   comboBoxSnapshots.EndUpdate();
               }
           };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        string m_comboboxIndicesAccount;
        public void LoadComboboxIndices(Snapshot snapshot)
        {
            Action a = delegate
           {
               if (m_comboboxIndicesAccount != snapshot.AccountName)
               {
                   m_comboboxIndicesAccount = snapshot.AccountName;
                   comboBoxPositionGridIndex.BeginUpdate();
                   comboBoxPositionGridIndex.Items.Clear();
                   foreach (AccountDataSet.IndicesRow index in snapshot.Indices)
                   {
                       if (index.Symbol != "Total")
                           comboBoxPositionGridIndex.Items.Add(index);
                   }
                   comboBoxPositionGridIndex.DisplayMember = "Symbol";
                   comboBoxPositionGridIndex.EndUpdate();
                   if (comboBoxPositionGridIndex.Items.Count > 0)
                       comboBoxPositionGridIndex.SelectedIndex = 0;
               }
           };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }
       

        // make sure single account
        public void RefreshStressTest(Snapshot snapshot)
        {
            Action a = delegate
            {
                dataGridViewStressTestPositions.SuspendLayout();
                try
                {
                    if (snapshot != null)
                    {
                        labelStressTestAccount.Text = snapshot.AccountName;
                        labelStressTestTimeStamp.Text = String.Format(@"{0:yyyy-MM-dd hh\:mm}", snapshot.TimeStamp);

                        m_stressTest.StressTestDataTable = AccountDataSet.BuildStressTest(snapshot.AccountName, snapshot.Positions);
                        m_stressTest.Indices = snapshot.Indices;
                        m_stressTest.CurrentEquity = snapshot.AccountData[0].CurrentEquity;

                        RebindTableToDataGridView(m_stressTest.StressTestDataTable, dataGridViewStressTestPositions);
                        dataGridViewStressTestPositions.ClearSelection();
                        ShowPortfolioInfo("");
                    }
                }
                catch (Exception ex)
                {
                    ShowStressTestError(1, "Error refreshing stress test positions-", ex);
                }
                finally
                {
                    dataGridViewStressTestPositions.ResumeLayout();
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void RefreshPositions(Snapshot snapshot, string errorMessage)
        {
            Action a = delegate
            {
                bool bRefresh = true;
                dataGridViewPositions.SuspendLayout();
                try
                {
                    if (snapshot != null)
                    {
                        if (AccountName == snapshot.AccountName)
                        {
                            if (snapshot.Positions != null)
                            {
                                var distinctAccounts = snapshot.Positions.Select(x => x.AccountName).Distinct();
                                bool multipleAccounts = distinctAccounts.Count() > 1;

                                if (multipleAccounts)
                                {
                                    if ((PortfolioAccountName == "") && (dataGridViewPositions.SelectedRows.Count > 0))
                                        bRefresh = false;
                                    PortfolioAccountName = "";
                                    PositionsAccountName.Visible = true;
                                }
                                else
                                {
                                    if ((PortfolioAccountName == snapshot.AccountName) && (dataGridViewPositions.SelectedRows.Count > 0))
                                        bRefresh = false;
                                    PortfolioAccountName = snapshot.AccountName;
                                    PositionsAccountName.Visible = false;
                                }
                            }

                            if (bRefresh)
                            {
                                RebindTableToDataGridView(snapshot.Positions, dataGridViewPositions);
                                dataGridViewPositions.ClearSelection();
                            }
                        }

                        if (String.IsNullOrEmpty(errorMessage) && bRefresh)
                        {
                            ShowPortfolioInfo(snapshot.ToString());
                        }
                    }

                    if (!String.IsNullOrEmpty(errorMessage))
                    {
                        ShowPortfolioError(2, errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    ShowPortfolioError(3, "Error refreshing portfolio-", ex);
                }
                finally
                {
                    dataGridViewPositions.ResumeLayout();
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void RefreshRisk(Snapshot snapshot, string errorMessage)
        {
            Action a = delegate
            {
                try
                {
                    if (snapshot != null)
                    {
                        if ((AccountName == snapshot.AccountName) && (snapshot.AccountData.Count > 0))
                        {
                            RiskAccountName = snapshot.AccountName;
                            EquityType = snapshot.AccountData[0].EquityType;
                            ClientType = snapshot.AccountData[0].ClientType;

                            if (snapshot.AccountData.Rows.Count > 0)
                            {
                                // overall risk stats
                                m_cellInflows.SetValue(snapshot.AccountData[0].TodaysInflows);
                                m_cellSODEquity.SetValue(snapshot.AccountData[0].StartOfDayEquity);
                                m_cellAvailableCash.SetValue(snapshot.AccountData[0].AvailableCash);
                                m_cellCurrentEquity.SetValue(snapshot.AccountData[0].CurrentEquity);
                                m_cellCurrentCash.SetValue(snapshot.AccountData[0].CurrentCash);
                                m_cellOptionPandL.SetValue(snapshot.AccountData[0].Option_P_and_L);
                                m_cellStockPandL.SetValue(snapshot.AccountData[0].Stock_P_and_L);
                                m_cellFuturesPandL.SetValue(snapshot.AccountData[0].Futures_P_and_L);
                                m_cellTotalPandL.SetValue(snapshot.AccountData[0].Total_P_and_L);
                                m_cellMinDelta.SetValue(snapshot.AccountData[0].MinDelta);
                                m_cellMaxDelta.SetValue(snapshot.AccountData[0].MaxDelta);
                                m_cellTargetDelta.SetValue(snapshot.AccountData[0].TargetDelta);
                                m_cellLeverage.SetValue(snapshot.AccountData[0].Leverage);
                                m_cellCurrentLeverage.SetValue(snapshot.AccountData[0].CurrentLeverage);
                                m_cellPutsPctTarget.SetValue(snapshot.AccountData[0].PutsPctTarget);
                                m_cellMaximumCaks.SetValue(snapshot.AccountData[0].MaximumCaks);
                                m_cellDollarDeltasTraded.SetValue(snapshot.AccountData[0].DollarDeltasTraded);
                                m_cellDeltaPctTraded.SetValue(snapshot.AccountData[0].DeltaPctTraded);
                                m_cellPutsTraded.SetValue(snapshot.AccountData[0].PutsTraded);
                                if (snapshot.AccountData[0].StartOfDayEquity > 0)
                                    m_cellPerformance.SetValue(snapshot.AccountData[0].Total_P_and_L / snapshot.AccountData[0].StartOfDayEquity);

                                if (!snapshot.AccountData[0].IsNextTradeTimeNull())
                                {
                                    m_cellNextTradeTime.SetValue(snapshot.AccountData[0].NextTradeTime, true);
                                }
                                else
                                {
                                    m_cellNextTradeTime.SetValue(0.0);
                                }

                                foreach (Cell caksLabel in m_caksLabels)
                                {
                                    if (snapshot.AccountData[0].MaximumCaks != 0)
                                        caksLabel.Threshold = snapshot.AccountData[0].MaximumCaks;
                                    else
                                        caksLabel.Threshold = null;
                                }

                                if (snapshot.Indices.Rows.Count > 0)  // must have at least one index and a total
                                {
                                    // show trading goals
                                    AccountDataSet.IndicesRow totalRow = snapshot.Indices[snapshot.Indices.Rows.Count - 1];
                                    if (Math.Abs(snapshot.AccountData[0].DeltaGoal) > .0001)
                                    {
                                        m_cellTotalDeltaPct.Threshold = m_cellDeltaGoal.Threshold = snapshot.AccountData[0].TargetDelta;
                                        m_cellDeltaGoal.SetValue(snapshot.AccountData[0].DeltaGoal, true);
                                        m_cellTotalDeltaPct.SetValue(totalRow.TotalDeltaPct, true);
                                        labelDeltasToTradeLabel.Visible = true;
                                    }
                                    else
                                    {
                                        m_cellTotalDeltaPct.Threshold = m_cellDeltaGoal.Threshold = null;
                                        m_cellDeltaGoal.SetValue(0f);
                                        m_cellTotalDeltaPct.SetValue(totalRow.TotalDeltaPct, false);
                                        labelDeltasToTradeLabel.Visible = false;
                                    }

                                    if (snapshot.AccountData[0].PutsOutOfBounds)
                                    {
                                        labelPutsOutOfBounds.Text = String.Format(PUTS_OUTOFBOUNDS_TEXT, snapshot.AccountData[0].PutsOutOfMoneyThreshold);
                                     }
                                    labelPutsOutOfBounds.Visible = snapshot.AccountData[0].PutsOutOfBounds;

                                    // show index stats
                                    panelPutStats.Visible = ((snapshot.AccountData[0].PutsPctTarget > 0) || (totalRow.PutDeltaPct != 0));
                                    int row = 0;
                                    foreach (Cell[] cellRow in m_indexLabels)
                                    {
                                        for (int column = 0; column < cellRow.Length; column++)
                                        {
                                            Cell cell = cellRow[column];
                                            if (cell != null)
                                            {
                                                if (snapshot.Indices.Rows.Count - 1 <= row)
                                                {
                                                    cell.SetValue(null);
                                                }
                                                else
                                                {
                                                    cell.SetValue(snapshot.Indices[row][column]);
                                                }
                                            }
                                        }
                                        row++;
                                    }

                                    // show target performance
                                    if (!totalRow.IsPrevCloseNull())
                                    {
                                        if (totalRow.PrevClose > 0)
                                        {
                                            m_cellTargetPerformance.SetValue(totalRow.LastPrice / totalRow.PrevClose - 1);
                                        }
                                    }

                                    // determine if "Puts to Rebalance" should be shown
                                    labelPutsToRebalanceLabel.Visible = (labelIndex1PutsToRebalance.Visible || labelIndex2PutsToRebalance.Visible || labelIndex3PutsToRebalance.Visible)
                                        && (snapshot.Indices.Rows.Count > 2);
                                    // show total status
                                    for (int column = 0; column < m_totalLabels.Length; column++)
                                    {
                                        Cell cell = m_totalLabels[column];
                                        if (cell != null)
                                        {
                                            cell.SetValue(totalRow[column]);
                                        }
                                    }

                                    // invert colors on PutsToTrade field 
                                    m_cellPutsToTrade.SetValue(totalRow.PutsToTrade, true);
                                }
                            }

                            if (String.IsNullOrEmpty(errorMessage))
                            {
                                ShowRiskInfo(snapshot.ToString());
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(errorMessage))
                    {
                        ShowRiskError(2, errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    ShowRiskError(3, "Error refreshing risk", ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void RefreshBlotter(string acctName, DateTime tradeDate, AccountDataSet.BlotterDataTable blotterTable, string errorMessage)
        {
            Action a = delegate
            {
                bool bRefreshed = false;
                if (TradeDate == tradeDate)
                {
                    dataGridViewTrades.SuspendLayout();
                    try
                    {

                        BindingSource bindingSource = dataGridViewTrades.DataSource as BindingSource;

                        if (((acctName ?? "") != BlotterAccountName) || (BlotterTradeDate != tradeDate) || (dataGridViewTrades.Rows.Count <= 0))
                        {
                            BlotterAccountName = acctName;
                            BlotterTradeDate = tradeDate;
                            bindingSource.DataSource = blotterTable;
                            dataGridViewTrades.Sort(tradeTimeDataGridViewTextBoxColumn, ListSortDirection.Descending);
                            dataGridViewTrades.ClearSelection();
                            bRefreshed = true;
                        }
                        else
                        {
                            if (dataGridViewTrades.SelectedRows.Count <= 0)
                            {
                                RebindTableToDataGridView(blotterTable, dataGridViewTrades);
                                dataGridViewTrades.ClearSelection();
                                bRefreshed = true;
                            }
                        }

                        accountNameDataGridViewTextBoxColumn.Visible = (acctName == null);

                        if (String.IsNullOrEmpty(errorMessage))
                        {
                            if (BlotterTradeDate != DateTime.Today)
                                ShowBlotterInfo(String.Format("Trades from {0:d}", BlotterTradeDate), false);
                            else if (bRefreshed)
                                ShowBlotterInfo("Last update", true);
                        }
                        else
                        {
                            ShowBlotterError(2, errorMessage, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowBlotterError(3, "Error refreshing blotter", ex);
                    }
                    finally
                    {
                        dataGridViewTrades.ResumeLayout();
                     }
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void RefreshAccountSummaries(AccountDataSet.AccountSummaryDataTable accountSummaries, string errorMessage)
        {
            Action a = delegate
            {
                dataGridViewSummaries.SuspendLayout();
                try
                {
                    RebindTableToDataGridView(accountSummaries, dataGridViewSummaries);
                    if ((dataGridViewSummaries.SortedColumn == null) && (accountSummaries != null))
                        dataGridViewSummaries.Sort(accountNameDataGridViewTextBoxColumn1, ListSortDirection.Ascending);

                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        ShowAccountSummariesInfo("Last update");
                    }
                    else
                    {
                        ShowAccountSummariesError(2, errorMessage, null);
                    }
                }
                catch (Exception ex)
                {
                    ShowAccountSummariesError(3, "Error refreshing account summaries", ex);
                }
                finally
                {
                    dataGridViewSummaries.ResumeLayout();
                    dataGridViewSummaries.ClearSelection();
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void RefreshTradingSchedule(SentoniServiceReference.TradingSchedule[] tradingSchedule, string errorMessage)
        {
            Action a = delegate
            {
                dataGridViewSchedule.SuspendLayout();
                try
                {

                    BindingSource bindingSource = dataGridViewSchedule.DataSource as BindingSource;
                    List<TradingSchedule> tradingScheduleList = new List<TradingSchedule>(tradingSchedule);
                    tradingScheduleList.Sort(new TradingScheduleComparer());

                    // save schedule
                    m_tradingScheduleTable = tradingScheduleList;
                    RebindTableToDataGridView(tradingScheduleList, dataGridViewSchedule);

                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        ShowTradingScheduleInfo("Last update");
                    }
                    else
                    {
                        ShowTradingScheduleError(2, errorMessage, null);
                    }
                }
                catch (Exception ex)
                {
                    ShowTradingScheduleError(3, "Error refreshing trading schedule", ex);
                }
                finally
                {
                    dataGridViewSchedule.ResumeLayout();
                    dataGridViewSchedule.ClearSelection();
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        bool m_bActiveAccountError = false;
        public void RefreshActiveAccount()
        {
            Action a = delegate
           {
               try
               {
                   bool activeAccountFound = false;
                   if (m_tradingScheduleTable != null)
                   {

                       List<TradingSchedule> rows = new List<TradingSchedule>(m_tradingScheduleTable.Where(x => (x.EndTradeTime > DateTime.Now.TimeOfDay) && (x.NextTradeTime <= DateTime.Now.TimeOfDay)));
                       rows.Sort(new TradingScheduleComparer());

                       if (rows.Count > 0)
                       {
                           TimeSpan timeLeft = rows[0].EndTradeTime.Subtract(DateTime.Now.TimeOfDay);
                           if (timeLeft.TotalSeconds > 0)
                           {
                               buttonActiveAccount.Tag = rows[0].AccountName;
                               buttonActiveAccount.Text = String.Format(@"{0} (Time left {1:mm\:ss})", rows[0].AccountName, timeLeft);
                               activeAccountFound = true;
                           }
                       }
                   }

                   if (!activeAccountFound)
                   {
                       buttonActiveAccount.Tag = null;
                       buttonActiveAccount.Text = "";
                   }

                   if (m_bActiveAccountError)
                   {
                       m_bActiveAccountError = false;
                       ShowInfo(""); // clear error message
                   }
               }
               catch (Exception ex)
               {
                   m_bActiveAccountError = true;
                   ShowError(3, "Error refreshing active account", ex);
               }
           };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        #region Error logging
        int m_currentError = 0;
        public void ShowInfo(string msg)
        {
            Action a = delegate
            {
                m_currentError = 0;
                labelStatus.Text = FormatMessage(msg);
                labelStatus.ForeColor = System.Drawing.Color.Black;

                buttonClearErrorMessage.Visible = buttonClearErrorMessage.Enabled = false;

                if ((m_logger != null) && !String.IsNullOrEmpty(msg))
                {
                    lock (m_logger)
                    {
                        m_logger.Info(msg);
                    }
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentError != errorNumber)
                {
                    m_currentError = errorNumber;
                    labelStatus.Text = FormatMessage(msg, ex);
                    labelStatus.ForeColor = System.Drawing.Color.Red;

                    buttonClearErrorMessage.Visible = buttonClearErrorMessage.Enabled = true;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        int m_currentPortfolioError = 0;
        public void ShowPortfolioInfo(string msg)
        {
            Action a = delegate
            {
                m_currentPortfolioError = 0;
                labelPortfolioStatus.Text = msg;
                labelPortfolioStatus.ForeColor = System.Drawing.Color.Black;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowPortfolioError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentPortfolioError != errorNumber)
                {
                    m_currentPortfolioError = errorNumber;
                    labelPortfolioStatus.Text = FormatMessage(msg, ex);
                    labelPortfolioStatus.ForeColor = System.Drawing.Color.Red;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        int m_currentStressTestError = 0;
        public void ShowStressTestInfo(string msg)
        {
            Action a = delegate
            {
                m_currentStressTestError = 0;
                labelStressTestStatus.Text = msg;
                labelStressTestStatus.ForeColor = System.Drawing.Color.Black;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowStressTestError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentStressTestError != errorNumber)
                {
                    m_currentStressTestError = errorNumber;
                    labelStressTestStatus.Text = FormatMessage(msg, ex);
                    labelStressTestStatus.ForeColor = System.Drawing.Color.Red;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        int m_currentRiskError = 0;
        public void ShowRiskInfo(string msg)
        {
            Action a = delegate
            {
                m_currentRiskError = 0;
                labelRiskStatus.Text = msg;
                labelRiskStatus.ForeColor = System.Drawing.Color.Black;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowRiskError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentRiskError != errorNumber)
                {
                    m_currentRiskError = errorNumber;
                    labelRiskStatus.Text = FormatMessage(msg, ex);
                    labelRiskStatus.ForeColor = System.Drawing.Color.Red;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        int m_currentBlotterError = 0;
        public void ShowBlotterInfo(string msg, bool appendTime = true)
        {
            Action a = delegate
            {
                m_currentBlotterError = 0;
                labelBlotterStatus.Text = appendTime ? FormatMessage(msg) : msg;
                labelBlotterStatus.ForeColor = System.Drawing.Color.Black;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowBlotterError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentBlotterError != errorNumber)
                {
                    m_currentBlotterError = errorNumber;
                    labelBlotterStatus.Text = FormatMessage(msg, ex);
                    labelBlotterStatus.ForeColor = System.Drawing.Color.Red;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }
        #endregion

        int m_currentAccountSummariesError = 0;
        public void ShowAccountSummariesInfo(string msg)
        {
            Action a = delegate
            {
                m_currentAccountSummariesError = 0;
                labelAccountSummariesStatus.Text = FormatMessage(msg);
                labelAccountSummariesStatus.ForeColor = System.Drawing.Color.Black;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowAccountSummariesError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentAccountSummariesError != errorNumber)
                {
                    m_currentAccountSummariesError = errorNumber;
                    labelAccountSummariesStatus.Text = FormatMessage(msg, ex);
                    labelAccountSummariesStatus.ForeColor = System.Drawing.Color.Red;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        int m_currentTradingScheduleError = 0;
        public void ShowTradingScheduleInfo(string msg)
        {
            Action a = delegate
            {
                m_currentTradingScheduleError = 0;
                labelTradingScheduleStatus.Text = FormatMessage(msg);
                labelTradingScheduleStatus.ForeColor = System.Drawing.Color.Black;
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        public void ShowTradingScheduleError(int errorNumber, string msg, Exception ex = null)
        {
            Action a = delegate
            {
                if (m_currentTradingScheduleError != errorNumber)
                {
                    m_currentTradingScheduleError = 0;
                    labelTradingScheduleStatus.Text = FormatMessage(msg, ex);
                    labelTradingScheduleStatus.ForeColor = System.Drawing.Color.Red;

                    LogError(msg, ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        bool m_bQuoteServiceIsUp = true;
        public void RefreshSnapshot(Snapshot snapshot, string errorMessage)
        {
            Action a = delegate
            {
                if (snapshot != null)
                {
                    m_snapshot = snapshot;
                    string msg = "";
                    if (snapshot.QuoteServiceStoppedTime.HasValue)
                    {
                        DateTime stoppedTime = DateTime.Today + snapshot.QuoteServiceStoppedTime.Value;
                        labelQuoteServiceStatus.Text = String.Format(@"Last quote received at {0:T}", stoppedTime);
                        if (m_bQuoteServiceIsUp)
                        {
                            msg = labelQuoteServiceStatus.Text;
                            m_bQuoteServiceIsUp = false;
                        }
                    }
                    else
                    {
                        if (snapshot.UnsubscribedSymbols > 0)
                        {
                            labelQuoteServiceStatus.Text = String.Format(@"No quotes available for {0} symbols", snapshot.UnsubscribedSymbols);
                        }
                        else
                        {
                            labelQuoteServiceStatus.Text = "";
                        }

                        if (!m_bQuoteServiceIsUp)
                        {
                            msg = String.Format(@"Quote service is up as of {0:T}", DateTime.Now);
                            m_bQuoteServiceIsUp = true;
                        }
                    }

                    bloombergMenuItem.Checked = (snapshot.QuoteServiceHost.ToUpper() == Properties.Settings.Default.BloombergHost.ToUpper());
                    ibMenuItem.Checked = (snapshot.QuoteServiceHost.ToUpper() == Properties.Settings.Default.IBHost.ToUpper());

                    if ((m_logger != null) && !String.IsNullOrEmpty(msg))
                    {
                        lock (m_logger)
                        {
                            m_logger.Info(msg);
                        }
                    }
                }

                exportToExcelToolStripMenuItem.Enabled = toolStripButtonSnapshot.Enabled = String.IsNullOrEmpty(errorMessage);
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }
        #endregion

        #region Private methods
        private bool IsNull(object obj)
        {
            if (obj == null)
                return true;
            if (obj.GetType() == typeof(DBNull))
                return true;
            if (obj.GetType() == typeof(string))
                return String.IsNullOrEmpty((string)obj);
            return false;
        }
    
        private void ClearPortfolio()
        {
            labelPortfolioAccount.Text = "";
            labelPerformance.Text = "";
            labelTargetPerformance.Text = "";
            labelBlotterAccount.Text = "";
            labelPortfolioStatus.Text = "";
            labelBlotterStatus.Text = "";
            labelRiskAccount.Text = "";
            labelTotalPandL.Text = "";
            labelFuturesPandL.Text = "";
            labelStockPandL.Text = "";
            labelOptionPandL.Text = "";
            labelSODCash.Text = "";
            labelSODEquity.Text = "";
            labelRiskStatus.Text = "";
            labelInflows.Text = "";
            labelIndex4Price.Text = "";
            labelIndex3Price.Text = "";
            labelIndex2Price.Text = "";
            labelIndex1Price.Text = "";
            labelIndex4Weight.Text = "";
            labelIndex3Weight.Text = "";
            labelIndex2Weight.Text = "";
            labelIndex1Weight.Text = "";
            labelIndex4Symbol.Text = "";
            labelIndex3Symbol.Text = "";
            labelIndex2Symbol.Text = "";
            labelIndex1Symbol.Text = "";
            labelMinDelta.Text = "";
            labelTargetDelta.Text = "";
            labelMaxDelta.Text = "";
            labelPutsPctTarget.Text = "";
            labelMaximumCaks.Text = "";
            labelLeverage.Text = "";
            labelCurrentLeverage.Text = "";
            labelIndex4DeltasToTrade.Text = "";
            labelIndex3DeltasToTrade.Text = "";
            labelIndex2DeltasToTrade.Text = "";
            labelIndex1DeltasToTrade.Text = "";
            labelIndex4PutsToRebalance.Text = "";
            labelIndex3PutsToRebalance.Text = "";
            labelIndex2PutsToRebalance.Text = "";
            labelIndex1PutsToRebalance.Text = "";
            labelIndex4Caks.Text = "";
            labelIndex3Caks.Text = "";
            labelIndex2Caks.Text = "";
            labelIndex1Caks.Text = "";
            labelIndex4TimePremium.Text = "";
            labelIndex3TimePremium.Text = "";
            labelIndex2TimePremium.Text = "";
            labelIndex1TimePremium.Text = "";
            labelIndex4ShortPct.Text = "";
            labelIndex3ShortPct.Text = "";
            labelIndex2ShortPct.Text = "";
            labelIndex1ShortPct.Text = "";
            labelIndex4TotalDeltaPct.Text = "";
            labelIndex3TotalDeltaPct.Text = "";
            labelIndex2TotalDeltaPct.Text = "";
            labelIndex1TotalDeltaPct.Text = "";
            labelIndex4PutDeltaPct.Text = "";
            labelIndex3PutDeltaPct.Text = "";
            labelIndex2PutDeltaPct.Text = "";
            labelIndex1PutDeltaPct.Text = "";
            labelIndex4CallDeltaPct.Text = "";
            labelIndex3CallDeltaPct.Text = "";
            labelIndex2CallDeltaPct.Text = "";
            labelIndex1CallDeltaPct.Text = "";
            labelIndex4FaceValuePutsPct.Text = "";
            labelIndex3FaceValuePutsPct.Text = "";
            labelIndex2FaceValuePutsPct.Text = "";
            labelIndex1FaceValuePutsPct.Text = "";
            labelCurrentEquity.Text = "";
            labelCurrentCash.Text = "";
            TargetFaceValuePutsPct.Text = "";
            TargetCaks.Text = "";
            TargetTimePremium.Text = "";
            TargetShortPct.Text = "";
            labelTotalDeltaPct.Text = "";
            TargetPutDeltaPct.Text = "";
            TargetCallDeltaPct.Text = "";
            labelTargetSymbol.Text = "";
            labelPutsToTrade.Text = "";
            labelDeltaGoal.Text = "";
            labelAnnualizedTheta.Text = "";
            labelIndex1GammaPct.Text = "";
            labelIndex2GammaPct.Text = "";
            labelIndex3GammaPct.Text = "";
            labelIndex4GammaPct.Text = "";
            TargetGammaPct.Text = "";
            TargetTotalDeltaPct.Text = "";
            TargetPutsToTrade.Text = "";
            labelIndex1PutsToTrade.Text = "";
            labelIndex2PutsToTrade.Text = "";
            labelIndex3PutsToTrade.Text = "";
            labelIndex4PutsToTrade.Text = "";
            labelNextTradeTime.Text = "";
            labelDollarDeltasTraded.Text = "";
            labelDeltaPctTraded.Text = "";
            labelPutsTraded.Text = "";

            BindingSource bindingSource = dataGridViewPositions.DataSource as BindingSource;
            bindingSource.DataSource = null;
        }

        public void LogError(string msg, Exception ex)
        {
            if ((m_logger != null) && !String.IsNullOrEmpty(msg))
            {
                lock (m_logger)
                {
                    if (ex != null)
                    {
                        m_logger.Error(msg, ex);
                    }
                    else
                    {
                        m_logger.Error(msg);
                    }
                }
            }
        }

        private string FormatMessage(string msg, Exception ex = null)
        {
            if (ex != null)
            {
                return String.Format("{0}->{1} at {2:T}", msg, ex.Message, DateTime.Now);
            }
            else if (!String.IsNullOrEmpty(msg))
            {
                return String.Format("{0} at {1:T}", msg, DateTime.Now);
            }
            else
            {
                return "";
            }
        }
   
        private bool CheckHeartBeat()
        {
            bool receivedHeartBeat = false;

            // make sure we don't call HeartBeat if we are still waiting for a previous call to return
            if (0 == Interlocked.Exchange(ref m_hearbeatCallInProgress, 1))
            {
                try
                {
                    receivedHeartBeat = m_proxy.HeartBeat();

                    if (m_errorCheckingHeartbeat)
                    {
                        m_errorCheckingHeartbeat = false;
                        ShowInfo("");   // clear error message
                    }
                }
                catch (Exception ex)
                {
                    // log error only once for each lost connection
                    if (!m_errorCheckingHeartbeat)
                    {
                        m_errorCheckingHeartbeat = true;
                        LogError("Error checking heartbeat", ex);
                    }

                    ShowError(4, "No heartbeat - attempting to reconnect");

                    // create a new context and try again 
                    m_callback = new SentoniServiceCallback(this);
                    m_context = new InstanceContext(m_callback);
                }
                finally
                {
                    Interlocked.Exchange(ref m_hearbeatCallInProgress, 0);
                    SetServiceStatus(receivedHeartBeat);
                }
            }
            return receivedHeartBeat;
        }

        private void SetServiceStatus(bool up)
        {
            Action a = delegate
            {
                string msg = "";
                buttonRefresh.Enabled = up;

                if (up && (m_serviceStatus != ServiceStatus.Up))
                {
                    m_serviceStatus = ServiceStatus.Up;
                    labelPositionServiceStatus.Text = msg = String.Format("Connected to Sentoni Service as of {0:T}", DateTime.Now);
                    labelPositionServiceStatus.ForeColor = System.Drawing.Color.Green;

                    if (comboBoxAccounts.DataSource == null)
                    {
                        comboBoxAccounts.DataSource = m_proxy.GetAccountList();
                        if (comboBoxAccounts.Items.Count > 0)
                        {
                            var account = comboBoxAccounts.Items[0];
                            OnAccountChangeCommitted(account.ToString());
                        }
                    }
                }
                else if (!up && (m_serviceStatus != ServiceStatus.Down))
                {
                    m_serviceStatus = ServiceStatus.Down;
                    labelPositionServiceStatus.Text = msg = String.Format("Not connected to Sentoni Service as of {0:T}", DateTime.Now);
                    labelPositionServiceStatus.ForeColor = System.Drawing.Color.Red;
                }

                if ((m_logger != null) && !String.IsNullOrEmpty(msg))
                {
                    lock (m_logger)
                    {
                        m_logger.Info(labelPositionServiceStatus.Text);
                    }
                }
            };
            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        private void GetPortfolio()
        {
            Action a = delegate
            {
                try
                {
                    if ((AccountName != "") && (m_serviceStatus == ServiceStatus.Up))
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(GetPortfolioThread);
                        GetPortfolioArgs args = new GetPortfolioArgs() { AcctName = AccountName, PositionsForAllAccounts = checkBoxPositionsForAllAccounts.Checked };
                        worker.RunWorkerAsync(args);
                    }
                }
                catch (Exception ex)
                {
                    ShowPortfolioError(4, "Error starting GetPortfolio thread", ex);
                    ShowRiskError(4, "Error starting GetPortfolio thread", ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        private void GetBlotter()
        {
            Action a = delegate
            {
                if ((tabControl1.SelectedTab == tabPageBlotter) && (m_serviceStatus == ServiceStatus.Up))
                {
                    try
                    {
                        if (TradeDate != DateTime.Today)
                        {
                            BindingSource bindingSource = dataGridViewTrades.DataSource as BindingSource;
                            bindingSource.DataSource = null;
                            ShowBlotterInfo(String.Format("Getting trades from {0:d}...", TradeDate), false);
                        }

                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(GetBlotterThread);
                        GetBlotterArgs args = new GetBlotterArgs() { TradeDate = this.TradeDate };

                        if ((AccountName == "") || checkBoxTradesForAllAccounts.Checked)
                        {
                            args.AcctName = null;
                        }
                        else
                        {
                            args.AcctName = AccountName;
                        }
                        worker.RunWorkerAsync(args);
                    }
                    catch (Exception ex)
                    {
                        ShowBlotterError(4, "Error starting GetBlotter thread", ex);
                    }
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        private void GetSchedule()
        {
            Action a = delegate
            {
                try
                {
                    if ((tabControl1.SelectedTab == tabPageTradingSchedule || m_tradingScheduleTable == null) && (m_serviceStatus == ServiceStatus.Up))
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(GetScheduleThread);
                        worker.RunWorkerAsync();
                    }
                }
                catch (Exception ex)
                {
                    ShowTradingScheduleError(4, "Error starting GetSchedule thread", ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

         private void GetSummary()
        {
            Action a = delegate
            {
                try
                {
                    if ((tabControl1.SelectedTab == tabPageSummary) && (m_serviceStatus == ServiceStatus.Up))
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(GetSummaryThread);
                        worker.RunWorkerAsync();
                    }
                }
                catch (Exception ex)
                {
                    ShowAccountSummariesError(4, "Error starting GetSummary thread", ex);
                }
            };

            if (InvokeRequired)
                BeginInvoke(a);
            else
                a.Invoke();
        }

        private void RefreshAll(bool requestFromUser=false)
        {
            if (CurrentSnapshotId == 0)
                GetPortfolio();
            if ((TradeDate == DateTime.Today) || requestFromUser)
                GetBlotter();
            GetSchedule();
            GetSummary();

            RefreshActiveAccount();
        }

        private void RebindTableToDataGridView(object dataTable, DataGridView dataGridView)
        {
            BindingSource bindingSource = dataGridView.DataSource as BindingSource;
            DataGridViewColumn oldSortColumn = dataGridView.SortedColumn;
            ListSortDirection oldSortOrder = (dataGridView.SortOrder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending;

            if ((dataGridView.Rows.Count <= 0) || (dataTable == null))
            {
                bindingSource.DataSource = dataTable;
            }
            else
            {
                int saveRow = -1;
                int saveHorizontalPosition = dataGridView.HorizontalScrollingOffset;
                if (dataGridView.FirstDisplayedCell != null)
                    saveRow = dataGridView.FirstDisplayedCell.RowIndex;

                bindingSource.DataSource = dataTable;

                if ((saveRow >= 0) && (saveRow < dataGridView.RowCount))
                    dataGridView.FirstDisplayedScrollingRowIndex = saveRow;
                dataGridView.HorizontalScrollingOffset = saveHorizontalPosition;

            }

            // Restore the sort
            if (oldSortColumn != null)
                dataGridView.Sort(oldSortColumn, oldSortOrder);
        }

        private void SelectAccount(string accountName)
        {
            comboBoxAccounts.SelectedIndex = comboBoxAccounts.FindStringExact(accountName);
            OnAccountChangeCommitted(accountName);
            tabControl1.SelectedTab = tabPageRisk;
        }

        private void OnAccountChangeCommitted(string accountName)
        {
            AccountName = accountName;
            CurrentSnapshotId = 0;
            RefreshAll(true);
         }

        // returns true if gtColor was chosen
        private bool ChooseCellStyle<T>(DataGridViewCellPaintingEventArgs e, T threshold, System.Drawing.Color gtColor, System.Drawing.Color ltColor, bool invertColors=false)
        {
            bool gtColorWasChosen = true;
            e.CellStyle.ForeColor = gtColor;

            if (!IsNull(e.Value) && !IsNull(threshold))
            {
                if (Comparer<T>.Default.Compare((T)e.Value, threshold) < 0)
                {
                    e.CellStyle.ForeColor = ltColor;
                    gtColorWasChosen = false;
                }
            }
 
            if (invertColors && (e.CellStyle.ForeColor != Color.Black))
            {
                e.CellStyle.BackColor = e.CellStyle.ForeColor;
                e.CellStyle.ForeColor = Color.White;
            }
            return gtColorWasChosen;
        }

        private string BuildNewTradeParms(DataGridViewRow row)
        {
            string parms = String.Format("/acct:{0}", AccountName.Replace(' ', '%'));
            if (row != null)
            {
                if ((bool)row.Cells[isStockDataGridViewCheckBoxColumn.Index].Value)
                {
                    parms += String.Format(",/instr:Stock,/sym:{0}",
                        row.Cells[underlyingSymbolDataGridViewTextBoxColumn.Index].Value);
                }
                else if ((bool)row.Cells[isOptionDataGridViewCheckBoxColumn.Index].Value)
                    parms += String.Format(",/instr:Option,/sym:{0},/str:{1},/opt:{2},/exp:{3}",
                        row.Cells[symbolDataGridViewTextBoxColumn.Index].Value.ToString().Substring(0,6).TrimEnd(),
                        row.Cells[strikePriceDataGridViewTextBoxColumn.Index].Value,
                        row.Cells[optionTypeDataGridViewTextBoxColumn.Index].Value,
                        ((DateTime)row.Cells[expirationDateDataGridViewTextBoxColumn.Index].Value).ToString("yyyy-MM-dd"));
                else if ((bool)row.Cells[isFutureDataGridViewCheckBoxColumn.Index].Value)
                    parms += String.Format(",/instr:Future,/sym:{0}",
                        row.Cells[underlyingSymbolDataGridViewTextBoxColumn.Index].Value);
            }
            return parms;
        }

        private string BuildEditTradeParms(DataGridViewRow row)
        {
            string parms = String.Format("/tid:{0}",
                    (int)row.Cells[TradeId.Index].Value);
            return parms;
        }

        private string BuildChangeAccountParms(DataGridViewRow row)
        {
            string parms = String.Format("/tid:{0},/prov:{1}",
                    (int)row.Cells[TradeId.Index].Value,
                    (string)row.Cells[TradeSource.Index].Value);
            return parms;
        }
  
        private bool CanEditTrade(DataGridViewRow row)
        {
            try
            {
                return ((TradeDate == DateTime.Today) || ("$" == (string)row.Cells[underlyingSymbolDataGridViewTextBoxColumn1.Index].Value)) &&
                    ("Hugo" == (string)row.Cells[TradeSource.Index].Value);
            }
            catch
            {
                return false;
            }
        }

        private bool CanChangeAccount(DataGridViewRow row)
        {
            try
            {
                return ("Hugo" != (string)row.Cells[TradeSource.Index].Value);
            }
            catch
            {
                return false;
            }
        }

        private void LaunchTradeEntry(string parms)
        {
            string shortcutName =
                   string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                     Properties.Settings.Default.TradeEntryPath, ".appref-ms");
            try
            {
                System.Diagnostics.Process.Start(shortcutName, parms ?? "");
            }
            catch (Exception ex)
            {
                ShowError(9, "Can't launch Trade Entry application", ex);
            }
        }

        private void LaunchReports(string parms)
        {
            string shortcutName =
                   string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                     Properties.Settings.Default.ReportsPath, ".appref-ms");
            try
            {
                System.Diagnostics.Process.Start(shortcutName, parms ?? "");
            }
            catch (Exception ex)
            {
                ShowError(3, "Can't launch Reports application", ex);
            }
        }
        #endregion

        #region Background Worker Threads
        private void MonitorHeartbeatThread(object sender, DoWorkEventArgs e)
        {
            bool bErrorReported = false;
            while (!m_formClosing)
            {
                try
                {
                    if (m_serviceStatus != ServiceStatus.Up)
                    {
                        m_proxy = new SentoniServiceClient(m_context);
                    }

                    if (CheckHeartBeat())
                    {
                        if (checkBoxAutoRefresh.Checked)
                        {
                            RefreshAll();
                        }
                    }
                    if (bErrorReported)
                    {
                        bErrorReported = false;
                        ShowInfo("Heartbeat thread resumed");
                    }
                }
                catch (Exception ex)
                {
                    bErrorReported = true;
                    ShowError(5, "Error in heartbeat thread", ex);
                }

                System.Threading.Thread.Sleep(Properties.Settings.Default.HeartbeatTimer);
            }
        }

        private void GetPortfolioThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                GetPortfolioArgs args = (GetPortfolioArgs)e.Argument;
                if (m_serviceStatus == ServiceStatus.Up)
                {
                    m_proxy.GetPortfolio(args.AcctName, args.PositionsForAllAccounts);
                }
            }
            catch (Exception ex)
            {
                ShowPortfolioError(1, "Error getting portfolio", ex);
                ShowRiskError(1, "Error getting portfolio", ex);
            }
        }

        private void GetBlotterThread(object o, DoWorkEventArgs e)
        {
            try
            {
                GetBlotterArgs args = (GetBlotterArgs)e.Argument;
                if (m_serviceStatus == ServiceStatus.Up)
                {
                    m_proxy.GetBlotter(args.AcctName, args.TradeDate);
                }
            }
            catch (Exception ex)
            {
                ShowBlotterError(1, "Error getting blotter", ex);
            }
        }

        private void GetScheduleThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_serviceStatus == ServiceStatus.Up)
                {
                    m_proxy.GetTradingSchedule();
                }
            }
            catch (Exception ex)
            {
                this.ShowTradingScheduleError(1, "Error getting schedule", ex);
            }
        }

        private void GetSummaryThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (m_serviceStatus == ServiceStatus.Up)
                {
                    m_proxy.GetAccountSummaries();
                }
            }
            catch (Exception ex)
            {
                ShowAccountSummariesError(1, "Error getting account summaries", ex);
            }
        }
        #endregion

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            Text = String.Format("{0} {1}",
                 System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            BindingSource bindingSource = dataGridViewPositions.DataSource as BindingSource;
            bindingSource.Filter = checkBoxOptionsAndFutures.Checked ? "IsStock = false" : "";

            // initialize logging
            log4net.Config.XmlConfigurator.Configure();

            m_callback = new SentoniServiceCallback(this);
            m_context = new InstanceContext(m_callback);

            m_toolTipTexts = new ToolTipTexts(this);
            m_toolTip.AutoPopDelay = 30000;

            toolStripButtonEquityManager.Image = imageList1.Images[0];
            toolStripButtonTradeEntry.Image = imageList1.Images[1];
            toolStripButtonSnapshot.Image = imageList1.Images[2];
            toolStripButtonReports.Image = imageList1.Images[3];
            toolStripButtonGPARCalculator.Image = imageList1.Images[5];

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(MonitorHeartbeatThread);
            worker.RunWorkerAsync();

            m_newTradeCommand = new ToolStripMenuItem("Enter &new trade", null, new System.EventHandler(OnEnterNewTrade));
            m_editTradeCommand = new ToolStripMenuItem("&Edit or delete trade", null, new System.EventHandler(OnEditTrade));
            m_changeAccountCommand = new ToolStripMenuItem("&Change account", null, new System.EventHandler(OnChangeAccount));
            Disposed += new EventHandler(Form1_Disposed);
        }

        private void Form1_Disposed(object sender, EventArgs e)
        {
 	        if (m_menu != null)
                m_menu.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_formClosing = true;
            System.Threading.Thread.Sleep(2 * Properties.Settings.Default.HeartbeatTimer);
        }

        private void comboBoxAccounts_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ClearPortfolio();

            var account = comboBoxAccounts.SelectedItem;
            OnAccountChangeCommitted(account.ToString());
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAll();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            dataGridViewPositions.ClearSelection();
            dataGridViewTrades.ClearSelection();
            RefreshAll(true);
        }

        private void checkBoxAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoRefresh.Checked)
            {
                RefreshAll(true);
            }
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPageStressTest)
            {
                ExportStressTest();
            }
            else
            {
                ExportSnapshot();
            }
        }
 
       private void ExportSnapshot()
       {
            if (m_snapshot != null)
            {
                try
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = String.Format("{0} Portfolio {1:yyyyMMdd HHmm}.xlsx", AccountName, m_snapshot.TimeStamp);

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        m_snapshot.ExportToExcel(saveFileDialog1.FileName);
                        MessageBox.Show(String.Format("Exported portfolio to {0}", saveFileDialog1.FileName), "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting portfolio->" + ex.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       private void ExportStressTest()
       {
           try
           {
               SaveFileDialog saveFileDialog1 = new SaveFileDialog();

               saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
               saveFileDialog1.FilterIndex = 1;
               saveFileDialog1.RestoreDirectory = true;
               saveFileDialog1.FileName = String.Format("{0} Stress Test {1:yyyyMMdd HHmm}.xlsx", AccountName, m_snapshot.TimeStamp);

               if (saveFileDialog1.ShowDialog() == DialogResult.OK)
               {
                   m_stressTest.ExportToExcel(saveFileDialog1.FileName);
                   MessageBox.Show(String.Format("Exported stress test to {0}", saveFileDialog1.FileName), "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show("Error exporting stress test->" + ex.Message, "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
       }


        private void checkBoxTradesForAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewTrades.ClearSelection();
            GetBlotter();
        }

        private void checkBoxPositionsForAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            // if check box is not enabled, it was changed programmatically - no need to refresh grid
            if (checkBoxPositionsForAllAccounts.Enabled == true)
            {
                dataGridViewPositions.ClearSelection();
                GetPortfolio();
            }
        }

        private void dataGridViewSummaries_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewSummaries.Rows[e.RowIndex];
            if (!row.IsNewRow)
            {
                SelectAccount(row.Cells[accountNameDataGridViewTextBoxColumn1.Index].Value.ToString());
            }
        }

        private void dataGridViewSchedule_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridViewSchedule.Rows[e.RowIndex];
            if (!row.IsNewRow)
            {
                SelectAccount(row.Cells[accountNameDataGridViewTextBoxColumn2.Index].Value.ToString());
            }
        }

        private void buttonActiveAccount_Click(object sender, EventArgs e)
        {
            if (buttonActiveAccount.Tag != null)
                SelectAccount((string)buttonActiveAccount.Tag);
        }

        private void dataGridViewPositions_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (Performance.Index == e.ColumnIndex)
                    {
                        ChooseCellStyle(e, 0.0, Color.Black, Color.Red);
                    }
                    else if ((currentPositionDataGridViewTextBoxColumn.Index == e.ColumnIndex) && !IsNull(dataGridViewPositions[NettingAdjustment.Index, e.RowIndex].Value))
                    {
                        if (0 == (int)dataGridViewPositions[NettingAdjustment.Index, e.RowIndex].Value)
                            e.CellStyle.ForeColor = Color.Black;
                        else
                            e.CellStyle.ForeColor = Color.Red;
                    }
                    else if ((e.ColumnIndex >= 0)  && !IsNull(dataGridViewPositions[IsOutOfBounds.Index, e.RowIndex].Value))
                    {
                        if ((bool)dataGridViewPositions[IsOutOfBounds.Index, e.RowIndex].Value)
                            e.CellStyle.ForeColor = Color.Red;
                        else
                            e.CellStyle.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(7, String.Format("Exception on Positions, row {0}, column {1}", e.RowIndex, e.ColumnIndex), ex);
            }
        }

        private void dataGridViewPositionGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                {
                    switch (((PositionGridItem)e.Value).StrikeType)
                    {
                        case PositionGridItem.StrikeTypeEnum.ATTHEMONEY:
                            e.CellStyle.BackColor = Color.LightGreen;
                            break;
                        case PositionGridItem.StrikeTypeEnum.DOWN10PERCENT:
                        case PositionGridItem.StrikeTypeEnum.DOWN2PERCENT:
                            e.CellStyle.BackColor = Color.Yellow;
                            break;
                    }
                    e.CellStyle.ForeColor = ((PositionGridItem)e.Value).IsNegative ? Color.Red : Color.Black;
                }
            }
            catch (Exception ex)
            {
                ShowError(16, "Exception painting Position Grid DataGrid", ex);
            }
        }

        private void dataGridViewSummaries_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if ((e.RowIndex >= 0) && !IsNull(dataGridViewSummaries[accountNameDataGridViewTextBoxColumn1.Index, e.RowIndex].Value))
                {
                    if (totalPandLDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.Black, Color.Red);
                    if (ReturnOnEquity.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.ForestGreen, Color.Red);
                    if (NetReturnMTD.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.ForestGreen, Color.Red);
                    if (TargetReturn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.ForestGreen, Color.Red);
                    if (BenchmarkReturn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.ForestGreen, Color.Red);
                    if (BenchmarkReturnMTD.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.ForestGreen, Color.Red);
                    if (optionPandLDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.Black, Color.Red);
                    if (stockPandLDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.Black, Color.Red);
                    if (futuresPandLDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, 0.0, Color.Black, Color.Red);
                    if (nextTradeTimeDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                        ChooseCellStyle(e, DateTime.Now.TimeOfDay, Color.Black, Color.ForestGreen);

                    if (putsToTradeDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                    {
                        if (ChooseCellStyle(e, 0, Color.Black, Color.Red, true)) // red if < 0
                            if (!ChooseCellStyle(e, 1, Color.ForestGreen, Color.Black, true)) // forest green if >= 1
                                if ((bool)dataGridViewSummaries[PutsOutOfBounds.Index, e.RowIndex].Value) // orange if we have puts out of bounds
                                {
                                    e.CellStyle.BackColor = Color.DarkOrange;
                                }

                    }

                    if (totalDeltaPctDataGridViewTextBoxColumn.Index == e.ColumnIndex)
                    {
                        bool bShow = true;
                        if (!IsNull(dataGridViewSummaries[TradingComplete.Index, e.RowIndex].Value))
                        {
                            bShow = !(bool)dataGridViewSummaries[TradingComplete.Index, e.RowIndex].Value;
                        }
                        if (bShow)
                        {
                            if (ChooseCellStyle(e, (double?)dataGridViewSummaries[minDeltaDataGridViewTextBoxColumn.Index, e.RowIndex].Value, Color.Black, Color.ForestGreen, true))
                                ChooseCellStyle(e, (double?)dataGridViewSummaries[maxDeltaDataGridViewTextBoxColumn.Index, e.RowIndex].Value, Color.Red, Color.Black, true);
                        }
                    }

                    if (accountNameDataGridViewTextBoxColumn1.Index == e.ColumnIndex)
                    {
                        e.CellStyle.ForeColor = Color.Black;
                        if (!IsNull(dataGridViewSummaries[TargetReturn.Index, e.RowIndex].Value) && !IsNull(dataGridViewSummaries[ReturnOnEquity.Index, e.RowIndex].Value))
                        {
                            double expectedReturn = (double)dataGridViewSummaries[TargetReturn.Index, e.RowIndex].Value * (double)dataGridViewSummaries[TargetDelta.Index, e.RowIndex].Value;
                            if ((double)dataGridViewSummaries[ReturnOnEquity.Index, e.RowIndex].Value > expectedReturn)
                                e.CellStyle.ForeColor = Color.ForestGreen;
                            else if ((double)dataGridViewSummaries[ReturnOnEquity.Index, e.RowIndex].Value < expectedReturn)
                                e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(1, "Exception painting Account Summaries DataGrid", ex);
            }
        }

        private void dataGridViewSchedule_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var nextTradeTime = dataGridViewSchedule[NextTradeTime.Index, e.RowIndex].Value;
                    var endTradeTime = dataGridViewSchedule[EndTradeTime.Index, e.RowIndex].Value;

                    e.CellStyle.ForeColor = Color.Black;
                    if ((nextTradeTime != null) && (endTradeTime != null))
                    {
                        if (((TimeSpan)nextTradeTime <= DateTime.Now.TimeOfDay) && ((TimeSpan)endTradeTime > DateTime.Now.TimeOfDay))
                            e.CellStyle.ForeColor = Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(2, "Exception painting Trading Schedule DataGrid", ex);
            }
        }

        private void comboBoxSnapshots_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SnapshotId snapshotId = (SnapshotId)comboBoxSnapshots.SelectedItem;

                if (CurrentSnapshotId != snapshotId.Id)
                {
                    ClearPortfolio();
                    CurrentSnapshotId = snapshotId.Id;

                    if (snapshotId.Id == 0)
                    {
                        RefreshAll(true);
                    }
                    else
                    {
                        string msg = String.Format("Getting snapshot {0}...", snapshotId.Id);
                        ShowPortfolioInfo(msg);
                        ShowRiskInfo(msg);
                        m_proxy.GetPortfolioSnapshot(snapshotId.Id);

                        if ((tabControl1.SelectedTab != tabPageRisk) && (tabControl1.SelectedTab != tabPagePositions) && (tabControl1.SelectedTab != tabPageStressTest))
                        {
                            tabControl1.SelectedTab = tabPageRisk;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(6, "Exception selecting snapshot", ex);
            }


        }

        private void comboBoxSnapshots_DropDown(object sender, EventArgs e)
        {
            m_viewingSnapshotList = true;
        }

        private void comboBoxSnapshots_DropDownClosed(object sender, EventArgs e)
        {
            m_viewingSnapshotList = false;
        }

        private void label_MouseHover(object sender, EventArgs e)
        {
            string text = ToolTipTexts.GetText((Control)sender, EquityType, ClientType);
            m_toolTip.SetToolTip((Control)sender, text);
            m_toolTip.Show(text, ((Control)sender).Parent);


        }

        private void blotterDateTimePicker_CloseUp(object sender, EventArgs e)
        {
            GetBlotter();
        }

        private void checkBoxOptionsAndFutures_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewTrades.ClearSelection();
            BindingSource bindingSource = dataGridViewPositions.DataSource as BindingSource;
            bindingSource.Filter = checkBoxOptionsAndFutures.Checked ? "IsStock = false" : "";
        }
 
        private void toolStripButtonEquityManager_Click(object sender, EventArgs e)
        {
            string shortcutName =
                     string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                       Properties.Settings.Default.EquityManagerPath, ".appref-ms");
            try
            {
                string parameters = "";
                if (!String.IsNullOrEmpty(AccountName))
                {
                    parameters = String.Format(@"/a:{0}", AccountName);
                }
                System.Diagnostics.Process.Start(shortcutName, parameters);
            }
            catch (Exception ex)
            {
                ShowError(8, "Can't launch Equity Manager", ex);
            }
        }

        private void toolStripButtonSnapshot_Click(object sender, EventArgs e)
        {
            string acctName = AccountName;
            if (!String.IsNullOrEmpty(acctName))
            {
                try
                {
                    if (DialogResult.Yes == MessageBox.Show("Take a snapshot?", "Sentoni 2.0", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        m_proxy.TakeSnapshot(acctName);
                        MessageBox.Show(String.Format("Snapshot taken for account {0}", acctName), "Sentoni 2.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error taking snapshot for account {0}->{1}", acctName, ex.Message), "Sentoni 2.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonClearErrorMessage_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "";
            buttonClearErrorMessage.Visible = buttonClearErrorMessage.Enabled = false;
        }

        private void dataGridViewPositions_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex < 0) & (e.RowIndex >= 0))
                {
                    m_menu.Items.Clear();
                    m_menu.ShowImageMargin = false;

                    m_menu.Items.Add(m_newTradeCommand);
                    m_newTradeCommand.Tag = dataGridViewPositions.Rows[e.RowIndex];
                    e.ContextMenuStrip = m_menu;
                }

            }
            catch (Exception ex)
            {
                ShowError(10, "Error showing context menu", ex);
            }

        }

        private void dataGridViewTrades_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex < 0) & (e.RowIndex >= 0))
                {
                    m_menu.Items.Clear();
                    m_menu.ShowImageMargin = false;

                    if (CanEditTrade(dataGridViewTrades.Rows[e.RowIndex]))
                    {

                        m_menu.Items.Add(m_editTradeCommand);
                        m_editTradeCommand.Tag = dataGridViewTrades.Rows[e.RowIndex];
                    }
                    if (CanChangeAccount(dataGridViewTrades.Rows[e.RowIndex]))
                    {

                        m_menu.Items.Add(m_changeAccountCommand);
                        m_changeAccountCommand.Tag = dataGridViewTrades.Rows[e.RowIndex];
                    }

                    if (m_menu.Items.Count > 0)
                    {
                        e.ContextMenuStrip = m_menu;
                    }
                }

            }
            catch (Exception ex)
            {
                ShowError(17, "Error showing context menu", ex);
            }

        }

        private void OnEnterNewTrade(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
                LaunchTradeEntry(BuildNewTradeParms(row));
            }
            catch (Exception ex)
            {
                ShowError(11, "Error processing Enter New Trade command", ex);
            }
        }

        private void OnEditTrade(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
                LaunchTradeEntry(BuildEditTradeParms(row));
            }
            catch (Exception ex)
            {
                ShowError(18, "Error processing Edit Trade command", ex);
            }
        }

        private void OnChangeAccount(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = ((ToolStripMenuItem)sender).Tag as DataGridViewRow;
                LaunchTradeEntry(BuildChangeAccountParms(row));
            }
            catch (Exception ex)
            {
                ShowError(19, "Error processing Change Account command", ex);
            }
        }

        private void toolStripButtonTradeEntry_Click(object sender, EventArgs e)
        {
            string parms = null;
            if ((tabControl1.SelectedTab == tabPagePositions) && (dataGridViewPositions.SelectedRows.Count > 0))
            {
                parms = BuildNewTradeParms(dataGridViewPositions.SelectedRows[0]);
            }
            else if ((tabControl1.SelectedTab == tabPageBlotter) && (dataGridViewTrades.SelectedRows.Count > 0))
            {
                if (CanEditTrade(dataGridViewTrades.SelectedRows[0]))
                    parms = BuildEditTradeParms(dataGridViewTrades.SelectedRows[0]);
            }
            LaunchTradeEntry(parms);
        }

        private void toolStripButtonReports_Click(object sender, EventArgs e)
        {
            string parms = String.Format("/a:{0},/r:Daily%Performance", AccountName.Replace(' ', '%'));
            LaunchReports(parms);
        }
 
        //private void toolStripButtonPutCalculator_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        FormPutCalculator form = new FormPutCalculator(m_snapshot, LogError);
        //        form.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowError(12, "Error loading put calculator", ex);
        //    }

        //}

        private void buttonStressTestLoadPositions_Click(object sender, EventArgs e)
        {
            if (m_snapshot != null)
            {
                RefreshStressTest(m_snapshot);
            }
        }

        private void buttonStressTestCalculate_Click(object sender, EventArgs e)
        {
            labelStressTestCurrentMarketValue.Visible = false;
            labelStressTestNewMarketValue.Visible = false;
            labelStressTestPandL.Visible = false;
            labelStressTestPercent.Visible = false;
            ShowStressTestInfo("");

            if (VerifyStressTestInput())
            {
                RunStressTest();
            }
        }

        private void toolStripButtonGPARCalculator_Click(object sender, EventArgs e)
        {
            try
            {
                     GPARCalculator form = new GPARCalculator();
                    if (m_snapshot != null)
                     {
                         if (m_snapshot.AccountData.Count > 0)
                         {
                             form.Equity = m_snapshot.AccountData[0].CurrentEquity;
                             form.Multiplier = m_snapshot.AccountData[0].PortfolioPercentage;
                             if (Math.Abs(m_snapshot.AccountData[0].DeltaGoal) > .0001)
                             {
                                 form.TargetDelta = m_snapshot.AccountData[0].DeltaGoal;
                             }
                             else
                             {
                                 form.TargetDelta = m_snapshot.AccountData[0].TargetDelta;
                             }
                          }
                         if (m_snapshot.Indices.Count > 0)
                         {
                             form.IndexPrice = m_snapshot.Indices[0].LastPrice;
                             form.DeltasToTrade = m_snapshot.Indices[0].DeltasToTrade;
                             form.TotalDeltaPct = m_snapshot.Indices[0].TotalDeltaPct;
                         }
                         if (m_snapshot.Positions != null)
                         {
                             string shortPut = null;
                             string shortCall = null;
                             int shortPutPosition = 0;
                             double shortPutDelta = 0;
                             double longPutDelta = 0;
                             // find the short put
                             foreach (AccountDataSet.PortfolioRow row in m_snapshot.Positions)
                             {
                                 if (row.IsOption && (row.OptionType == "Put") && (row.Current_Position < 0))
                                 {
                                     shortPut = row.Symbol;
                                     shortPutPosition = row.Current_Position;
                                     shortPutDelta = row.Delta;
                                     break;
                                 }
                             }
                             // find the long put
                             foreach (AccountDataSet.PortfolioRow row in m_snapshot.Positions)
                             {
                                 if (row.IsOption && (row.OptionType == "Put") && (row.Current_Position > 0))
                                 {
                                     longPutDelta = row.Delta;
                                     break;
                                 }
                             }
                             // find the calls
                             if (longPutDelta * shortPutDelta != 0)
                             {
                                 shortCall = shortPut.Substring(0, 6) + shortPut.Substring(6, shortPut.Length - 6).Replace('P', 'C');
                                 int i = 1;
                                 foreach (AccountDataSet.PortfolioRow row in m_snapshot.Positions)
                                 {
                                     if (row.IsOption && (row.OptionType == "Call") && (row.Current_Position < 0))
                                     {
                                         SetGPARCallPosition(i, form, row.Symbol, row.Current_Position + ((row.Symbol == shortCall) ? shortPutPosition : 0), row.Delta);
                                         if (++i > 5)
                                             break;
                                     }
                                 }

 //                              SetGPARCallPosition(2, form, "EEM   181221C00072000", -50, .3);  // for testing

                                 form.VerticalDelta = (longPutDelta - shortPutDelta);
                                 form.NumVerticals = - shortPutPosition;
                                 form.OTMPutDelta = longPutDelta;
                             }
                         }
                     }

                    form.Show();
             }
            catch (Exception ex)
            {
                ShowError(12, "Error in GPAR calculator", ex);
            }
        }

        private void SetGPARCallPosition(int i, GPARCalculator form, string symbol, int position, double delta)
        {
            switch (i)
            {
                case 1:
                    form.Call1 = symbol;
                    form.Call1Position = position;
                    form.Call1Delta = delta;
                    break;
                case 2:
                    form.Call2 = symbol;
                    form.Call2Position = position;
                    form.Call2Delta = delta;
                    break;
                case 3:
                    form.Call3 = symbol;
                    form.Call3Position = position;
                    form.Call3Delta = delta;
                    break;
                case 4:
                    form.Call4 = symbol;
                    form.Call4Position = position;
                    form.Call4Delta = delta;
                    break;
                case 5:
                    form.Call5 = symbol;
                    form.Call5Position = position;
                    form.Call5Delta = delta;
                    break;
            }
        }
        #endregion

        private bool VerifyStressTestInput()
        {
            try
            {
                double interestRate;
                if (!Double.TryParse(textBoxInterestRate.Text, out interestRate))
                {
                    ShowStressTestInfo("Invalid interest rate");
                    return false;
                }
                if ((interestRate < 0) || (interestRate > 100))
                {
                    ShowStressTestInfo("Invalid interest rate");
                    return false;
                }
                m_stressTest.RiskFreeInterestRate = interestRate / 100;

                double changeInMarket;
                if (!Double.TryParse(textBoxChangeInMarket.Text, out changeInMarket))
                {
                    ShowStressTestInfo("Invalid change in market");
                    return false;
                }
                if ((changeInMarket < -100) || (changeInMarket > 1000))
                {
                    ShowStressTestInfo("Invalid change in market");
                    return false;
                }
                m_stressTest.ChangeInMarket = changeInMarket / 100;


                double changeInVolatility;
                if (!Double.TryParse(textBoxChangeInVolatility.Text, out changeInVolatility))
                {
                    ShowStressTestInfo("Invalid change in volatility");
                    return false;
                }
                if ((changeInVolatility < -100) || (changeInVolatility > 1000))
                {
                    ShowStressTestInfo("Invalid change in volatility");
                    return false;
                }
                m_stressTest.ChangeInVolatility = changeInVolatility / 100;

                return true;
            }
            catch (Exception ex)
            {
                ShowStressTestError(2, "Error verifying input-", ex);
                return false;
            }
        }

        private void RunStressTest()
        {
            try
            {
                if (m_stressTest.RunTest())
                {
                    labelStressTestCurrentMarketValue.Text = String.Format("{0:C0}", m_stressTest.CurrentMarketValue);
                    labelStressTestNewMarketValue.Text = String.Format("{0:C0}", m_stressTest.NewMarketValue);
                    labelStressTestPandL.Text = String.Format("{0:C0}", m_stressTest.NewMarketValue - m_stressTest.CurrentMarketValue);
                    labelStressTestPercent.Text = String.Format("{0:P1}", m_stressTest.PercentPandL);

                    labelStressTestCurrentMarketValue.Visible = true;
                    labelStressTestNewMarketValue.Visible = true;
                    labelStressTestPandL.Visible = true;
                    labelStressTestPercent.Visible = true;
                }
                else
                {
                    ShowStressTestInfo("Error running stress test-" + m_stressTest.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ShowStressTestError(3, "Error running stress test-", ex);
            }
        }
 
        private void buttonPositionGridShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPositionGridIndex.SelectedIndex < 0)
                {
                    ShowError(13, "No index chosen");
                    return;
                }
                double strikeIncrement;
                if (!double.TryParse(textBoxPositionGridIncrement.Text, out strikeIncrement))
                {
                    ShowError(14, "Invalid strike increment " + textBoxPositionGridIncrement.Text);
                    return;
                }

                PositionGridTable table = new PositionGridTable((AccountDataSet.IndicesRow)comboBoxPositionGridIndex.SelectedItem, strikeIncrement);
                table.BuildTable(m_snapshot);
                dataGridViewPositionGrid.DataSource = table;
                ShowInfo(String.Format("Position grid updated at {0:T}", DateTime.Now));
            }
            catch (Exception ex)
            {
                ShowError(20, "Error building position grid", ex);
            }
            
        }

        private void bloombergMenuItem_Click(object sender, EventArgs e)
        {
            ibMenuItem.Checked = false;
            SwitchQuoteFeed(Properties.Settings.Default.BloombergHost);
        }

        private void ibMenuItem_Click(object sender, EventArgs e)
        {
            bloombergMenuItem.Checked = false;
            SwitchQuoteFeed(Properties.Settings.Default.IBHost);
           
         }

        private void SwitchQuoteFeed(string quoteFeedHost)
        {
            try
            {
                if (m_serviceStatus == ServiceStatus.Up)
                {
                    m_proxy.SwitchQuoteFeed(quoteFeedHost);
                }
            }
            catch (Exception ex)
            {
                ShowError(21, "Error switching quote feed", ex);
            }
        }
    }
}
