using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UcmStore.Models
{
    public class ClothingItem
    {
        public int id { get; set; }
        public int category { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string description { get; set; }

        public string picLocation { get; set; }
    }
}