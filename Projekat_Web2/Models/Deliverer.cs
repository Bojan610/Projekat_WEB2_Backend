﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Models
{
    public class Deliverer : User
    {
        public string Verified { get; set; }
        public Order CurrentOrder_Del { get; set; }
        //public int CurrentOrderID { get; set; }     //zbog baze
        public List<Order> MyPreviousOrders { get; set; }
    }
}
