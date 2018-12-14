using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using preparation.Controllers;
using preparation.Models;
using preparation.Services.Recommender;
using preparation.Services.Streinger;
using preparation.ViewModels.Product;

namespace preparationTests.Controllers.ProductControllerTest
{
    [TestFixture]
    public class ProductControllerTests
    {

        [TestFixture]
        public class ProductView
        {
            [Test]
            public async Task WhenNameInvalidResultNull()
            {

                var controller = new ProductController(
                    new Mock<IStreinger>().Object,
                    new Mock<IRecommender>().Object);

                var view = await controller.Product("exist");
                Assert.IsAssignableFrom<ViewResult>(view);
                var model = (view as ViewResult).ViewData.Model;

                Assert.IsNull(model);
            }

            [Test]
            public async Task WhenNameValidResultIsOK()
            {
                var strngr = new Mock<IStreinger>();
                strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(new Good[] { new Good() }.AsEnumerable()));
                strngr.Setup(ex => ex.Goods(It.IsAny<string>())).Returns(Task.FromResult(new Good[] { new Good() }.AsEnumerable()));
                var rcmm = new Mock<IRecommender>();


                var controller = new ProductController(
                    streinger: strngr.Object,
                    recommendator: rcmm.Object);

                var view = await controller.Product("exist");
                Assert.IsAssignableFrom<ViewResult>(view);
                var model = (view as ViewResult).ViewData.Model;

                Assert.IsNotNull(model);
            }
        }

        public class Integration
        {
            public class ProductTest
            {

                private IStreinger strngr;

                [SetUp]
                public void SetUp()
                {
                    var goods = new Good[]
                        {
                        new Good(){Product = new Preparation(), Supplier = new Supplier()}
                        }.AsEnumerable();

                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(goods));
                    strngr.Setup(ex => ex.Goods(It.IsAny<string>())).Returns(Task.FromResult(goods));
                    strngr.Setup(ex => ex.Goods("NOT_EXISTS")).Returns(Task<IEnumerable<Good>>.FromResult((IEnumerable<Good>)null));

                    this.strngr = strngr.Object;
                }

                [Test]
                public async Task WhenNameInvalidResultNull()
                {
                    var rec = new Recommender(strngr);
                    var controller = new ProductController(strngr, recommendator: rec);

                    var view = await controller.Product("NOT_EXISTS");
                    Assert.IsAssignableFrom<ViewResult>(view);
                    var model = (view as ViewResult).ViewData.Model;

                    Assert.IsNull(model);
                }
                
                [Test]
                public async Task WhenNameValidResultOK()
                {
                    var rec = new Recommender(strngr);
                    var controller = new ProductController(strngr, recommendator: rec);

                    var view = await controller.Product("exists_objects");
                    Assert.IsAssignableFrom<ViewResult>(view);
                    var model = (view as ViewResult).ViewData.Model;

                    Assert.IsNotNull(model);
                }

                [Test]
                public async Task WhenRecommenderIsNullResultExeption()
                {
                    var rec = new Recommender(strngr);
                    var controller = new ProductController(streinger: strngr, recommendator: null);
                    
                    Assert.CatchAsync<NullReferenceException>(() => controller.Product("exists_objects"));
                }
            }
        }
    }
}
