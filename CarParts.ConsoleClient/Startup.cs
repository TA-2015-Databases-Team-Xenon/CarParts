namespace CarParts.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.Excell;
    using Data.MongoDb;
    using Data.MySql;
    using Data.Pdf;
    using Data.SQLite;
    using Data.SqlServer;
    using Data.Xml;
    using Models;
    using Utils;

    public class Startup
    {
        public static void Main()
        {
            //var mongoHandler = new MongoDbHandler();

            // Mongolab.com credentials - Username: TeamXenon , Passsword: xenon123
            //// These tasks are done. You can physically delete the data from your SqlServerDatabase and the extracted excell files and run this method again.
            //ProblemOne(mongoHandler);

            //ProblemTwo();

            var report = new PartReportInputModel
            {
                PartId = 2,
                PartName = "Shlyokavica",
                Price = 14,
                Quantity = 1,
                Vendor = "Bai Iliq"
            };

            List<PartReportInputModel> reports = new List<PartReportInputModel>(){report};

            new MySqlHandler().WriteReports(reports);

            //// This will throw because coutries are already added to the cloud and have unique constraint to their name in sql server.
            //// You can delete physically the data and run the method if you like.
            //ProblemFive(mongoHandler);
            
            //var partNames = new List<string>();
            //using (var db = new CarPartsDbContext())
            //{
            //    partNames = db.Parts.Select(p => p.Name).ToList();
            //}

            //new SqliteHandler().Seed(partNames);

            
        }

        private static void ProblemTwo()
        {
            new PdfHandler().GenerateSalesInfoPdf();
        }

        private static void ProblemFive(MongoDbHandler mongoHandler)
        {
            var xmlHandler = new XmlHandler();
            var countries = xmlHandler.GetCountries();
            
            new XmlToSqlServerLoader().LoadCountries(countries);
            mongoHandler.WriteCollection<XmlCountry>("Countries", countries);
            Console.WriteLine("successfully write down countries to MongoDB and SqlServer databases");
        }

        private static void ProblemOne(MongoDbHandler mongoHandler)
        {
            // Populating database in MongoLab with some useless testing data
            try
            {                
              new MongoDataSeeder().Seed(mongoHandler);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Just in case you uncomment and try to populate the cloud database with existing id's and it throws :)");
            }

            new MongoToSqlServerLoader().Load(mongoHandler);
            Console.WriteLine("successsfully added all in a sql database");

            new ZipExtractor().Extract();
            Console.WriteLine("Files extracted successfully.");

            new ExcellHandler().MigrateFromExcellToSqlServer();
            Console.WriteLine("successfully added sales table from excell files.");
        }
    }
}
