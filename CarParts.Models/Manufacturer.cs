namespace CarParts.Models
{
    using System;
    using System.Collections.Generic;

    public class Manufacturer
    {
        private ICollection<Part> parts;

        public Manufacturer()
        {
            this.parts = new HashSet<Part>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Part> Parts
        {
            get { return this.parts; }
            set { this.parts = value; }
        }
    }
}
