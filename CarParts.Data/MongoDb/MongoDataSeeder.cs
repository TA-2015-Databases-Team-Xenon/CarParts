namespace CarParts.Data.MongoDb
{
    using System.Collections.Generic;

    using Models.MongoDbModels;
    using MongoDB.Bson;

    public class MongoDataSeeder
    {
        public void Seed(MongoDbHandler mongoHandler)
        {
            this.SeedCategories(mongoHandler);
            this.SeedManufacturers(mongoHandler);
            this.SeedVendors(mongoHandler);
            this.SeedParts(mongoHandler);
        }

        private void SeedCategories(MongoDbHandler mongoHandler)
        {
            var categories = new List<BsonDocument>()
            {
                new PartCategory(1, "Electrics").ToBsonDocument(),
                new PartCategory(2, "Cooling").ToBsonDocument(),
                new PartCategory(3, "Exterior").ToBsonDocument()
            };

            mongoHandler.WriteCollection("PartCategories", categories);
        }

        private void SeedManufacturers(MongoDbHandler mongoHandler)
        {
            var manufacturers = new List<BsonDocument>();

            for (int i = 1; i < 11; i++)
            {
                if (i % 3 == 0)
                {
                    manufacturers.Add(new Manufacturer(i, "Cheap Parts Co." + i).ToBsonDocument());
                }
                else if (i % 3 == 1)
                {
                    manufacturers.Add(new Manufacturer(i, "Expensive Parts Ltd." + i).ToBsonDocument());
                }
                else
                {
                    manufacturers.Add(new Manufacturer(i, "Bai Ivan's Garage" + i).ToBsonDocument());
                }
            }

            mongoHandler.WriteCollection("Manufacturers", manufacturers);
        }

        private void SeedVendors(MongoDbHandler mongoHandler)
        {
            var vendors = new List<BsonDocument>();

            for (int i = 1; i < 11; i++)
            {
                if (i % 3 == 0)
                {
                    vendors.Add(new Vendor(i, "Auto Trade" + i).ToBsonDocument());
                }
                else if (i % 3 == 1)
                {
                    vendors.Add(new Vendor(i, "Pesho's shop" + i).ToBsonDocument());
                }
                else
                {
                    vendors.Add(new Vendor(i, "Gosho Land" + i).ToBsonDocument());
                }
            }

            mongoHandler.WriteCollection("Vendors", vendors);
        }

        private void SeedParts(MongoDbHandler mongoHandler)
        {
            var parts = new List<BsonDocument>();

            for (int i = 1; i < 30; i++)
            {
                Part part = new Part(i, i + 100, (i / 3 + 1));
                if (i % 3 == 0)
                {
                    part.Name = "Cool Radiator" + i;
                    part.Categories.Add(2);
                    part.VendorIds.Add(i / 3 + 1);
                    part.VendorIds.Add(i);
                }
                else if (i % 3 == 1)
                {
                    part.Name = "Bi-Xenon Headlights" + i;
                    part.Categories.Add(1);
                    part.Categories.Add(3);
                    part.VendorIds.Add(i / 3 + 1);
                    part.VendorIds.Add(i);
                }
                else
                {
                    part.Name = "Radiator Fan" + i;
                    part.Categories.Add(1);
                    part.Categories.Add(2);
                    part.VendorIds.Add(i / 3 + 1);
                    part.VendorIds.Add(i);
                }

                parts.Add(part.ToBsonDocument());
            }

            mongoHandler.WriteCollection("Parts", parts);
        }
    }
}
