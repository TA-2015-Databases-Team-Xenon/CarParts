namespace CarParts.Data.SqlServer
{
    using System.Collections.Generic;

    using Data.Xml;

    public class XmlToSqlServerLoader
    {
        public void Load(XmlHandler xmlHandler)
        {
            var countries = xmlHandler.GetCountries();

            using (var db = new CarPartsDbContext())
            {
                foreach (var country in countries)
                {
                    db.Countries.Add(country);

                    var vendors = country.Vendors;

                    foreach (var vendor in vendors)
                    {
                        vendor.Countries.Add(country);
                    }

                    var manufacturers = country.Manufacturers;

                    foreach (var manufacturer in manufacturers)
                    {
                        manufacturer.Countries.Add(country);
                    }

                }

                db.SaveChanges();
            }
        }
    }
}
