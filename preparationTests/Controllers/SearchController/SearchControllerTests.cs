using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using preparation;
using preparation.Controllers;
using preparation.Models;
using preparation.Services.ExternalDB;
using preparation.Services.Streinger;
using Xunit;
using Xunit.Sdk;

namespace preparationTests.Controllers.SearchController
{
    public class SearchControllerTests
    {
        public class Get
        {
            [Fact]
            public async Task SearchWhenServiceResponceValidData()
            {
                //Arrange
                var goods = new[]
                {
                    new Good() { Price = 12.2m, Product = new Preparation(){Name = "hello_world"}},
                    new Good() { Price = 2.12m, Product = new Preparation(){Name = "_world_"}},
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods("_find_"))
                    .ReturnsAsync(goods);

                var seachController = new preparation.Controllers.SearchController(mok.Object);
                var expected = goods;
                //Actual
                var resp = await seachController.Search("_find_");
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                IEnumerable<Good> model =
                    Assert.IsAssignableFrom<IEnumerable<Good>>(viewResult.ViewData.Model);
                Assert.Equal(expected, model);
            }

            [Fact]
            public async Task SearchWhenServiceResponceInValidData()
            {
                //Arrange
                var goods = new[]
                {
                    new Good() { Price = 12.2m, Product = new Preparation(){Name = "hello_world"}},
                    new Good() { Price = 2.12m, Product = new Preparation(){Name = "_world_"}},
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods("_find_"))
                    .ReturnsAsync(goods);

                var seachController = new preparation.Controllers.SearchController(mok.Object);
                var expected = goods;
                //Actual
                var resp = await seachController.Search("_find_");
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                IEnumerable<Good> model =
                    Assert.IsAssignableFrom<IEnumerable<Good>>(viewResult.ViewData.Model);
                Assert.Equal(expected, model);
            }

            [Fact]
            public async Task SearchWhenServiceResponceInvalidData()
            {
                //Arrange
                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods("_find_invalid_"))
                    .ReturnsAsync((IEnumerable<Good>)null);

                var seachController = new preparation.Controllers.SearchController(mok.Object);
                //Actual
                var resp = await seachController.Search("_find_invalid_");
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                //IEnumerable<Good> model =
                //    Assert.IsAssignableFrom<IEnumerable<Good>>(viewResult.ViewData.Model);
                Assert.Null(viewResult.ViewData.Model);
            }

            [Fact]
            public async Task PreparationsWhenSearchIsEmptyStr()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)>()))
                    .Returns(Task.FromResult(""));
                var streinger = new Streinger(mok.Object);

                var seachController = new preparation.Controllers.SearchController(streinger);
                string preparationName = "";
                //Actual
                var resp = await seachController.Search(preparationName);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                Assert.Null(viewResult.ViewData.Model);
            }
        }
    }
}
