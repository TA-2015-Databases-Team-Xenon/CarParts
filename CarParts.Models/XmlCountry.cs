namespace CarParts.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class XmlCountry
    {
        private ICollection<int> vendorsIds;
        private ICollection<int> manufacturersIds;

        public XmlCountry()
        {
            this.vendorsIds = new HashSet<int>();
            this.manufacturersIds = new HashSet<int>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<int> VendorsIds
        {
            get { return this.vendorsIds; }
            set { this.vendorsIds = value; }
        }

        public ICollection<int> ManufacturersIds
        {
            get { return this.manufacturersIds; }
            set { this.manufacturersIds = value; }
        }
    }
}
