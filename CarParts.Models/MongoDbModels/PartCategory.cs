namespace CarParts.Models.MongoDbModels
{
    public class PartCategory
    {
        public PartCategory(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
