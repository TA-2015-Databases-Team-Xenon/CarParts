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
        public void Load(MongoDbHandler mongoHandler)
        {
            this.MigrateManufacturers(mongoHandler);
            this.MigrateVendors(mongoHandler);
            this.MigrateParts(mongoHandler);
        }

        private void MigrateManufacturers(MongoDbHandler mongoHandler)
        {
            var manufacturers = mongoHandler.ReadCollection("Manufacturers").Result;

            using (var db = new CarPartsDbContext())
            {
                foreach (var manufacturer in manufacturers)
                {
                    var sqlManufacturer = new Manufacturer();
                    sqlManufacturer.Id = manufacturer["_id"].AsInt32;
                    sqlManufacturer.Name = manufacturer["Name"].AsString;
                    db.Manufacturers.Add(sqlManufacturer);
                }

                db.SaveChanges();
            }
        }

        private void MigrateVendors(MongoDbHandler mongoHandler)
        {
            var vendors = mongoHandler.ReadCollection("Vendors").Result;

            using (var db = new CarPartsDbContext())
            {
                foreach (var vendor in vendors)
                {
                    var sqlVendor = new Vendor();
                    sqlVendor.Id = vendor["_id"].AsInt32;
                    sqlVendor.Name = vendor["Name"].AsString;
                    db.Vendors.Add(sqlVendor);
                }

                db.SaveChanges();
            }
        }

        private void MigrateParts(MongoDbHandler mongoHandler)
        {
            var parts = mongoHandler.ReadCollection("Parts").Result;

            using (var db = new CarPartsDbContext())
            {
                foreach (var part in parts)
                {
                    var sqlPart = new Part();
                    sqlPart.Id = part["_id"].AsInt32;
                    sqlPart.Name = part["Name"].AsString;
                    sqlPart.Price = decimal.Parse(part["Price"].AsString);
                    sqlPart.ManufacturerId = part["ManufacturerId"].AsInt32;
                    sqlPart.Manufacturer = db.Manufacturers.Where(m => m.Id == sqlPart.ManufacturerId)
                                                                       .FirstOrDefault();                                       

                    foreach (var categoryId in part["Categories"] as BsonArray)
                    {
                        int category = categoryId.AsInt32;
                        sqlPart.Categories.Add((PartCategory)category);
                    }

                    var vendorIds = new List<int>();
                    foreach (var vendorId in part["VendorIds"] as BsonArray)
                    {
                        vendorIds.Add(vendorId.AsInt32);
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
