using ConsumerDelivererAPI.Models;
using System.Collections.Generic;

namespace ConsumerDelivererAPI.Dto
{
    public class PickupOrderDto
    {
        public int Id { get; set; }

        public List<Product> Products { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }

        public string EmailDeliverer { get; set; }
    }
}
