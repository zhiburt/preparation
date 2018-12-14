using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using preparation;
using preparation.Controllers;
using preparation.Models;
using preparation.Services.ExternalDB;
using preparation.Services.Streinger;
using preparation.Services.TopAlgorithm;
using Xunit;
using Xunit.Sdk;
using NUnitAssert = NUnit.Framework.Assert;
using Assert = Xunit.Assert;

namespace preparationTests.Controllers.SearchController
{
    public class SearchControllerTests
    {
        [TestFixture]
        public class Integration
        {
            [TestFixture]

            public class Index
            {
                private IStreinger strngr;

                [SetUp]
                public void SetUp()
                {
                    var goods = new Good[]
                    {
                        new Good()
                        {
                            Product = new Preparation(){Name = "hello_world"},
                            Supplier = new Supplier(){Company = "company"}
                        }
                    }.AsEnumerable();

                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(goods));
                    strngr.Setup(ex => ex.Goods(It.IsAny<string>())).Returns(Task.FromResult(goods));
                    strngr.Setup(ex => ex.Goods("NOT_EXISTS")).Returns(Task<IEnumerable<Good>>.FromResult((IEnumerable<Good>)null));

                    this.strngr = strngr.Object;
                }

                [Test]
                public async Task WhenStreingerReturnNULLResultExeption()
                {
                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult((IEnumerable<Good>)null));

                    var algo = new TopAlgorithm();
                    var search = new preparation.Controllers.SearchController(streinger: strngr.Object, topAlgorithm: algo);

                    NUnitAssert.CatchAsync(async () => await search.Index());
                }

                [Test]
                public async Task WhenStreingerReturnOKDataResultOKModel()
                {
                    var algo = new TopAlgorithm();
                    var search = new preparation.Controllers.SearchController(streinger: strngr, topAlgorithm: algo);

                    var resp = await search.Index();

                    NUnitAssert.IsAssignableFrom(typeof(ViewResult), resp);
                    NUnitAssert.NotNull(resp);

                    var model = (resp as ViewResult).ViewData.Model;
                    NUnitAssert.NotNull(model);
                    NUnitAssert.IsAssignableFrom<IEnumerable<IProduct>[]>(model);
                }

                //TODO THIS IS BUG
                [Test]
                public async Task WhenCompanyNameOrProductNameNullResultExeption()
                {
                    var goods = new Good[]
                    {
                        new Good()
                        {
                            Product = new Preparation(){},
                            Supplier = new Supplier(){}
                        }
                    }.AsEnumerable();

                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(goods));

                    var algo = new TopAlgorithm();
                    var search = new preparation.Controllers.SearchController(streinger: strngr.Object, topAlgorithm: algo);

                    NUnitAssert.CatchAsync<ArgumentNullException>(async () => await search.Index());
                }
            }

            public class StackLogic
            {
                [Test]
                public async Task WhenParamValidResultOKAsync()
                {
                    var goods = new Good[]
                    {
                        new Good()
                        {
                            Product = new Preparation(){Name = "2"},
                            Supplier = new Supplier(){}
                        },
                        new Good()
                        {
                            Product = new Preparation(){Name = "1"},
                            Supplier = new Supplier(){}
                        }
                    }.AsEnumerable();

                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(goods));
                    var algo = new Mock<ITopAlgorithm>();
                    var search = new preparation.Controllers.SearchController(streinger: strngr.Object, topAlgorithm: algo.Object);

                    var actual = search.StackLogic(await strngr.Object.Goods() as IEnumerable<IProduct>);
                    NUnitAssert.IsNotNull(actual);
                    NUnitAssert.AreEqual(2, actual.Count());

                    goods = new Good[]
                    {
                        new Good()
                        {
                            Product = new Preparation(){Name = "1"},
                            Supplier = new Supplier(){}
                        },
                        new Good()
                        {
                            Product = new Preparation(){Name = "1"},
                            Supplier = new Supplier(){}
                        }
                    }.AsEnumerable();

                    actual = search.StackLogic(goods as IEnumerable<IProduct>);
                    NUnitAssert.IsNotNull(actual);
                    NUnitAssert.AreEqual(1, actual.Count());
                }

                [Test]
                public static async Task WhenParamIsNullResultExeptionAsync()
                {
                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult((IEnumerable<Good>)null));
                    var algo = new Mock<ITopAlgorithm>();
                    var search = new preparation.Controllers.SearchController(streinger: strngr.Object, topAlgorithm: algo.Object);

                    NUnitAssert.CatchAsync(async () => search.StackLogic(await strngr.Object.Goods() as IEnumerable<IProduct>));
                }
            }
        }

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
