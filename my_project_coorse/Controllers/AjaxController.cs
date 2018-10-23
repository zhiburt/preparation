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
        public async Task<bool> AddProduct(int productId)
        {
            if (productId <= 0) throw new ArgumentOutOfRangeException(nameof(productId));

            Cart cart = new Cart(HttpContext);
            var prod = (await _streinger.Goods()).First(g => g.Id == productId);
            cart.AddProduct( prod );
            return true;
        }

        [HttpDelete]
        [Route("addProduct")]
        public async Task<bool> DeleteProduct(int productId)
        {
            if (productId <= 0) throw new ArgumentOutOfRangeException(nameof(productId));

            Cart cart = new Cart(HttpContext);
            var prod = (await _streinger.Goods()).First(g => g.Id == productId);
            cart.AddProduct(prod);
            return true;
        }
    }
}