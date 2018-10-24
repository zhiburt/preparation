using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models;

namespace preparation.ViewModels.Cart
{
    public class CartViewModel
    {
        public decimal TotalPrice { get; set; }
        public IEnumerable<IProduct> Products { get; set; }
        public IPromoCode PromoCode { get; set; }
    }
}
