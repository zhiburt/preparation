using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation.Models;
using preparation.Services.Streinger;
using preparation.Services.TopAlgorithm;

namespace preparation.Controllers
{
    public class SearchController : Controller
    {
        private readonly IStreinger _streinger;
        private readonly ITopAlgorithm _topAlgorithm;

        public SearchController(IStreinger streinger, ITopAlgorithm topAlgorithm)
        {
            this._streinger = streinger;
            _topAlgorithm = topAlgorithm;
        }

        public async Task<IActionResult> Index()
        {
            var goods = await _streinger.Goods();
            var stackGoods = StackLogic(goods as IEnumerable<IProduct>);
            var tops = _topAlgorithm.Top(stackGoods);

            return View(tops); // return best goods better
        }

        //TODO better give IEnumerable<Stack<Good>>() for better View

        [HttpGet]
        public async Task<IActionResult> Search(string searchName)
        {
            var allGoods = await _streinger.Goods();

            IEnumerable<IEnumerable<IProduct>> stackProd = null;
            if(allGoods != null && !string.IsNullOrEmpty(searchName))
            {
                var products = SearchLogic(allGoods, searchName);
                if (products != null)
                {
                    var stack = StackLogic(products);
                    stackProd = SortProductsStack(stack);
                }
            }

            return View("Index", stackProd);
        }

        public IEnumerable<IEnumerable<IProduct>> StackLogic(IEnumerable<IProduct> products)
        {
            var dict = new SortedDictionary<string, Stack<IProduct>>();
            foreach (var product in products)
            {
                if (!dict.ContainsKey(product.Product.Name))
                {
                    dict.Add(product.Product.Name, new Stack<IProduct>());
                }

                dict[product.Product.Name].Push(product);
            }

            return dict.Values;
        }

        public IEnumerable<IEnumerable<IProduct>> SortProductsStack(IEnumerable<IEnumerable<IProduct>> produEnumerable)
        {
            var products = produEnumerable.Select(el => 
                el.OrderBy(ex => ex.Price)); //main sort
            
            return products.OrderBy((e) => e.First().Price);
        }

        public IEnumerable<IProduct> SearchLogic(IEnumerable<IProduct> allProducts, string searchName)
        {
            var mentalProducts = from product in allProducts
                                 where product.Product.Name.Contains(searchName)
                                 select product;

            return !mentalProducts.Any() ? null : mentalProducts;
        }
    }
}