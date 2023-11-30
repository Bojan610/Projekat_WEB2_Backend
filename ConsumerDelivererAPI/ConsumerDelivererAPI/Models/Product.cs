using System.Collections.Generic;

namespace ConsumerDelivererAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Ingredients { get; set; }
    }
}
