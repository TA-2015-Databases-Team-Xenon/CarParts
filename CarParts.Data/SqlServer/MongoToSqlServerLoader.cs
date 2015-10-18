namespace CarParts.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Models;
    using MongoDb;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoToSqlServerLoader
    {
        private MongoDbHandler mongoHandler;

        public void Load()
        {
            this.mongoHandler = new MongoDbHandler();

            this.MigrateManufacturers();
            this.MigrateVendors();
            this.MigrateParts();
        }


        private void MigrateManufacturers()
        {
            var manufacturers = this.mongoHandler.ReadCollection("Manufacturers");

            using (var db = new CarPartsDbContext())
            {
                foreach (var manufacturer in manufacturers)
                {
                    var sqlManufacturer = new Manufacturer();
                    sqlManufacturer.Id = manufacturer["_id"].ToInt32();
                    sqlManufacturer.Name = manufacturer["Name"].ToString();
                    db.Manufacturers.Add(sqlManufacturer);
                }

                db.SaveChanges();
            }
        }

        private void MigrateVendors()
        {
            var vendors = this.mongoHandler.ReadCollection("Vendors");

            using (var db = new CarPartsDbContext())
            {
                foreach (var vendor in vendors)
                {
                    var sqlVendor = new Vendor();
                    sqlVendor.Id = vendor["_id"].ToInt32();
                    sqlVendor.Name = vendor["Name"].ToString();
                    db.Vendors.Add(sqlVendor);
                }

                db.SaveChanges();
            }
        }

        private void MigrateParts()
        {
            var parts = this.mongoHandler.ReadCollection("Parts");

            using (var db = new CarPartsDbContext())
            {
                foreach (var part in parts)
                {
                    var sqlPart = new Part();
                    sqlPart.Id = part["_id"].ToInt32();
                    sqlPart.Name = part["Name"].ToString();
                    sqlPart.Price = decimal.Parse(part["Price"].ToString());
                    sqlPart.ManufacturerId = part["ManufacturerId"].ToInt32();
                    sqlPart.Manufacturer = db.Manufacturers.Where(m => m.Id == sqlPart.ManufacturerId)
                                                                       .FirstOrDefault();                                       

                    foreach (var categoryId in part["Categories"] as BsonArray)
                    {
                        int category = categoryId.ToInt32();
                        sqlPart.Categories.Add((PartCategory)category);
                    }

                    var vendorIds = new List<int>();
                    foreach (var vendorId in part["VendorIds"] as BsonArray)
                    {
                        vendorIds.Add(vendorId.ToInt32());
                    }

                    sqlPart.Vendors = db.Vendors.Where(v => vendorIds.Contains(v.Id)).ToList();

                    sqlPart.Manufacturer.Parts.Add(sqlPart);
                    foreach (var vendor in sqlPart.Vendors)
                    {
                        vendor.Parts.Add(sqlPart);
                    }

                    db.Parts.Add(sqlPart);
                }

                db.SaveChanges();
            }
        }
    }
}
