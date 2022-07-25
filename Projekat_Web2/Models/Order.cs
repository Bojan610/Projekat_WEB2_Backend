using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Models
{
    public class Order
    {
        public int Id { get; set; }

        public List<Product> Products { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        //public Consumer ConsumerCurrent { get; set; }  //zbog baze
        //public Consumer ConsumerPrevious { get; set; }  //zbog baze
        //public string ConsumerPreviousID { get; set; }  //zbog baze
        //public Deliverer DelivererCurrent { get; set; }  //zbog baze
        //public Deliverer DelivererPrevious { get; set; }  //zbog baze
        //public string DelivererPreviousID { get; set; }  //zbog baze
    }
}
