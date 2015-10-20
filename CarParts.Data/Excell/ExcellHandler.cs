namespace CarParts.Data.Excell
{
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.IO;
    using System.Linq;

    using ClosedXML.Excel;
    using Data.MySql;
    using Data.SqlServer;
    using Models;
    using Models.SQLiteModels;

    public class ExcellHandler
    {
        private const string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\'Excel 12.0 xml;HDR=Yes\';";
        private const string DefaultExcellFilesPath = "..\\..\\..\\DataSources\\SalesReports";
        private const string DefaultExcellOutputPath = "..\\..\\..\\DataOutput\\";

        private string filePath;
        private string outputFilePath;

        public ExcellHandler()
            : this(DefaultExcellFilesPath, DefaultExcellOutputPath)
        {
        }

        public ExcellHandler(string filePath, string outputFilePath)
        {
            this.filePath = filePath;
            this.outputFilePath = outputFilePath;
        }

        public void MigrateFromExcellToSqlServer()
        {
            var excellFilesNames = Directory.GetFileSystemEntries(this.filePath, "*.xls", SearchOption.AllDirectories);

            foreach (var fileName in excellFilesNames)
            {
                var sales = this.ReadSalesFromExcellSheet(fileName);
                this.WriteSalesToSqlServer(sales);
            }
        }

        public void GenerateExcellFile(IEnumerable<PartReportInputModel> reports, IEnumerable<ProductTax> taxes, string fileName)
        {
            var vendorsFinancialData = from r in reports
                                       join t in taxes
                                       on r.PartName equals t.ProductName
                                       select new VendorFinancialData
                                       {
                                           VendorName = r.Vendor,
                                           Income = r.TotalPrice,
                                           Tax = (decimal)(t.Tax * r.Quantity)
                                       };

            var vendorFinancialReports = vendorsFinancialData.GroupBy(d => d.VendorName)
                                                             .Select(gr => new VendorFinancialReport
                                                             {
                                                                 VendorName = gr.Key,
                                                                 Income = gr.Sum(g => g.Income),
                                                                 Tax = gr.Sum(g => g.Tax),
                                                                 Profit = gr.Sum(g => g.Income) - gr.Sum(g => g.Tax)
                                                             })
                                                             .ToList();                       

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Vendors Financial Report");

            ws.Cell("B2").Value = "Vendor";
            ws.Cell("C2").Value = "Incomes";
            ws.Cell("D2").Value = "Taxes";
            ws.Cell("E2").Value = "Profit";

            int rowCount = 3;
            foreach (var report in vendorFinancialReports)
            {
                string vendorCell = "B" + rowCount;
                string incomesCell = "C" + rowCount;
                string taxesCell = "D" + rowCount;
                string profitCell = "E" + rowCount;

                ws.Cell(vendorCell).Value = report.VendorName;
                ws.Cell(incomesCell).Value = report.Income;
                ws.Cell(taxesCell).Value = report.Tax;
                ws.Cell(profitCell).Value = report.Profit;

                rowCount++;
            }

            var tableRange = ws.Range("B2", "E" + (rowCount - 1));
            var financialReportTable = tableRange.CreateTable();
            financialReportTable.Theme = XLTableTheme.TableStyleMedium16;

            ws.Columns().AdjustToContents();
            wb.SaveAs(this.outputFilePath + fileName);
        }

        private IEnumerable<Sale> ReadSalesFromExcellSheet(string path)
        {
            var sales = new List<Sale>();
            string connectionString = string.Format(ConnectionString, path);
            var excellCon = new OleDbConnection(connectionString);
            excellCon.Open();

            using (excellCon)
            {
                var excelSchema = excellCon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();

                var excelDbCommand = new OleDbCommand(@"SELECT * FROM [" + sheetName + "]", excellCon);

                var sale = new Sale();
                using (var reader = excelDbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sale.Id = int.Parse(reader["SaleId"].ToString());
                        sale.PartId = int.Parse(reader["ProductId"].ToString());
                        sale.Quantity = int.Parse(reader["Quantity"].ToString());
                        sale.UnitPrice = decimal.Parse(reader["UnitPrice"].ToString());
                        sale.Date = (DateTime)reader["Date"];
                        sale.VendorId = int.Parse(reader["VendorId"].ToString());

                        sales.Add(sale);
                        sale = new Sale();
                    }
                }
            }

            return sales;
        }

        private void WriteSalesToSqlServer(IEnumerable<Sale> sales)
        {
            using (var db = new CarPartsDbContext())
            {
                foreach (var sale in sales)
                {
                    db.Sales.Add(sale);
                }

                db.SaveChanges();
            }
        }
    }
}
