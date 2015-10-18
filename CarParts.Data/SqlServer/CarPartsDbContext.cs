namespace CarParts.Data.SqlServer
{
    using System.Data.Entity;

    using Models;

    public class CarPartsDbContext : DbContext
    {
        public CarPartsDbContext()
            : base("CarPartsDb")
        {
        }

        public virtual IDbSet<Part> Parts { get; set; }

        public virtual IDbSet<Vendor> Vendors { get; set; }

        public virtual IDbSet<Manufacturer> Manufacturers { get; set; }
    }
}
