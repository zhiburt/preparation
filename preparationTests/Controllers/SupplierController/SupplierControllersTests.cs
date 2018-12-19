using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using preparation.Controllers;
using preparation.Models;
using preparation.Models.Account;
using preparation.Models.Contexts;
using preparation.Services.Streinger;
using preparation.ViewModels.Supplier;
using preparationTests.ServiceTest.TestSerivices.Authefication;
using Xunit;
using Supplier = preparation.Models.DbEntity.Supplier;

namespace preparationTests.Controllers.SupplierController
{
    public class SupplierControllersTests
    {
        public class AddProduct
        {
            [Fact]
            public async Task AddValidProduct()
            {
                //Arrange
                var mok = new Mock<IStreinger>();
                mok.Setup(e => e.AddGood(It.IsAny<Good>()))
                    .ReturnsAsync(true);

                var mokManager = FakeTestingService.MockUserManager(new List<User>());
                var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);
                IResultViewModel expected = new ResultViewModel("success", true) { };

                var suppContr = new preparation.Controllers.SupplierController(mok.Object, mokSupplierContext.Object, mokManager.Object);
                //Actual
                IActionResult success = await suppContr.AddProduct(new AddProductViewModel()
                {
                    Price = 1m,
                    SupplierId = 1,
                    ProductId = 2
                });
                //Assert
                var result = Assert.IsType<ViewResult>(success);
                var model = Assert.IsAssignableFrom<IResultViewModel>(result.ViewData.Model);
                Assert.Equal(expected, model);
            }

            [Fact]
            public async Task AddInValidProduct()
            {
                //Arrange
                var product = new AddProductViewModel()
                {
                    SupplierId = 1,
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(e => e.AddGood(It.IsAny<Good>()))
                    .ReturnsAsync(true);

                var mokManager = FakeTestingService.MockUserManager(new List<User>());
                var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);
                IResultViewModel expected = new ResultViewModel("failed", false) { };

                var suppContr = new preparation.Controllers.SupplierController(mok.Object, mokSupplierContext.Object, mokManager.Object);
                //Actual
                IActionResult success = await suppContr.AddProduct(product);
                //Assert
                var result = Assert.IsType<ViewResult>(success);
                var model = Assert.IsAssignableFrom<IResultViewModel>(result.ViewData.Model);
                Assert.Equal(expected, model);
            }

            private DbContextOptions<SuppliersContext> DammyOptions { get; } = new DbContextOptionsBuilder<SuppliersContext>().Options;
        }

        public class AddPreparation
        {
            [Fact]
            public async Task AddPreparationWhenModelValidResultRedirectToAction()
            {
                //Arrange
                var prep = new AddPreparationViewModel("1", "2", "3", "4", "5");

                var mok = new Mock<IStreinger>();
                mok.Setup(e => e.AddPreparationAsync(It.IsAny<Preparation>()))
                    .ReturnsAsync(true);
                //var mokManager = FakeTestingService.MockUserManager(new List<User>());
                //var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

                var controller = new preparation.Controllers.SupplierController(mok.Object, null, null);
                //act
                var actual = await controller.AddPreparation(prep);
                //assert
                var view = Assert.IsAssignableFrom<RedirectToActionResult>(actual);
                Assert.Equal("СhangeProduct", view.ActionName);
            }

            [Fact]
            public async Task AddPreparationWhenModelInvalidResultInvalidModelRepeatPage()
            {
                //Arrange
                var prep = new AddPreparationViewModel("", "", "", "", "");

                var mok = new Mock<IStreinger>();

                var controller = new preparation.Controllers.SupplierController(mok.Object, null, null);
                //act
                var actual = await controller.AddPreparation(prep);
                //assert
                var view = Assert.IsAssignableFrom<ViewResult>(actual);
                Assert.NotNull(view.ViewData.Model);
                Assert.IsType<AddPreparationViewModel>(view.ViewData.Model);
            }

            [Fact]
            public async Task AddPreparationWhenModelIsNullResultInvalidModelRepeatPage()
            {
                //Arrange
                var mok = new Mock<IStreinger>();

                var controller = new preparation.Controllers.SupplierController(mok.Object, null, null);
                //act
                var actual = await controller.AddPreparation(null, null);
                //assert
                var view = Assert.IsAssignableFrom<ViewResult>(actual);
                Assert.Null(view.ViewData.Model);
                Assert.True(view.ViewData.ModelState.ErrorCount > 0);
            }

            private DbContextOptions<SuppliersContext> DammyOptions { get; } = new DbContextOptionsBuilder<SuppliersContext>().Options;

        }

        public class RemoveProduct
        {
            [Fact]
            public async Task RemoveWhenDataValidResultRedirect()
            {
                bool isChange = false;
                var goods = new List<Good>()
                {
                    new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(e => e.Goods())
                    .ReturnsAsync(goods);
                mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                    .Returns(Task.FromResult(true)).Callback(() => isChange = true);


                var mokManager = FakeTestingService.MockUserManager(new List<User>());
                mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                    .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

                var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

                var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
                suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                    .ReturnsAsync(goods);
                suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                    .Returns(new RedirectToActionResult("", "", null));

                var contr = suppContr.Object;

                var result = await contr.DeleteProduct(0);

                Assert.NotNull(result);
                var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
                Assert.True(isChange);
            }

            [Fact]
            public async Task RemoveWhenIndexMoreThenNeedResultNotChange()
            {
                bool isChange = false;
                var goods = new List<Good>()
                {
                    new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(e => e.Goods())
                    .ReturnsAsync(goods);
                mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                    .Returns(Task.FromResult(true)).Callback(() => isChange = true);

                var mokManager = FakeTestingService.MockUserManager(new List<User>());
                mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                    .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

                var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

                var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
                suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                    .ReturnsAsync(goods);
                suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                    .Returns(new RedirectToActionResult("", "", null));

                var contr = suppContr.Object;

                var result = await contr.DeleteProduct(10);

                Assert.NotNull(result);
                var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
                Assert.False(isChange);
            }

            [Fact]
            public async Task RemoveWhenIndexLessThenZeroResultNotChange()
            {
                bool isChange = false;
                var goods = new List<Good>()
                {
                    new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(e => e.Goods())
                    .ReturnsAsync(goods);
                mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                    .Returns(Task.FromResult(true)).Callback(() => isChange = true);

                var mokManager = FakeTestingService.MockUserManager(new List<User>());
                mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                    .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

                var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

                var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
                suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                    .ReturnsAsync(goods);
                suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                    .Returns(new RedirectToActionResult("", "", null));

                var contr = suppContr.Object;

                var result = await contr.DeleteProduct(-1);

                Assert.NotNull(result);
                var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
                Assert.False(isChange);
            }

            private DbContextOptions<SuppliersContext> DammyOptions { get; } = new DbContextOptionsBuilder<SuppliersContext>().Options;
        }
    }

    public class ChangeProduct
    {
        [Fact]
        public async Task ChangeProductWhenModelValidResultOk()
        {
            bool isChange = false;

            var model = new ChangeProductViewModel()
            {
                NewPrice = 10
            };

            var goods = new List<Good>()
                {
                    new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
                };
            var mok = new Mock<IStreinger>();
            mok.Setup(e => e.Goods())
                .ReturnsAsync(goods);
            mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                .Returns(Task.FromResult(true)).Callback(() => isChange = true);

            var mokManager = FakeTestingService.MockUserManager(new List<User>());
            mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

            var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

            var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
            suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                .ReturnsAsync(goods);
            suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                .Returns(new RedirectToActionResult("", "", null));

            var contr = suppContr.Object;

            var result = await contr.СhangeProduct(model, null);

            Assert.NotNull(result);
            var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.True(isChange);
        }

        [Fact]
        public async Task ChangeProductWhenPriceLessZeroResultNotChanged()
        {
            bool isChange = false;

            var model = new ChangeProductViewModel()
            {
                NewPrice = -1m
            };

            var goods = new List<Good>()
            {
                new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
            };
            var mok = new Mock<IStreinger>();
            mok.Setup(e => e.Goods())
                .ReturnsAsync(goods);
            mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                .Returns(Task.FromResult(true)).Callback(() => isChange = true);

            var mokManager = FakeTestingService.MockUserManager(new List<User>());
            mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

            var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

            var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
            suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                .ReturnsAsync(goods);
            suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                .Returns(new RedirectToActionResult("", "", null));

            var contr = suppContr.Object;

            var result = await contr.СhangeProduct(model, null);

            Assert.NotNull(result);
            var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.False(isChange);
        }

        [Fact]
        public async Task ChangeProductWhenPriceIsZeroResultNotChanged()
        {
            bool isChange = false;

            var model = new ChangeProductViewModel()
            {
                NewPrice = 0m
            };

            var goods = new List<Good>()
            {
                new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
            };
            var mok = new Mock<IStreinger>();
            mok.Setup(e => e.Goods())
                .ReturnsAsync(goods);
            mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                .Returns(Task.FromResult(true)).Callback(() => isChange = true);

            var mokManager = FakeTestingService.MockUserManager(new List<User>());
            mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

            var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

            var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
            suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                .ReturnsAsync(goods);
            suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                .Returns(new RedirectToActionResult("", "", null));

            var contr = suppContr.Object;

            var result = await contr.СhangeProduct(model, null);

            Assert.NotNull(result);
            var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.False(isChange);
        }

        [Fact]
        public async Task ChangeProductWhenModelNullResultNotChanged()
        {
            bool isChange = false;

            var goods = new List<Good>()
            {
                new Good(){Supplier = new preparation.Models.Supplier(){Id=1, Name="hello"}}
            };
            var mok = new Mock<IStreinger>();
            mok.Setup(e => e.Goods())
                .ReturnsAsync(goods);
            mok.Setup(ex => ex.RemoveGood(It.IsAny<Good>()))
                .Returns(Task.FromResult(true)).Callback(() => isChange = true);

            var mokManager = FakeTestingService.MockUserManager(new List<User>());
            mokManager.Setup(ex => ex.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User() { UserName = "zhiburt_maxim" });

            var mokSupplierContext = new Mock<SuppliersContext>(DammyOptions);

            var suppContr = new Mock<preparation.Controllers.SupplierController>(mok.Object, mokSupplierContext.Object, mokManager.Object);
            suppContr.Setup(ex => ex.GetProductsOfSuppliers(It.IsAny<User>()))
                .ReturnsAsync(goods);
            suppContr.Setup(ex => ex.RedirectToAction(It.IsAny<string>()))
                .Returns(new RedirectToActionResult("", "", null));

            var contr = suppContr.Object;

            var result = await contr.СhangeProduct(null, null);

            Assert.NotNull(result);
            var view = Assert.IsAssignableFrom<RedirectToActionResult>(result);
            Assert.False(isChange);
        }

        private DbContextOptions<SuppliersContext> DammyOptions { get; } = new DbContextOptionsBuilder<SuppliersContext>().Options;

    }
}
