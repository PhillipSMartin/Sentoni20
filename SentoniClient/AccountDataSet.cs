
using System;
namespace SentoniClient {
    
    
    public partial class AccountDataSet {

        public static StressTestDataTable BuildStressTest(string accountName, PortfolioDataTable portfolio)
        {
            StressTestDataTable stressTestTable = new StressTestDataTable();
            if (portfolio != null)
            {
                foreach (PortfolioRow portfolioRow in portfolio)
                {
                    if (portfolioRow.AccountName == accountName)
                    {
                        try
                        {
                            StressTestRow newRow = stressTestTable.NewStressTestRow();
                            newRow.AccountName = portfolioRow.AccountName;
                            newRow.Symbol = portfolioRow.Symbol;
                            newRow.UnderlyingSymbol = portfolioRow.UnderlyingSymbol;
                            if (portfolioRow.IsOption)
                            {
                                newRow.ExpirationDate = portfolioRow.ExpirationDate;
                                newRow.StrikePrice = portfolioRow.StrikePrice;
                                newRow.OptionType = portfolioRow.OptionType;
                                newRow.Delta = portfolioRow.Delta;
                                newRow.ImpliedVol = portfolioRow.ImpliedVol;
                            }
                            newRow.Multiplier = portfolioRow.Multiplier;
                            newRow.Current_Position = portfolioRow.Current_Position;
                            newRow.Current_Price = portfolioRow.Current_Price;
                            newRow.Current_Market_Value = portfolioRow.Current_Market_Value;
                            newRow.IsStock = portfolioRow.IsStock;
                            newRow.IsOption = portfolioRow.IsOption;
                            newRow.IsFuture = portfolioRow.IsFuture;

                            stressTestTable.Rows.Add(newRow);
                        }
                        catch(Exception )
                        {
                            // if there is a problem building the row, don't add it
                        }
                    }
                }
            }

            return stressTestTable;
        }
    }
}
