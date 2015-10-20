namespace CarParts.Data.SqlServer
{
    using System.Collections.Generic;
    using System.Linq;

    using Data.MySql;
    using Models;

    public class SqlServerHandler
    {
        public IEnumerable<PartReportInputModel> ReadPartReports()
        {
            var reports = new List<PartReportInputModel>();

            using (var db = new CarPartsDbContext())
            {
                reports = db.Sales.GroupBy(s => s.PartId)
                    .Select(gr => gr.FirstOrDefault())
                    .Select(s => new PartReportInputModel
                {
                    PartId = s.PartId,
                    PartName = s.Part.Name,
                    Price = s.UnitPrice,
                    Vendor = s.Vendor.Name,
                    Quantity = s.Quantity,
                    TotalPrice = s.Quantity * s.UnitPrice
                })
                .ToList();
            }

            return reports;
        }

        public IEnumerable<string> ReadPartNames()
        {
            var names = new List<string>();

            using (var db = new CarPartsDbContext())
            {
                names = db.Parts
                    .Select(p => p.Name)
                    .ToList();
            }

            return names;
        }        
    }
}
