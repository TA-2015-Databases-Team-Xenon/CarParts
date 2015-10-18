namespace CarParts.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<CarParts.Data.SqlServer.CarPartsDbContext>
    {
        public Configuration()
        {
            // Change this after project is done.
            this.AutomaticMigrationDataLossAllowed = true;

            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "CarParts.Data.SqlServer.CarPartsDbContext";
        }

        protected override void Seed(CarParts.Data.SqlServer.CarPartsDbContext context)
        {            
        }
    }
}
