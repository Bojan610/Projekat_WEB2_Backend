using System.Collections.Generic;

namespace ConsumerDelivererAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string DelivererEmail { get; set; }
    }
}
