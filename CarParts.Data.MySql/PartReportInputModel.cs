namespace CarParts.Data.MySql
{
    using System;

    public class PartReportInputModel
    {
        public int Id { get; set; }

        public int PartId { get; set; }

        public string PartName { get; set; }

        public string Vendor { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }        
    }
}
