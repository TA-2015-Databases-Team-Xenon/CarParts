namespace CarParts.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Sale
    {
        public int Id { get; set; }

        public int PartId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

        public int VendorId { get; set; }

        public virtual Part Part { get; set; }

        public virtual Vendor Vendor { get; set; }

        [NotMapped]
        public decimal Sum
        {
            get
            {
                return this.UnitPrice * this.Quantity;
            }
        }
    }
}
