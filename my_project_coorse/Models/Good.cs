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

        public override bool Equals(object obj)
        {
            var good = obj as Good;
            return good != null &&
                   Price == good.Price &&
                   EqualityComparer<Supplier>.Default.Equals(Supplier, good.Supplier) &&
                   EqualityComparer<Preparation>.Default.Equals(Product, good.Product);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Price, Supplier, Product);
        }
    }
}
