namespace CarParts.ConsoleClient
{
    using System;

    using Data.Excell;
    using Data.MongoDb;
    using Data.SqlServer;
    using Utils;

    public class Startup
    {
        public static void Main()
        {            
            //// Populating database in MongoLab with some useless testing data
            //try
            //{                
            //  new MongoDataSeeder().Seed();
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("Just in case you uncomment and try to populate the cloud database with existing id's and it throws :)");
            //}

            //new MongoToSqlServerLoader().Load();
            //Console.WriteLine("successsfully added all in a sql database");

            // Commented because files are already extracted in the folder.
            //new ZipExtractor().Extract();
            //Console.WriteLine("Files extracted successfully.");

            //new ExcellHandler().MigrateFromExcellToSqlServer();
            //Console.WriteLine("successfully added sales table from excell files.");

            new XmlToSqlServerLoader().Load(new Data.Xml.XmlHandler());
        }
    }
}
