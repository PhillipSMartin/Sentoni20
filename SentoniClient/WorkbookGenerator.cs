using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace SentoniClient
{
    class WorkbookGenerator
    {
        Excel.Application m_excelApp = new Excel.Application();
        Excel.Workbook m_workbook;

        public WorkbookGenerator()
        {
            m_workbook = m_excelApp.Workbooks.Add();
        }

        public void AddWorksheet(DataTable dataTable)
        {
            if (dataTable != null)
            {
                Excel.Worksheet worksheet = m_workbook.Sheets.Add();
                worksheet.Name = dataTable.TableName;
                int rowNumber = 0;
                int columnNumber = 0;

                // column headings
                for (columnNumber = 0; columnNumber < dataTable.Columns.Count; columnNumber++)
                {
                    worksheet.Cells[1, columnNumber + 1] = dataTable.Columns[columnNumber].ColumnName;
                }

                Excel.Range headerRange = worksheet.get_Range((Excel.Range)(worksheet.Cells[1, 1]), (Excel.Range)(worksheet.Cells[1, dataTable.Columns.Count]));
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                headerRange.Font.Bold = true;

                try
                {
                    // rows
                    for (rowNumber = 0; rowNumber < dataTable.Rows.Count; rowNumber++)
                    {
                        // to do: format datetime values before printing
                        for (columnNumber = 0; columnNumber < dataTable.Columns.Count; columnNumber++)
                        {
                            if (!Convert.IsDBNull(dataTable.Rows[rowNumber][columnNumber]))
                            {
                                if (dataTable.Columns[columnNumber].DataType == typeof(TimeSpan))
                                    worksheet.Cells[rowNumber + 2, columnNumber + 1] = dataTable.Rows[rowNumber][columnNumber].ToString();
                                else
                                    worksheet.Cells[rowNumber + 2, columnNumber + 1] = dataTable.Rows[rowNumber][columnNumber];
                            }
                        }
                    }
                    worksheet.Columns.AutoFit();
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Error on table {0}, row {1}, column {2}: {3}", dataTable.TableName, rowNumber + 1, dataTable.Columns[columnNumber].ColumnName, ex.Message));
                }
            }
        }

        public void SaveWorkbook(string fileName)
        {
            if (m_workbook != null)
            {
                try
                {
                    m_workbook.SaveAs(fileName);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error saving file: " + ex.Message);
                }
                finally
                {
                    m_workbook.Close();
                }
            }
        }
    }
}
