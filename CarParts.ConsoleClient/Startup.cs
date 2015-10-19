namespace CarParts.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.Excell;
    using Data.MongoDb;
    using Data.SQLite;
    using Data.SqlServer;
    using Data.Xml;
    using Models;
    using Utils;

    public class Startup
    {
        public static void Main()
        {
            var mongoHandler = new MongoDbHandler();
            //// Populating database in MongoLab with some useless testing data
            //try
            //{                
            //  new MongoDataSeeder().Seed(mongoHandler);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("Just in case you uncomment and try to populate the cloud database with existing id's and it throws :)");
            //}

            //new MongoToSqlServerLoader().Load(mongoHandler);
            //Console.WriteLine("successsfully added all in a sql database");

            // Commented because files are already extracted in the folder.
            //new ZipExtractor().Extract();
            //Console.WriteLine("Files extracted successfully.");

            //new ExcellHandler().MigrateFromExcellToSqlServer();
            //Console.WriteLine("successfully added sales table from excell files.");

            //var xmlHandler = new XmlHandler();
            //var countries = xmlHandler.GetCountries();

            //// These two will throw because coutries are already added to the cloud and have unique constraint to their name in sql server
            //new XmlToSqlServerLoader().LoadCountries(countries);
            //mongoHandler.WriteCollection<XmlCountry>("Countries", countries);
            //Console.WriteLine("successfully write down countries to MongoDB and SqlServer databases");
            var partNames = new List<string>();
            using (var db = new CarPartsDbContext())
            {
                partNames = db.Parts.Select(p => p.Name).ToList();
            }

            new SqliteHandler().Seed(partNames);

        }
    }
}
