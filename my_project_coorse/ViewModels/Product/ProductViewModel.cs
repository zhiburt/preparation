using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models;

namespace preparation.ViewModels.Product
{
    public class ProductViewModel
    {
        public IProduct Product { get; set; }
        public IEnumerable<string> Comments { get; set; }
        public IEnumerable<IProduct> Recommendations { get; set; }
        public IEnumerable<IProduct> Products { get; set; }
    }
}
