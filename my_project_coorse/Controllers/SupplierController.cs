using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using preparation.Models;
using preparation.Models.Account;
using preparation.Models.Contexts;
using preparation.Services.Streinger;
using preparation.ViewModels.Supplier;

namespace preparation.Controllers
{
    [Authorize(("supplier"))]
    public class SupplierController : Controller
    {
        private readonly IStreinger _streinger;
        private readonly SuppliersContext _suppliersContext;
        private readonly UserManager<User> _userManager;

        public SupplierController(IStreinger streinger, SuppliersContext suppliersContext, UserManager<User> userManager)
        {
            _streinger = streinger;
            _suppliersContext = suppliersContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductsInformation(ResultViewModel result = null)
        {
            return View(result);
        }

        public async Task<IActionResult> AddProduct(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var supplier = await _suppliersContext.Suppliers.FirstOrDefaultAsync(s => s.User == user);

            List<Supplier> resp = new List<Supplier>();
            foreach (var id in supplier.GetCompanysID())
            {
                if (id == "") //TODO THIS IS MISTAKE 
                    continue;
                resp.Add(await _streinger.Suppliers(int.Parse(id)));
            }

            ViewData["preparations"] = await _streinger.Preparations();
            ViewData["company"] = resp;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid && (
                    model.ProductId > 0 && model.SupplierId > 0 && model.Price >= 0))
            {
                await _streinger.AddGood(new Good
                {
                    Price = model.Price,
                    Supplier = await _streinger.Suppliers(model.SupplierId),
                    Product = await _streinger.Preparations(model.ProductId)
                });

                return View("ProductsInformation", new ResultViewModel("success", true));
            }

            return View("ProductsInformation", new ResultViewModel("failed", false));
        }
    }
}