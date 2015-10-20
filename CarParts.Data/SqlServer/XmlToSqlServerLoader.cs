namespace CarParts.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Data.Xml;
    using Models;

    public class XmlToSqlServerLoader
    {
        public void LoadCountries(IEnumerable<XmlCountry> collection)
        {
            var countries = new List<Country>();

            using (var db = new CarPartsDbContext())
            {
                foreach (var xmlCountry in collection)
                {
                    var country = new Country();
                    country.Id = xmlCountry.Id;
                    country.Name = xmlCountry.Name;

                    var vendors = db.Vendors.Where(v => xmlCountry.VendorsIds.Contains(v.Id))
                                                                  .ToList();

                    foreach (var vendor in vendors)
                    {
                        country.Vendors.Add(vendor);
                        vendor.Countries.Add(country);
                    }

                    var manufacturers = db.Manufacturers.Where(m => xmlCountry.ManufacturersIds.Contains(m.Id))
                                                                  .ToList();

                    foreach (var manufacturer in manufacturers)
                    {
                        country.Manufacturers.Add(manufacturer);
                        manufacturer.Countries.Add(country);
                    }

                    db.Countries.Add(country);
                }

                db.SaveChanges();
            }
        }
    }
}
