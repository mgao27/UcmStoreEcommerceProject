using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class Clothing_Item
    {
        public int ClothingID { get; set; }
        public int CategoryID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
    }
}
