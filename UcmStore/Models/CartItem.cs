using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UcmStore.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}