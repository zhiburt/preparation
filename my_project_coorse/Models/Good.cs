using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.Models
{
    public class Good
    {
        public decimal Price { get; set; }
        public Supplier Supplier { get; set; }
        public Preparation Product { get; set; }
    }
}
