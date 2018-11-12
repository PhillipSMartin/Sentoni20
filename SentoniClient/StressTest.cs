using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackScholesModel;

namespace SentoniClient
{
    public class StressTest
    {
        private Model m_model = new BlackScholesModel.Model();

        public double RiskFreeInterestRate { get; set; }
        public double ChangeInMarket { get; set; }
        public double ChangeInVolatility { get; set; }
        public double CurrentEquity { get; set; }
 
        public double CurrentMarketValue { get; private set; }
        public double NewMarketValue { get; private set; }

        public string ErrorMessage { get; private set; }
        public double PercentPandL
        {
            get
            {
                if (CurrentEquity > 0)
                {
                    return (NewMarketValue - CurrentMarketValue) / CurrentEquity;
                }
                else if (CurrentMarketValue != 0)
                {
                    return (NewMarketValue - CurrentMarketValue) / CurrentMarketValue;
                }
                else return 0;
            }
        }

        public AccountDataSet.StressTestDataTable StressTestDataTable { get; set; }
        public AccountDataSet.IndicesDataTable Indices { get; set; }

        // returns true on success
        public bool RunTest()
        {
            try
            {
                CurrentMarketValue = NewMarketValue = 0;

                foreach (AccountDataSet.StressTestRow row in StressTestDataTable)
                {
                    if (row.IsOption && (row.Current_Position != 0))
                    {
                        double daystoexpiration = row.ExpirationDate.AddHours(16).ToOADate() - DateTime.Now.ToOADate();
                        if (daystoexpiration > 0)
                        {
                            AccountDataSet.IndicesRow index = Indices.FindBySymbol(row.UnderlyingSymbol);
                            if (index != null)
                            {
                                row.Current_Theoretical = m_model.GetPrice((row.OptionType == "Call") ? OptionTypeEnum.CallOption : OptionTypeEnum.PutOption,
                                    index.LastPrice,
                                    (double)row.StrikePrice,
                                    daystoexpiration / 365,
                                    RiskFreeInterestRate / 100,
                                    row.ImpliedVol / 100);

                                row.New_Theoretical = m_model.GetPrice((row.OptionType == "Call") ? OptionTypeEnum.CallOption : OptionTypeEnum.PutOption,
                                    index.LastPrice * (1 + ChangeInMarket),
                                    (double)row.StrikePrice,
                                    daystoexpiration / 365,
                                    RiskFreeInterestRate / 100,
                                    row.ImpliedVol * (1 + ChangeInVolatility) / 100);

                            }
                        }
                    }
                    else
                    {
                        row.Current_Theoretical = row.Current_Price;
                        row.New_Theoretical = row.Current_Price * (1 + ChangeInMarket);
                    }

                    if (!row.IsCurrent_TheoreticalNull())
                    {
                        row.Current_Market_Value = row.Current_Position * row.Current_Theoretical * row.Multiplier;
                        CurrentMarketValue += row.Current_Market_Value;

                        row.New_Market_Value = row.Current_Position * row.New_Theoretical * row.Multiplier;
                        NewMarketValue += row.New_Market_Value;

                        row.P_and_L = row.New_Market_Value - row.Current_Market_Value;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

            return true;
        }

        public void ExportToExcel(string m_exportFileName)
        {
            AccountDataSet.StressTestSummaryDataTable summaryTable = new AccountDataSet.StressTestSummaryDataTable();
            summaryTable.AddStressTestSummaryRow(RiskFreeInterestRate, ChangeInMarket, ChangeInVolatility, CurrentEquity, CurrentMarketValue, NewMarketValue, NewMarketValue - CurrentMarketValue, PercentPandL);

            WorkbookGenerator workbookGenerator = new WorkbookGenerator();
            workbookGenerator.AddWorksheet(Indices);
            workbookGenerator.AddWorksheet(StressTestDataTable);
            workbookGenerator.AddWorksheet(summaryTable);
            workbookGenerator.SaveWorkbook(m_exportFileName);
        }
    }
}
