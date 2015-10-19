namespace CarParts.Data.SqlServer
{
    using System.Data.Entity;

    using Data.Migrations;
    using Models;

    public class CarPartsDbContext : DbContext
    {
        public CarPartsDbContext()
            : base("CarPartsDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CarPartsDbContext, Configuration>());
        }

        public virtual IDbSet<Part> Parts { get; set; }

        public virtual IDbSet<Vendor> Vendors { get; set; }

        public virtual IDbSet<Manufacturer> Manufacturers { get; set; }

        public virtual IDbSet<Sale> Sales { get; set; }
    }
}
