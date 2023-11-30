using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Models
{
    public class Consumer : User 
    {
        public List<Product> Products { get; set; }
        public Order CurrentOrder_Con { get; set; }
      

        //public List<Order> MyPreviousOrders { get; set; }
    }
}
