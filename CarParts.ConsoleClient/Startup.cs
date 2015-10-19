namespace CarParts.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.Excell;
    using Data.Json;
    using Data.MongoDb;
    using Data.MySql;
    using Data.Pdf;
    using Data.SQLite;
    using Data.Xml;
    using Models;
    using Utils;
    using CarParts.Data.SqlServer;

    public class Startup
    {
        public static void Main()
        {
            var mongoHandler = new MongoDbHandler();
            var sqlHandler = new SqlServerHandler();
            var mySqlHandler = new MySqlHandler();
            var pdfHandler = new PdfHandler();
            var xmlToSql = new XmlToSqlServerLoader();
            var excellHandler = new ExcellHandler();
            var mongoToSql = new MongoToSqlServerLoader();
            var zipExtractor = new ZipExtractor();
            var jsonHandler = new JsonHandler();

            // Mongolab.com credentials - Username: TeamXenon , Passsword: xenon123
            //// These tasks are done. You can physically delete the data from your SqlServerDatabase and the extracted excell files and run this method again.
            //ProblemOne(mongoHandler, mongoToSql, zipExtractor, excellHandler);

            //ProblemTwo(pdfHandler);

            //// NOTE!!! - you need to go to CarParts.Data.MySql project and in its App.config file 
            //// you should change the password with which you connect to your localhost instance of the MySQL Workbench server.
            //ProblemFour(sqlHandler, mySqlHandler, jsonHandler);

            //// This will throw because coutries are already added to the cloud and have unique constraint to their name in sql server.
            //// You can delete physically the data and run the method if you like.
            //ProblemFive(mongoHandler, xmlToSql);
            
            //var partNames = new List<string>();
            //using (var db = new CarPartsDbContext())
            //{
            //    partNames = db.Parts.Select(p => p.Name).ToList();
            //}

            //new SqliteHandler().Seed(partNames);

            
        }

        private static void ProblemFour(SqlServerHandler sqlHandler, MySqlHandler mySqlHandler, JsonHandler jsonHandler)
        {
            var reports = sqlHandler.ReadPartReports();

            jsonHandler.GenerateJsonReports(reports);
            mySqlHandler.WriteReports(reports);
        }

        private static void ProblemTwo(PdfHandler pdfHandler)
        {
            pdfHandler.GenerateSalesInfoPdf();
        }

        private static void ProblemFive(MongoDbHandler mongoHandler, XmlToSqlServerLoader xmlToSql)
        {
            var xmlHandler = new XmlHandler();
            var countries = xmlHandler.GetCountries();

            xmlToSql.LoadCountries(countries);
            mongoHandler.WriteCollection<XmlCountry>("Countries", countries);
            Console.WriteLine("successfully write down countries to MongoDB and SqlServer databases");
        }

        private static void ProblemOne(MongoDbHandler mongoHandler, MongoToSqlServerLoader mongoToSql, ZipExtractor extractor, ExcellHandler excellHandler)
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

            mongoToSql.Load(mongoHandler);
            Console.WriteLine("successsfully added all in a sql database");

            extractor.Extract();
            Console.WriteLine("Files extracted successfully.");

            excellHandler.MigrateFromExcellToSqlServer();
            Console.WriteLine("successfully added sales table from excell files.");
        }
    }
}
