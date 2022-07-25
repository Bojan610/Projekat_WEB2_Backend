using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Models
{
    public class ConsumerProduct
    {
        public int Id { get; set; }
        public string ConsumerID { get; set; }
        public Consumer Consumer { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
