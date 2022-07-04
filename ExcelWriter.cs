using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System.Data;

namespace AnotherExcelWriterPOC
{
    public static class ExcelWriter
    {
        public static void CreateExcelFile()
        {
            
            var users = GetUserDetails(2000);

            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(users), (typeof(DataTable)));

            using (SpreadsheetDocument document = SpreadsheetDocument.Create("TestNewData.xlsx", SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Resultados" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<string> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }

        }

        private static List<UserDetails> GetUserDetails(int quantity)
        {
            List<UserDetails> data = new();

            for (var i = 0; i < quantity; i++)
            {
                data.Add(new UserDetails() { ID = i, City = $"City {i}", Country = $"Country {i}", Name = $"Name {i}" });
            }

            return data;
        }
    }
}
