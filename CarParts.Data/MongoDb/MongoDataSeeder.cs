namespace CarParts.Data.MongoDb
{
    using System.Collections.Generic;

    using Models.MongoDbModels;

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
            var categories = new List<PartCategory>()
            {
                new PartCategory(1, "Electrics"),
                new PartCategory(2, "Cooling"),
                new PartCategory(3, "Exterior")
            };

            mongoHandler.WriteCollection<PartCategory>("PartCategories", categories);
        }

        private void SeedManufacturers(MongoDbHandler mongoHandler)
        {
            var manufacturers = new List<Manufacturer>();

            for (int i = 1; i < 11; i++)
            {
                if (i % 3 == 0)
                {
                    manufacturers.Add(new Manufacturer(i, "Cheap Parts Co." + i));
                }
                else if (i % 3 == 1)
                {
                    manufacturers.Add(new Manufacturer(i, "Expensive Parts Ltd." + i));
                }
                else
                {
                    manufacturers.Add(new Manufacturer(i, "Bai Ivan's Garage" + i));
                }
            }

            mongoHandler.WriteCollection<Manufacturer>("Manufacturers", manufacturers);
        }

        private void SeedVendors(MongoDbHandler mongoHandler)
        {
            var vendors = new List<Vendor>();

            for (int i = 1; i < 11; i++)
            {
                if (i % 3 == 0)
                {
                    vendors.Add(new Vendor(i, "Auto Trade" + i));
                }
                else if (i % 3 == 1)
                {
                    vendors.Add(new Vendor(i, "Pesho's shop" + i));
                }
                else
                {
                    vendors.Add(new Vendor(i, "Gosho Land" + i));
                }
            }

            mongoHandler.WriteCollection<Vendor>("Vendors", vendors);
        }

        private void SeedParts(MongoDbHandler mongoHandler)
        {
            var parts = new List<Part>();

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

                parts.Add(part);
            }

            mongoHandler.WriteCollection<Part>("Parts", parts);
        }
    }
}
