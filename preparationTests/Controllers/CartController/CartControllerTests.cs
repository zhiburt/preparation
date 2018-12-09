using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using preparation.Models;
using preparation.Services.Cart;
using preparation.ViewModels.Cart;
using Xunit;
using NUnitAssert = NUnit.Framework.Assert;

namespace preparationTests.Controllers.CartController
{
    public class CartControllerTests
    {
        public class Index
        {
            [Fact]
            public async Task WhenCartIsClear_ExpectedNotNullModel()
            {
                //Arrange
                var cartServ = new Mock<ICart>();
                cartServ.SetupAllProperties();
                var cart = new preparation.Controllers.CartController(cartServ.Object);
                //Actual
                var res =  cart.Index();
                //Assert
                var actionResult = Assert.IsType<ViewResult>(res);
                var model = Assert.IsAssignableFrom<CartViewModel>(actionResult.ViewData.Model);
                Assert.Null(model.Products);
                Assert.Null(model.PromoCode);
                Assert.Equal(0, model.TotalPrice);
            }

            [Fact]
            public async Task WhenCartIsNotClear_ExpectedOKModel()
            {
                //Arrange
                IEnumerable<IProduct> products = new[]{ new Good(){Price = 1}, new Good() {Price = 2} };
                decimal exepectedPrice = 3m;

                var cartServ = new Mock<ICart>();
                cartServ.SetupAllProperties();
                cartServ.Setup((c) => c.All())
                    .Returns(products);

                var cart = new preparation.Controllers.CartController(cartServ.Object);
                //Actual
                var res = cart.Index();
                //Assert
                var actionResult = Assert.IsType<ViewResult>(res);
                var model = Assert.IsAssignableFrom<CartViewModel>(actionResult.ViewData.Model);
                Assert.Equal(products, model.Products);
                Assert.Null(model.PromoCode);
                Assert.Equal(exepectedPrice, model.TotalPrice);
            }

            //[Fact]
            //public void WhenCartIsNotEmpty()
            //{
            //    //var cart = new preparation.Controllers.CartController();
            //}
        }


    }

    [NUnit.Framework.TestFixture]
    public class CartControllerTestsNunit
    {
        [NUnit.Framework.Test]
        public async Task WhenCartIsClear_ExpectedNotNullModel()
        {
            //Arrange
            var cartServ = new Mock<ICart>();
            cartServ.SetupAllProperties();
            var cart = new preparation.Controllers.CartController(cartServ.Object);
            //Actual
            var res = cart.Index();
            //Assert
            var actionResult = Assert.IsType<ViewResult>(res);
            var model = Assert.IsAssignableFrom<CartViewModel>(actionResult.ViewData.Model);
            NUnitAssert.Null(model.Products);
            NUnitAssert.Null(model.PromoCode);
            NUnitAssert.AreEqual(0, model.TotalPrice);
        }

        [NUnit.Framework.Test]
        public async Task WhenCartIsNotClear_ExpectedOKModel()
        {
            //Arrange
            IEnumerable<IProduct> products = new[] { new Good() { Price = 1 }, new Good() { Price = 2 } };
            decimal exepectedPrice = 3m;

            var cartServ = new Mock<ICart>();
            cartServ.SetupAllProperties();
            cartServ.Setup((c) => c.All())
                .Returns(products);

            var cart = new preparation.Controllers.CartController(cartServ.Object);
            //Actual
            var res = cart.Index();
            //Assert
            var actionResult = Assert.IsType<ViewResult>(res);
            var model = Assert.IsAssignableFrom<CartViewModel>(actionResult.ViewData.Model);
            NUnitAssert.AreEqual(products, model.Products);
            NUnitAssert.Null(model.PromoCode);
            NUnitAssert.AreEqual(exepectedPrice, model.TotalPrice);
        }
    }
}
