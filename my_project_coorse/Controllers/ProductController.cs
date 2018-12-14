using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation.Models;
using preparation.Services.Recommender;
using preparation.Services.Streinger;
using preparation.ViewModels.Product;

namespace preparation.Controllers
{
    [Route("[action]/[controller]")]
    public class ProductController : Controller
    {
        private readonly IStreinger _streinger;
        private readonly IRecommender _recommendator;

        public ProductController(IStreinger streinger, IRecommender recommendator)
        {
            this._streinger = streinger;
            this._recommendator = recommendator;
        }

        [HttpGet]
        public async Task<ViewResult> Product(string productName, string returlUrl = null)
        {
            if (!string.IsNullOrEmpty(productName))
            {
                var products = await _streinger.Goods(productName);
                if (products != null && products.Count() != 0)
                {
                    IProduct firstProd = products.First();
                    var prodViewModel = new ProductViewModel()
                    {
                        Comments = _recommendator.CommentsTo(firstProd),
                        Product = firstProd,
                        Products = products,
                        Recommendations = _recommendator.RecommendationsTo(firstProd)
                    };

                    return View(prodViewModel);
                }
            }

            return View();
        }


        #region private

        #endregion
    }
}
