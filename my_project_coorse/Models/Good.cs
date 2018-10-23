using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.Models
{
    public class Good : IProduct
    {
        public decimal Price { get; set; }
        public ISupplier Supplier { get; set; }
        public IGood Product { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            var good = obj as Good;
            return good != null &&
                   Price == good.Price &&
                   EqualityComparer<ISupplier>.Default.Equals(Supplier, good.Supplier) &&
                   EqualityComparer<IGood>.Default.Equals(Product, good.Product);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Price, Supplier, Product);
        }
    }
}
