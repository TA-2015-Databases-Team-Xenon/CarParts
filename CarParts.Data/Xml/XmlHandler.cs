namespace CarParts.Data.Xml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Data.SqlServer;
    using Models;

    public class XmlHandler
    {
        private const string DefaultXmlToLoadFilePath = "..\\..\\..\\DataSources\\countries.xml";
        private const string DefaultReportsDestinationPath = "Some path";

        private string fileToLoadPath;
        private string reportsDestinationPath;
        private XmlSerializer serializer;

        public XmlHandler()
            : this(DefaultXmlToLoadFilePath, DefaultReportsDestinationPath)
        {
        }

        public XmlHandler(string fileToLoadPath, string reportsDestinationPath)
        {
            this.fileToLoadPath = fileToLoadPath;
            this.reportsDestinationPath = reportsDestinationPath;
        }

        // TODO: Method for generating reports and saving them to the file system;

        // TODO: Method for reading additional data from xml file and populating it the Mongo and SQLServer databases(Tsvetan is on this now);
        public IEnumerable<Country> GetCountries()
        {
            var countries = new List<Country>();

            XmlDocument doc = new XmlDocument();
            doc.Load(this.fileToLoadPath);

            var countriesNodeList = doc.SelectNodes("/Countries/Country");

            foreach (XmlNode countryNode in countriesNodeList)
            {
                Country country = new Country();

                country.Id = int.Parse(countryNode["CountryId"].InnerText);

                country.Name = countryNode["CountryName"].InnerText;

                using (var db = new CarPartsDbContext())
                {
                    var vendorIds = countryNode.SelectNodes("Vendors/VendorId");
                    foreach (XmlNode vendorId in vendorIds)
                    {
                        var id = int.Parse(vendorId.InnerText);
                        var vendor = db.Vendors.Where(v => v.Id == id).FirstOrDefault();

                        country.Vendors.Add(vendor);
                    }

                    var manufacturersIds = countryNode.SelectNodes("Manufacturers/ManufacturerId");
                    foreach (XmlNode manufacturerId in manufacturersIds)
                    {
                        var id = int.Parse(manufacturerId.InnerText);
                        var manufacturer = db.Manufacturers.Where(m => m.Id == id).FirstOrDefault();

                        country.Manufacturers.Add(manufacturer);
                    }
                }

                countries.Add(country);
            }

            return countries;
        }
    }
}
