namespace CarParts.Reports
{
    using Ionic.Zip;

    public class ZipExtractor
    {
        private const string DefaultSalesZipArchivePath = "..\\..\\..\\DataSources\\Sales.zip";
        private const string DefaultSalesExtractionPath = "..\\..\\..\\DataSources\\SalesReports";

        private string zipPath;
        private string extractPath;

        public ZipExtractor()
            : this(DefaultSalesZipArchivePath, DefaultSalesExtractionPath)
        {
        }

        public ZipExtractor(string zipPath, string extractPath)
        {
            this.zipPath = zipPath;
            this.extractPath = extractPath;
        }

        public void Extract()
        {
            using (ZipFile archive = ZipFile.Read(this.zipPath))
            {
                foreach (ZipEntry entry in archive)
                {
                    entry.Extract(this.extractPath, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}
