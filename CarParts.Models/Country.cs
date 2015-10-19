namespace CarParts.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        private ICollection<Vendor> vendors;
        private ICollection<Manufacturer> manufacturers;

        public Country()
        {
            this.vendors = new HashSet<Vendor>();
            this.manufacturers = new HashSet<Manufacturer>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Vendor> Vendors
        {
            get { return this.vendors; }
            set { this.vendors = value; }
        }

        public virtual ICollection<Manufacturer> Manufacturers
        {
            get { return this.manufacturers; }
            set { this.manufacturers = value; }
        }
    }
}
