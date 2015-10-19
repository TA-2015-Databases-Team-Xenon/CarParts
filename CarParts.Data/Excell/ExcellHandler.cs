namespace CarParts.Data.Excell
{
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.IO;
    using System.Linq;

    using Data.SqlServer;
    using Models;

    public class ExcellHandler
    {
        private const string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\'Excel 12.0 xml;HDR=Yes\';";
        private const string DefaultExcellFilesPath = "..\\..\\..\\DataSources\\SalesReports";

        private string filePath;

        public ExcellHandler() 
            : this(DefaultExcellFilesPath)
        {
        }

        public ExcellHandler(string filePath)
        {
            this.filePath = filePath;
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

        // Here somewhere in some public void GenerateExcell method the generating of excell file for task 6 will be done.

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
