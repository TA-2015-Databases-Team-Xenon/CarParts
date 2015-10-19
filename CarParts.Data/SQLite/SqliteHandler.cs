namespace CarParts.Data.SQLite
{
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;

    using Models.SQLiteModels;

    public class SqliteHandler
    {
        private const string ConnectionString = @"Data Source=../../../DataSources/sqliteDb.sqlite;Version=3;";

        public void Seed(IEnumerable<string> partNames)
        {
            var con = new SQLiteConnection(ConnectionString);
            con.Open();

            using (con)
            {
                var deleteCommandString = @"DELETE FROM ProductTaxes";
                var deleteCommand = new SQLiteCommand(deleteCommandString, con);
                deleteCommand.ExecuteNonQuery();

                var sqlCommandString = @"INSERT INTO ProductTaxes (Id, ProductName, Tax) VALUES (@id, @name, @tax)";
                var sqlCommand = new SQLiteCommand(sqlCommandString, con);
                int counter = 1;
                foreach (var name in partNames)
                {
                    sqlCommand.Parameters.AddWithValue("@id", counter);
                    sqlCommand.Parameters.AddWithValue("@name", name);
                    sqlCommand.Parameters.AddWithValue("@tax", 20 + counter / 5);

                    sqlCommand.ExecuteNonQuery();

                    counter++;
                }
            }
        }

        public IEnumerable<ProductTax> ReadTaxes()
        {
            var productTaxes = new List<ProductTax>();

            var con = new SQLiteConnection(ConnectionString);
            con.Open();

            using (con)
            {
                var sqlCommandString = @"SELECT Id, ProductName, Tax FROM ProductTaxes";
                var sqlCommand = new SQLiteCommand(sqlCommandString, con);

                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string name = (string)reader["ProductName"];
                        double tax = (double)reader["Tax"];

                        var taxItem = new ProductTax(id, name, tax);
                        productTaxes.Add(taxItem);
                    }
                }
            }

            return productTaxes;
        }

    }
}
