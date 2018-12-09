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
using preparation.Services.TopAlgorithm;
using Xunit;
using Xunit.Sdk;

namespace preparationTests.Controllers.SearchController
{
    public class SearchControllerTests
    {
        public class Search
        {
            [Fact]
            public async Task SearchWhenProductExists()
            {
                //Arrange
                var goods = new[]
                {
                    new Good() { Price = 2.12m, Product = new Preparation(){Name = "_world_"}},
                    new Good() { Price = 12.2m, Product = new Preparation(){Name = "hello_world"}},
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods())
                    .ReturnsAsync(goods);
                var seachController = new preparation.Controllers.SearchController(mok.Object, null);

                IEnumerable<IEnumerable<IProduct>> expected = new[]
                {
                    new[]{goods[0]},
                    new[]{goods[1]},
                };

                //Actual
                var resp = await seachController.Search("_world");
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                IEnumerable<IEnumerable<IProduct>> model =
                    Assert.IsAssignableFrom<IEnumerable<IEnumerable<IProduct>>>(viewResult.ViewData.Model);
                Assert.Equal(expected, model);
            }

            [Fact]
            public async Task SearchWhenProductDoesntExist()
            {
                //Arrange
                var goods = new[]
                {
                    new Good() { Price = 2.12m, Product = new Preparation(){Name = "_world_"}}
                };
                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods())
                    .ReturnsAsync(goods);
                var seachController = new preparation.Controllers.SearchController(mok.Object, null);

                //Actual
                var resp = await seachController.Search("_not_found");
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                var model = viewResult.ViewData.Model;

                Assert.Null(model);
            }


            [Fact]
            public async Task SearchWhenInputIsEmptyStr()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)>()))
                    .Returns(Task.FromResult(""));
                var streinger = new Streinger(mok.Object);

                var seachController = new preparation.Controllers.SearchController(streinger, null);
                string preparationName = "";
                //Actual
                var resp = await seachController.Search(preparationName);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                Assert.Null(viewResult.ViewData.Model);
            }
        }


        public class Index
        {
            [Fact]
            public async Task GetTopWhenDataIsnotEmpty()
            {
                //Arrange
                var goods = new[]
                {
                new Good() { Price = 2.12m, Product = new Preparation(){Name = "_world_"}},
                new Good() { Price = 12.2m, Product = new Preparation(){Name = "hello_world"}},
            };
                IEnumerable<IEnumerable<IProduct>> expected = new[]
                {
                new[]{goods[0]},
                new[]{goods[1]},
            };

                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods())
                    .ReturnsAsync(goods);
                var algorighm = new Mock<TopAlgorithm>();
                algorighm.Setup(a => a.Top(It.IsAny<IEnumerable<IEnumerable<IProduct>>>()))
                    .Returns(expected);

                var seachController = new preparation.Controllers.SearchController(mok.Object, algorighm.Object);

                //Actual
                var resp = await seachController.Index();
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                IEnumerable<IEnumerable<IProduct>> model =
                    Assert.IsAssignableFrom<IEnumerable<IEnumerable<IProduct>>>(viewResult.ViewData.Model);
                Assert.Equal(expected, model);
            }

            [Fact]
            public async Task GetTopWhenDataIsEmpty()
            {
                //Arrange
                IEnumerable<Good> goods = new Good[] { };
                var mok = new Mock<IStreinger>();
                mok.Setup(m => m.Goods())
                    .ReturnsAsync(goods);
                var algorighm = new Mock<TopAlgorithm>();
                algorighm.Setup(a => a.Top(It.IsAny<IEnumerable<IEnumerable<IProduct>>>()))
                    .Returns((IEnumerable<IEnumerable<IProduct>>)null);

                var seachController = new preparation.Controllers.SearchController(mok.Object, algorighm.Object);

                //Actual
                var resp = await seachController.Index();
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                Assert.Null(viewResult.ViewData.Model);
            }
        }
    }
}
