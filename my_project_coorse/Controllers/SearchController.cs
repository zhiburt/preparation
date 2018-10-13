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
            return View(await _streinger.Preparations());
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchName)
        {
            var prep = await _streinger.Preparations(searchName);
            return View("Index", prep);
        }
    }
}