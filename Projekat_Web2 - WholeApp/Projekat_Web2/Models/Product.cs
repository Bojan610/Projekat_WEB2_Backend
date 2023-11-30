using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Ingredients { get; set; }
        public List<Consumer> Consumers { get; set; }   //zbog baze

        public List<Order> Orders { get; set; }   //zbog baze
    }
}
