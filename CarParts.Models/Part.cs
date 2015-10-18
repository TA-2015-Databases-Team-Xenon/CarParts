namespace CarParts.Models
{
    using System;
    using System.Collections.Generic;

    public class Part
    {
        private ICollection<PartCategory> categories;
        private ICollection<Vendor> vendors;

        public Part()
        {
            this.categories = new HashSet<PartCategory>();
            this.vendors = new HashSet<Vendor>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int ManufacturerId { get; set; }
        
        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<PartCategory> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public virtual ICollection<Vendor> Vendors
        {
            get { return this.vendors; }
            set { this.vendors = value; }
        }
    }
}
