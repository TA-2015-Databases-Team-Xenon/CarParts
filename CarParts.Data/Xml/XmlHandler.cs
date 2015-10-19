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
        
               

        public IEnumerable<XmlCountry> GetCountries()
        {
            var countries = new List<XmlCountry>();

            XmlDocument doc = new XmlDocument();
            doc.Load(this.fileToLoadPath);

            var countriesNodeList = doc.SelectNodes("/Countries/Country");

            foreach (XmlNode countryNode in countriesNodeList)
            {
                XmlCountry country = new XmlCountry();

                country.Id = int.Parse(countryNode["CountryId"].InnerText);

                country.Name = countryNode["CountryName"].InnerText;

                var vendorIds = countryNode.SelectNodes("Vendors/VendorId");
                foreach (XmlNode vendorId in vendorIds)
                {
                    country.VendorsIds.Add(int.Parse(vendorId.InnerText));
                }

                var manufacturersIds = countryNode.SelectNodes("Manufacturers/ManufacturerId");
                foreach (XmlNode manufacturerId in manufacturersIds)
                {
                    country.ManufacturersIds.Add(int.Parse(manufacturerId.InnerText));
                }

                countries.Add(country);
            }

            return countries;
        }
    }
}
