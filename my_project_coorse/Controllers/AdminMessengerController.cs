using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using preparation.Models.Account;
using preparation.Models.Contexts;
using preparation.Models.DbEntity;
using preparation.Services.Messenger;
using preparation.Services.Streinger;
using preparation.ViewModels.AdminMessenger;

namespace preparation.Controllers
{
    [Authorize]
    public class AdminMessengerController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMessenger _messenger;

        public AdminMessengerController(UserManager<User> userManager, IMessenger messenger)
        {
            this._userManager = userManager;
            this._messenger = messenger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SupplierForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SupplierForm(NewSupplierRequest model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var admins = await _userManager.GetUsersInRoleAsync("admin");
                foreach (var admin in admins)
                {
                    await _messenger.Send(new { Data = model, Type = "SupplierForm" }, admin, await _userManager.GetUserAsync(User));
                }

                return RedirectToAction("Index", "Search");
            }

            return View(model);
        }
    }
}