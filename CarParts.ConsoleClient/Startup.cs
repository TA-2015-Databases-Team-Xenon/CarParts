namespace CarParts.ConsoleClient
{
    using System;

    using Data.MongoDb;
    using Data.SqlServer;
    using Reports;

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

            //new ZipExtractor().Extract();
            //Console.WriteLine("Files extracted successfully.");
        }
    }
}
