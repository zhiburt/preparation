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

namespace preparationTests.Controllers.CartController
{
    public class CartControllerTests
    {
        public class Index
        {
            [Fact]
            public async Task WhenCartIsClear()
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
            public async Task WhenCartIsNotClear()
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
}
