namespace CarParts.Data.Xml
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Data.SqlServer;
    using Models;

    public class XmlHandler
    {
        private const string DefaultXmlToLoadFilePath = "..\\..\\..\\DataSources\\countries.xml";
        private const string DefaultReportsDestinationPath = "..\\..\\..\\DataOutput\\";
        private const string DefaultSaleReportFileName = "salesReport.xml";

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

        public void GenerateSalesReport(string fileName = DefaultSaleReportFileName)
        {
            XmlDocument report = new XmlDocument();
            XmlDeclaration xmlDeclaration = report.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = report.CreateElement("sales");
            report.AppendChild(root);
            report.InsertBefore(xmlDeclaration, root);
            using (var db = new CarPartsDbContext())
            {
                var groupedSales = db.Sales.GroupBy(s => s.Vendor)
                                        .ToList();

                foreach (var group in groupedSales)
                {
                    XmlElement sale = report.CreateElement("sale");
                    sale.SetAttribute("vendor", group.Key.Name);
                    root.AppendChild(sale);
                    foreach (var s in group)
                    {
                        XmlElement summary = report.CreateElement("summary");
                        summary.SetAttribute("date", s.Date.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture));
                        summary.SetAttribute("total-sum", (s.UnitPrice * s.Quantity).ToString());
                        sale.AppendChild(summary);
                    }
                }
            }

            if (!Directory.Exists(this.reportsDestinationPath))
            {
                Directory.CreateDirectory(this.reportsDestinationPath);
            }

            report.Save(this.reportsDestinationPath + fileName);
        }
               

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
