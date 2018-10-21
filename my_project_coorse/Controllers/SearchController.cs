using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation.Services.Streinger;

namespace preparation.Controllers
{
    public class SearchController : Controller
    {
        private readonly IStreinger _streinger;

        public SearchController(IStreinger streinger)
        {
            this._streinger = streinger;
        }

        public async Task<IActionResult> Index()
        {
            return View(); // return best goods better
        }

        //TODO better give IEnumerable<Stack<Good>>() for better View
         
        [HttpGet]
        public async Task<IActionResult> Search(string searchName)
        {
            var goods = await _streinger.Goods(searchName);
            return View("Index", goods);
        }
    }
}