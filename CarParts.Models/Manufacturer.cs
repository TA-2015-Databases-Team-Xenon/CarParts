namespace CarParts.Models
{
    using System;
    using System.Collections.Generic;

    public class Manufacturer
    {
        private ICollection<Part> parts;
        private ICollection<Country> countries;

        public Manufacturer()
        {
            this.parts = new HashSet<Part>();
            this.countries = new HashSet<Country>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Country> Countries
        {
            get { return this.countries; }
            set { this.countries = value; }
        }

        public virtual ICollection<Part> Parts
        {
            get { return this.parts; }
            set { this.parts = value; }
        }
    }
}
