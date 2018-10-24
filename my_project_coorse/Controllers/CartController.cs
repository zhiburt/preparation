using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation.Models;
using preparation.Services.Cart;
using preparation.ViewModels.Cart;

namespace preparation.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart _cartService;

        public CartController(ICart cartService)
        {
            _cartService = cartService;
        }

        //TODO: return some ViewModel
        //TODO: make json send from modal in search
        public IActionResult Index()
        {
            (_cartService).Context = HttpContext;

            var products =  _cartService.All();
            IProduct[] enumerable = null;
            if (products.Count() != 0)
                enumerable = products as IProduct[] ?? products.ToArray();

            var model = new CartViewModel()
            {
                Products = enumerable,
                TotalPrice = enumerable?.Sum(p => p.Price) ?? 0m,
                PromoCode = null
            };

            return View(model);
        }
    }
}