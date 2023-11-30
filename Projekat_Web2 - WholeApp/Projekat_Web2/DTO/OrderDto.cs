using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }

        public List<Product> Products { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    }
}
