using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.DTO
{
    public class AddToCartModelDto
    {
        public int ProductId { get; set; }
        public string email { get; set; }
        public int Quantity { get; set; }
    }
}
