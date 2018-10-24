using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using preparation.Models;

namespace preparation.Services.Cart
{
    public interface ICart
    {
        void AddProduct(IProduct product);
        void AddProduct(IProduct[] expectedList);
        IEnumerable<IProduct> All();
        int AmountProducts();
        IEnumerable<IProduct> GetAll();
        void PopLast();
        void Remove(IProduct product);
        decimal TotalPrice();

        HttpContext Context { set; }
    }
}