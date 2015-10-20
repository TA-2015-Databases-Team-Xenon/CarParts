namespace CarParts.Models.SQLiteModels
{
    using System.ComponentModel.DataAnnotations;

    public class ProductTax
    {
        public ProductTax(int id, string productName, double tax)
        {
            this.Id = id;
            this.ProductName = productName;
            this.Tax = tax;
        }

        public int Id { get; set; }

        public string ProductName { get; set; }

        public double Tax { get; set; }
    }
}
