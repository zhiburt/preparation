using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using preparation.Models;
using preparation.Services.Cart;
using preparation.Services.Streinger;

namespace preparation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AjaxController : ControllerBase
    {
        private readonly IStreinger _streinger;

        public AjaxController(IStreinger streinger)
        {
            this._streinger = streinger;
        }

        [HttpGet]
        [Route("getGoods")]
        public async Task<IEnumerable<Good>> SearchGoods(string search)
        {
            if (search == "")
            {
                return null;
            }

            return await _streinger.Goods(search);
        }

        [HttpPut]
        [Route("addProduct")]
        public async Task<bool> AddProduct(string productName, string supplier, string addressSupplier)
        {
            Cart cart = new Cart(HttpContext);
            var prod = (await _streinger.Goods()).First(g => g.Product.Name == productName &&
                                                             g.Supplier.Name == supplier &&
                                                             g.Supplier.Address == addressSupplier);
            cart.AddProduct( prod );
            return true;
        }

        [HttpDelete]
        [Route("addProduct")]
        public async Task<bool> DeleteProduct(string productName, string supplier, string addressSupplier)
        {

            Cart cart = new Cart(HttpContext);
            var prod = (await _streinger.Goods()).First(g => g.Product.Name == productName &&
                                                             g.Supplier.Name == supplier &&
                                                             g.Supplier.Address == addressSupplier);
            cart.Remove(prod);
            return true;
        }
    }
}