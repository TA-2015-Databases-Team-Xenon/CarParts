namespace CarParts.Models.MongoDbModels
{
    using System;
    using System.Collections.Generic;

    public class Manufacturer
    {
        private ICollection<int> partsIds;

        public Manufacturer(int id, string name)
        {
            this.Id = id;
            this.Name = name;

            this.partsIds = new HashSet<int>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<int> PartsIds
        {
            get { return this.partsIds; }
            set { this.partsIds = value; }
        }
    }
}
