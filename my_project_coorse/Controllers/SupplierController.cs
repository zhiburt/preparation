using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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

        public SupplierController(IStreinger streinger, SuppliersContext suppliersContext,
            UserManager<User> userManager)
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
            var resp = await GetCompanies(user);

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

        [HttpPost]
        public async Task<IActionResult> AddPreparation(AddPreparationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid && 
                !(model is null)   &&
                !string.IsNullOrEmpty(model.Name) &&
                !string.IsNullOrEmpty(model.ActiveIngredient) &&
                !string.IsNullOrEmpty(model.Description) &&
                !string.IsNullOrEmpty(model.ImageURL) &&
                !string.IsNullOrEmpty(model.Type))
            {
                await _streinger.AddPreparationAsync(new Preparation
                {
                    Name = model.Name,
                    ActiveIngredient = model.ActiveIngredient,
                    Description = model.Description,
                    ImageURL = model.ImageURL,
                    Type = model.Type
                });

                return RedirectToAction("СhangeProduct");
            }

            ModelState.AddModelError("invalid", "invalid model");
            return View("AddPreparation", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddPreparation(string returnUrl = null) => View();


        [HttpPost]
        public async Task<IActionResult> СhangeProduct([FromForm] ChangeProductViewModel model,
            [FromQuery(Name = "products")] IEnumerable<IProduct> products, string returnUrl = null)
        {
            if (model == null)
            {
               return RedirectToAction("СhangeProduct");
            }

            var user = await _userManager.GetUserAsync(HttpContext?.User);
            model.Products = await GetProductsOfSuppliers(user);

            if (ModelState.IsValid &&
                model.Products != null &&
                model.NewPrice > 0m &&
                model.IndexChanges >= 0 && model.IndexChanges < model.Products.Count())
            {
                var changeGood = model.Products.ElementAt(model.IndexChanges);
                await _streinger.RemoveGood(changeGood as Good);

                changeGood.Price = model.NewPrice;
                await _streinger.AddGood(changeGood as Good);
            }

            return RedirectToAction("СhangeProduct");
        }


        [HttpGet]
        public async Task<IActionResult> DeleteProduct( int deleteIndex, string returnUrl = null)
        {
            var model = new DeleteProductViewModel()
            {
                IndexChanges = deleteIndex,
            };

            var user = await _userManager.GetUserAsync(HttpContext?.User);
            model.Products = await GetProductsOfSuppliers(user);

            if (ModelState.IsValid &&
                model.Products != null &&
                model.IndexChanges >= 0 && model.IndexChanges < model.Products.Count())
            {
                var changeGood = model.Products.ElementAt(model.IndexChanges);
                await _streinger.RemoveGood(changeGood as Good);
            }

            return RedirectToAction("СhangeProduct");
        }

        [HttpGet]
        public async Task<IActionResult> СhangeProduct(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userCompanies = await GetCompanies(user);

            var goods = await _streinger.Goods();
            var userGoods = goods.AsQueryable().Where(g => userCompanies.Any(s => g.Supplier.Id == s.Id));

            ViewData["userGoods"] = userGoods;
            HttpContext.Items.Add("hello_user_goods", userGoods);

            return View();
        }


        #region private

        private async Task<IList<Supplier>> GetCompanies(User user)
        {
            var supplier = await _suppliersContext.Suppliers.FirstOrDefaultAsync(s => s.User == user);

            List<Supplier> resp = new List<Supplier>();
            foreach (var id in supplier.GetCompanysID())
            {
                if (id == "") //TODO THIS IS MISTAKE 
                    continue;
                resp.Add(await _streinger.Suppliers(int.Parse(id)));
            }

            return resp;
        }

        public virtual async Task<IEnumerable<IProduct>> GetProductsOfSuppliers(User user)
        {
            var userCompanies = await GetCompanies(user);

            var goods = await _streinger.Goods();
            var userGoods = goods.AsQueryable().Where(g => userCompanies.Any(s => g.Supplier.Id == s.Id));
            return userGoods;
        }

        #endregion
    }
}