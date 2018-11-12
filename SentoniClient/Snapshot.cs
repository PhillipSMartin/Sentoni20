using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentoniClient
{
    public class Snapshot
    {
        public string AccountName { get; set; }

        public AccountDataSet.PortfolioDataTable Positions { get; set; }
        public AccountDataSet.AccountDataDataTable AccountData { get; set; }
        public AccountDataSet.IndicesDataTable Indices { get; set; }

        public SentoniServiceReference.SnapshotType SnapshotType { get; set; }
        public DateTime TimeStamp { get; set; }
        public SnapshotId[] SavedSnapshots { get; set; }

        public string QuoteServiceHost { get; set; }
        public TimeSpan? QuoteServiceStoppedTime { get; set; }
        public int UnsubscribedSymbols { get; set; }

        public override string ToString()
        {
            if (SnapshotType == SentoniServiceReference.SnapshotType.Current)
                return String.Format("Last update at {0:T}", TimeStamp);
            else
                return String.Format("{0} snapshot taken at {1:g}", SnapshotType.ToString(), TimeStamp);
        }

        public void ExportToExcel(string m_exportFileName)
        {
            WorkbookGenerator workbookGenerator = new WorkbookGenerator();
            workbookGenerator.AddWorksheet(Indices);
            workbookGenerator.AddWorksheet(Positions);
            workbookGenerator.AddWorksheet(AccountData);
            workbookGenerator.SaveWorkbook(m_exportFileName);
        }
    }
}
