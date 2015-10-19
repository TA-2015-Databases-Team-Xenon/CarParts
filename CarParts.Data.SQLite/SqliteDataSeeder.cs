namespace CarParts.Data.SQLite
{
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;

    using Models.SQLiteModels;

    public class SqliteDataSeeder
    {
        private const string ConnectionString = @"Data Source=../../../DataSources/sqliteDb.sqlite;Version=3;";

        public void Seed(IEnumerable<string> partNames)
        {
            var con = new SQLiteConnection(ConnectionString);
            con.Open();


            using (con)
            {
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

    }
}
