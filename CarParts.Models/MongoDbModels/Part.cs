namespace CarParts.Models.MongoDbModels
{
    using System;
    using System.Collections.Generic;

    public class Part
    {
        private ICollection<int> categories;
        private ICollection<int> vendorIds;

        public Part(int id, decimal price, int manufacturerId)
        {
            this.Id = id;
            this.Price = price;
            this.ManufacturerId = manufacturerId;

            this.categories = new HashSet<int>();
            this.vendorIds = new HashSet<int>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int ManufacturerId { get; set; }
        
        public virtual ICollection<int> Categories
        {
            get { return this.categories; }
            set { this.categories = value; }
        }

        public virtual ICollection<int> VendorIds
        {
            get { return this.vendorIds; }
            set { this.vendorIds = value; }
        }
    }
}
