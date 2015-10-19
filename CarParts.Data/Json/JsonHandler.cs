namespace CarParts.Data.Json
{
    using System.IO;
    using CarParts.Models.ReportModels;
    using Newtonsoft.Json;

    public class JsonHandler
    {
        private const string DefaultOutputDirectory = "..\\..\\..\\DataOutput\\Json-Reports";
        private const string OutputFileFormat = "{0}\\{1}.json";
        
        public void GenerateJsonReport(PartReportInputModel productReport, string outputDirectory = DefaultOutputDirectory)
        {
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (var writer = File.CreateText(string.Format(OutputFileFormat,outputDirectory, productReport.PartId)))
            {
                writer.Write(JsonConvert.SerializeObject(new
                {
                    id = productReport.PartId,
                    name = productReport.PartName,
                    vendor = productReport.Vendor,
                    price = productReport.Price,
                    quantity = productReport.Quantity,
                    total = productReport.TotalPrice
                },
                Formatting.Indented));
            }
        }
    }
}
