﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using preparation.Models;
using preparation.Services.Cart;
using preparation.Services.Streinger;
using preparation.ViewModels.Ajax;

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

        [HttpPost]
        [Route("addProduct")]
        public async Task<bool> AddProduct([FromBody]AddProductToCartViewModel model)
        {
            if (ModelState.IsValid)
            {
                Cart cart = new Cart(HttpContext);
                var prod = (await _streinger.Goods()).First(g => g.Product.Name == model.ProductName &&
                                                                 g.Supplier.Name == model.Supplier &&
                                                                 g.Supplier.Address == model.AddressSupplier);
                cart.AddProduct(prod);
                return true;
            }

            return false;
        }

        [HttpPost]
        [Route("removeProduct")]
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