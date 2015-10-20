namespace CarParts.ConsoleClient
{
    using System;
    using CarParts.Data.SqlServer;
    using Data.Excell;
    using Data.Json;
    using Data.MongoDb;
    using Data.MySql;
    using Data.Pdf;
    using Data.SQLite;
    using Data.Xml;
    using Utils;

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
            var sqliteHandler = new SqliteHandler();
            var xmlHandler = new XmlHandler();

            //// Mongolab.com credentials - Username: TeamXenon , Passsword: xenon123

            ProblemOne(mongoHandler, mongoToSql, zipExtractor, excellHandler);

            ProblemTwo(pdfHandler);

            ProblemThree(xmlHandler);

            //// NOTE!!! - you need to go to CarParts.Data.MySql project and in its App.config file 
            //// you should change the password in the connectionString tag with which you connect to your localhost instance of the MySQL Workbench server.
            ProblemFour(sqlHandler, mySqlHandler, jsonHandler);

            ProblemFive(mongoHandler, xmlToSql);

            ProblemSix(excellHandler, sqlHandler, sqliteHandler, mySqlHandler);
        }

        private static void ProblemThree(XmlHandler xmlHandler)
        {
            xmlHandler.GenerateSalesReport();
            Console.WriteLine("Successfully generated xml sales report file.");
        }

        private static void ProblemOne(MongoDbHandler mongoHandler, MongoToSqlServerLoader mongoToSql, ZipExtractor extractor, ExcellHandler excellHandler)
        {
            /*// populating database in mongolab with some useless testing data
            try
            {
                new mongodataseeder().seed(mongohandler);
            }
            catch (exception ex)
            {
                console.writeline("just in case you uncomment and try to populate the cloud database with existing id's and it throws :)");
            }*/

            mongoToSql.Load(mongoHandler);
            Console.WriteLine("successsfully added all in a sql database");

            extractor.Extract();
            Console.WriteLine("Files extracted successfully.");

            excellHandler.MigrateFromExcellToSqlServer();
            Console.WriteLine("successfully added sales table from excell files.");
        }

        private static void ProblemTwo(PdfHandler pdfHandler)
        {
            pdfHandler.GenerateSalesInfoPdf();
            Console.WriteLine("Successfully generated pdf file.");
        }

        private static void ProblemFour(SqlServerHandler sqlHandler, MySqlHandler mySqlHandler, JsonHandler jsonHandler)
        {
            var reports = sqlHandler.ReadPartReports();

            jsonHandler.GenerateJsonReports(reports);
            mySqlHandler.WriteReports(reports);
            Console.WriteLine("Successfully added json reports to the file system and to MySQL database.");
        }

        private static void ProblemFive(MongoDbHandler mongoHandler, XmlToSqlServerLoader xmlToSql)
        {
            var xmlHandler = new XmlHandler();
            var countries = xmlHandler.GetCountries();

            // If these are runned before they will throw because coutries are already added to the cloud and have unique constraint to their name in sql server.
            // You can delete physically the data and run the method if you like.
            xmlToSql.LoadCountries(countries);

            /*try // Just in case :)
            {
                mongoHandler.WriteCollection<XmlCountry>("Countries", countries);
            }
            catch (Exception ex)
            {
            }*/

            Console.WriteLine("Successfully write down countries from XML to MongoDB and SqlServer databases");
        }

        private static void ProblemSix(ExcellHandler excellHandler, SqlServerHandler sqlHandler, SqliteHandler sqliteHandler, MySqlHandler mySqlHandler)
        {
            var reports = mySqlHandler.ReadReports();
            var taxes = sqliteHandler.ReadTaxes();

            excellHandler.GenerateExcellFile(reports, taxes, "VendorsFinancialReport.xlsx");
            Console.WriteLine("Successfully generated excell file to ");
        }
    }
}
