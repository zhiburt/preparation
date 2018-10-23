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
        //TODO: return some ViewModel
        //TODO: make json send from modal in search
        public IActionResult Index()
        {
            Cart cart = new Cart(HttpContext);
            return View(cart.All());
        }
    }
}