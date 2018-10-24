using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation.Services.Cart;

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
            _cartService.Context = HttpContext;

            return View(_cartService.All());
        }
    }
}