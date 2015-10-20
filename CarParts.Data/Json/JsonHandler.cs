namespace CarParts.Data.Json
{
    using System.Collections.Generic;
    using System.IO;

    using Data.MySql;
    using Newtonsoft.Json;

    public class JsonHandler
    {
        private const string DefaultOutputDirectory = "..\\..\\..\\DataOutput\\Json-Reports";
        private const string OutputFileFormat = "{0}\\{1}.json";
        
        public void GenerateJsonReports(IEnumerable<PartReportInputModel> productReports, string outputDirectory = DefaultOutputDirectory)
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            foreach (var report in productReports)
            {
                using (var writer = File.CreateText(string.Format(OutputFileFormat, outputDirectory, report.PartId)))
                {
                    writer.Write(JsonConvert.SerializeObject(
                        new
                        {
                            id = report.PartId,
                            name = report.PartName,
                            vendor = report.Vendor,
                            price = report.Price,
                            quantity = report.Quantity,
                            total = report.TotalPrice
                        },
                    Formatting.Indented));
                }
            }
        }
    }
}
