using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using preparation.Models;
using preparation.Services.Recommender;
using preparation.Services.Streinger;

namespace preparationTests.ServiceTest.RecommenderTests
{
    [TestFixture]
    public class RecommenderTests
    {
        public class Comments
        {
            private IStreinger strngr;

            [SetUp]
            public void SetUp()
            {
                var strngr = new Mock<IStreinger>();
                strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(new Good[] { new Good() }.AsEnumerable()));
                strngr.Setup(ex => ex.Goods(It.IsAny<string>())).Returns(Task.FromResult(new Good[] { new Good() }.AsEnumerable()));
                this.strngr = strngr.Object;
            }

            [Test]
            public void WhenParamIsNullResultNull()
            {
                var recommender = new Recommender(strngr);

                var comments =  recommender.CommentsTo(null);
                Assert.IsNull(comments);
            }

            [Test]
            public void WhenParamIsInvalidResultExeption()
            {
                var recommender = new Recommender(strngr);
                var prod = new Good() { };
                
                Assert.Catch<NullReferenceException>(() => recommender.CommentsTo(product: prod));
            }

            [Test]
            public void WhenParamIsNotNullResultOK()
            {
                var recommender = new Recommender(strngr);
                var prod = new Good() {Product = new Preparation(), Supplier = new Supplier()};

                var comments = recommender.CommentsTo(product: prod);

                Assert.NotNull(comments);
            }
        }

        public class Recommendations
        {
            public class Comments
            {
                private IStreinger strngr;

                [SetUp]
                public void SetUp()
                {
                    var strngr = new Mock<IStreinger>();
                    strngr.Setup(ex => ex.Goods()).Returns(Task.FromResult(new Good[] { new Good() }.AsEnumerable()));
                    strngr.Setup(ex => ex.Goods(It.IsAny<string>())).Returns(Task.FromResult(new Good[] { new Good() }.AsEnumerable()));
                    this.strngr = strngr.Object;
                }

                [Test]
                public void WhenParamIsNullResultNull()
                {
                    var recommender = new Recommender(strngr);

                    var recommendations = recommender.RecommendationsTo(null);

                    Assert.IsNull(recommendations);
                }

                [Test]
                public void WhenParamIsInvalidResultOK()
                {
                    var recommender = new Recommender(strngr);
                    var prod = new Good() { };

                    var recommendations = recommender.RecommendationsTo(product: prod);

                    Assert.IsNotNull(recommendations);
                }

                [Test]
                public void WhenParamIsNotNullResultOK()
                {
                    var recommender = new Recommender(strngr);
                    var prod = new Good() { Product = new Preparation(), Supplier = new Supplier() };

                    var recommendations = recommender.RecommendationsTo(product: prod);

                    Assert.NotNull(recommendations);
                }
            }
        }
    }
}
