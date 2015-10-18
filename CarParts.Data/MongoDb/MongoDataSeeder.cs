namespace CarParts.Data.MongoDb
{
    using System.Collections.Generic;

    using Models.MongoDbModels;

    public class MongoDataSeeder
    {
        private MongoDbHandler mongoHandler;

        public void Seed()
        {
            this.mongoHandler = new MongoDbHandler();

            this.SeedCategories();
            this.SeedManufacturers();
            this.SeedVendors();
            this.SeedParts();
        }

        private void SeedCategories()
        {
            var categories = new List<PartCategory>()
            {
                new PartCategory(1, "Electrics"),
                new PartCategory(2, "Cooling"),
                new PartCategory(3, "Exterior")
            };

            this.mongoHandler.WriteCollection<PartCategory>("PartCategories", categories);
        }

        private void SeedManufacturers()
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

            this.mongoHandler.WriteCollection<Manufacturer>("Manufacturers", manufacturers);
        }

        private void SeedVendors()
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

            this.mongoHandler.WriteCollection<Vendor>("Vendors", vendors);
        }

        private void SeedParts()
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

            this.mongoHandler.WriteCollection<Part>("Parts", parts);
        }
    }
}
