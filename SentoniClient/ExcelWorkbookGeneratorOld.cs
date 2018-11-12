using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarlosAg.ExcelXmlWriter;

namespace SentoniClient
{
    public class ExcelWorkbookGeneratorOld
    {
        Workbook m_workbook = new Workbook();

        public ExcelWorkbookGenerator()
        {
            CreateWorkbookStyles();
        }

        #region Public methods
        public void AddWorksheet(DataTable dataTable)
        {
            string cellValue;
            DataType cellDataType;
            string cellFormat;

            string worksheetName = dataTable.TableName;
            if (worksheetName.Length > 31)
                worksheetName = worksheetName.Substring(0, 31);
            Worksheet worksheet = m_workbook.Worksheets.Add(worksheetName);

            WorksheetRow header = worksheet.Table.Rows.Add();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                header.Cells.Add(new WorksheetCell(dataColumn.ColumnName, DataType.String, "Header"));
                worksheet.Table.Columns.Add(new WorksheetColumn(dataColumn.ColumnName.Length * 7));
            }
            foreach (DataRow dataRow in dataTable.Rows)
            {
                WorksheetRow worksheetRow = worksheet.Table.Rows.Add();
                int column = 0;
                foreach (object dataCell in dataRow.ItemArray)
                {
                    Type dataType = dataTable.Columns[column].DataType;
                    GetCellData(dataCell, dataType, out cellValue, out cellDataType, out cellFormat);
                    worksheetRow.Cells.Add(new WorksheetCell(cellValue, cellDataType, cellFormat));
                    column++;
                }
            }
        }
        public void SaveWorkbook(string fileName)
        {
            m_workbook.Save(fileName);
        }
        #endregion

        #region Private methods
        private void CreateWorkbookStyles()
        {
            WorksheetStyle styleHeader = new WorksheetStyle("Header");
            styleHeader.Font.FontName = "Tahoma";
            styleHeader.Font.Bold = true;
            styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m_workbook.Styles.Add(styleHeader);

            WorksheetStyle style0 = new WorksheetStyle("Default");
            m_workbook.Styles.Add(style0);

            WorksheetStyle style1 = new WorksheetStyle("DateTime");
            style1.NumberFormat = "mm/dd/yy";
            m_workbook.Styles.Add(style1);

            WorksheetStyle style2 = new WorksheetStyle("F2");
            style2.NumberFormat = "0.00";
            m_workbook.Styles.Add(style2);

            WorksheetStyle style3 = new WorksheetStyle("F3");
            style3.NumberFormat = "0.000";
            m_workbook.Styles.Add(style3);

            WorksheetStyle style4 = new WorksheetStyle("Currency");
            style4.NumberFormat = "Currency";
            m_workbook.Styles.Add(style4);
        }

        private static void GetCellData(object dataCell, Type dataType, out string cellValue, out DataType cellDataType, out string cellFormat)
        {
            cellDataType = ConvertCellDataType(dataType);
            cellValue = dataCell.ToString();
            cellFormat = ConvertCellFormat(dataType);
        }

        //private static string ConvertCellValue(object cellValue, Type cellDataType)
        //{
        //    if (cellDataType == typeof(DateTime))
        //        return Convert.ToDateTime(cellValue).ToOADate().ToString();
        //    else
        //        return Convert.ToString(cellValue);
        //}
        private static DataType ConvertCellDataType(Type cellDataType)
        {
            if ((cellDataType == typeof(double)) || (cellDataType == typeof(decimal)) || (cellDataType == typeof(int)) || (cellDataType == typeof(short)) || (cellDataType == typeof(DateTime)))
            {
                return DataType.Number;
            }
            else
            {
                return DataType.String;
            }
        }

        private static string ConvertCellFormat(Type cellDataType)
        {
            string cellFormat = "Default";

            if (cellDataType == typeof(DateTime))
            {
                cellFormat = "DateTime";
            }
            else if ((cellDataType == typeof(double)) || (cellDataType == typeof(decimal)))
            {
                 cellFormat = "F3";
            }
            return cellFormat;
        }
        #endregion
    }

 }
