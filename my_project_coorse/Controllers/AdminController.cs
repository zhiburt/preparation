using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using preparation.Models;
using preparation.Models.Account;
using preparation.Models.Contexts;
using preparation.Models.DbEntity;
using preparation.Services.Streinger;
using preparation.ViewModels.Admin;
using preparation.ViewModels.AdminMessenger;
using Supplier = preparation.Models.Supplier;

namespace preparation.Controllers
{
    [Authorize("admin")]
    public class AdminController : Controller
    {
        private readonly IStreinger _streinger;
        private readonly UserManager<User> _userManager;
        private readonly MessengerContext _messengerContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SuppliersContext _suppliersContext;

        public AdminController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SuppliersContext suppliersContext,
            MessengerContext messengerContext,
            IStreinger streinger)
        {
            _streinger = streinger;
            _userManager = userManager;
            _messengerContext = messengerContext;
            _roleManager = roleManager;
            _suppliersContext = suppliersContext;
        }

        public async Task<IActionResult> Index()
        {
            var adminViewModel = new AdminTableViewModel()
            {
                AmauntGoods = (await _streinger.Goods())?.Count() ?? 0,
                AmauntSuppliers = (await _streinger.Suppliers())?.Count() ?? 0,
                AmauntUsers = _userManager.Users.Count()
            };

            return View(adminViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Messanges()
        {
            var user = await _userManager.GetUserAsync(User);
            var mID = user.GetMessagesID();
            var messages = new List<Message>();
            foreach (var id in mID)
            {
                messages.Add(await _messengerContext.Messages.FirstOrDefaultAsync(m => m.Id == id));
            }

            return View(messages.AsQueryable());
        }

        [HttpGet]
        public async Task<IActionResult> RemoveMessage([FromQuery]string messageId, string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var mID = user.GetMessagesID();

            var message = await _messengerContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            message.Level--;
            await _messengerContext.SaveChangesAsync();
            return RedirectStandart(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOfSupplier([FromForm]NewSupplierRequest sp, [FromQuery]string messageId = "", [FromQuery]string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(sp.UserName);

                var success = await _streinger.AddSupplier(new Supplier()
                {
                    Name = sp.Name,
                    Company = sp.Company,
                    Address = sp.Address,
                    Geolocation = sp.Geolocation,
                    Description = sp.Description
                });
                if (success)
                {
                    var supp = await _streinger.Suppliers(sp.Name, sp.Address);
                    await AddSupplierToContext(user, supp.Id.ToString());

                    await _userManager.AddToRoleAsync(user, "supplier");
                    await _userManager.UpdateAsync(user);
                        
                    return await RemoveMessage(messageId);
                }
            }

            return RedirectStandart("");
        }

        private async Task AddSupplierToContext(User user, string companyId)
        {
            var res = await _suppliersContext.Suppliers.FirstOrDefaultAsync(supp => Equals(supp.User, user));
            if (res != null)
            {
                res.AddCompanysID(companyId);
            }
            else
            {
                var supp = new Models.DbEntity.Supplier()
                {
                    User = user
                };
                supp.AddCompanysID(companyId);

                await _suppliersContext.AddAsync(supp);
            }

            await _suppliersContext.SaveChangesAsync();
        }

        private IActionResult RedirectStandart(string modelReturnUrl)
        {
            // проверяем, принадлежит ли URL приложению
            if (!string.IsNullOrEmpty(modelReturnUrl) && Url.IsLocalUrl(modelReturnUrl))
            {
                return Redirect(modelReturnUrl);
            }
            else
            {
                return RedirectToAction("Messanges");
            }
        }

    }
}