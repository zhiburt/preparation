using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
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

    }
}
