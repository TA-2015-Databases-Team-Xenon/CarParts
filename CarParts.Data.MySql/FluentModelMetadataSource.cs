namespace CarParts.Data.MySql
{
    using System.Collections.Generic;
    using Telerik.OpenAccess.Metadata;
    using Telerik.OpenAccess.Metadata.Fluent;

    public partial class FluentModelMetadataSource : FluentMetadataSource
    {
        protected override IList<MappingConfiguration> PrepareMapping()
        {
            List<MappingConfiguration> configurations =
                new List<MappingConfiguration>();

            MappingConfiguration<PartReportInputModel> reportConfiguration = new MappingConfiguration<PartReportInputModel>();
            reportConfiguration.MapType(report => new
            {
                Id = report.Id,
                PartId = report.PartId,
                PartName = report.PartName,
                Vendor = report.Vendor,
                Price = report.Price,
                Quantity = report.Quantity,
                TotalPrice = report.TotalPrice
            }).ToTable("PartReportInputModels");
            reportConfiguration.HasProperty(r => r.Id).IsIdentity(KeyGenerator.Autoinc);

            configurations.Add(reportConfiguration);

            return configurations;
        }
    }
}
