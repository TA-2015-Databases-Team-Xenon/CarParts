namespace CarParts.ConsoleClient
{
    using Data.MongoDb;
    using Data.SqlServer;

    public class Startup
    {
        public static void Main()
        {
            // Populating database in MongoLab withsome useless testing data
            //new MongoDataSeeder().Seed();
            new MongoToSqlServerLoader().Load();
            System.Console.WriteLine("successsfully added all in a sql database");
        }
    }
}
