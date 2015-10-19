namespace CarParts.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Linq;

    using Data.MySql;

    public class SqlServerHandler
    {
        public IEnumerable<PartReportInputModel> ReadPartReports()
        {
            var reports = new List<PartReportInputModel>();

            using (var db = new CarPartsDbContext())
            {
                reports = db.Sales.Select(s => new PartReportInputModel
                {
                    PartId = s.PartId,
                    PartName = s.Part.Name,
                    Price = s.UnitPrice,
                    Vendor = s.Vendor.Name,
                    Quantity = s.Quantity,
                    TotalPrice = s.Quantity * s.UnitPrice
                })
                .Take(10)
                .ToList();
            }

            return reports;
        }
    }
}
