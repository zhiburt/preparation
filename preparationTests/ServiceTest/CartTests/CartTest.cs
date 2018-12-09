using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using preparation.Models;
using preparation.Services.Cart;
using preparationTests.ServiceTest.TestSerivices.FakeHttp;
using Xunit;
using NAssert = NUnit.Framework.Assert;

namespace preparationTests.ServiceTest.CartTests
{
    //TODO add strategy NAME_SESSION_OBJS in cart
    public class CartTest
    {
        public class AddProductToCart
        {
            [Fact]
            public void AddProduct_WhenSessionIsNotClear_ResultAdd2ProductsToCart()
            {
                //Arrange
                var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_0", new Good()),
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                    new KeyValuePair<string, IProduct>("cart_list_2", new Good()),
                });


                ICollection<IProduct> collection = new List<IProduct>((IEnumerable<IProduct>)new[]
                {
                    new Good(),
                    new Good()
                });

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                cart.AddProduct(collection.ToArray());
                //Assert
                Assert.Equal(map.Count, cart.AmountProducts());
            }

            [Fact]
            public void AddProduct_WhenSessionClear_ResultAdd2ProductsToCart()
            {
                //Arrange
                var map = new Dictionary<string, IProduct>();


                ICollection<IProduct> collection = new List<IProduct>((IEnumerable<IProduct>)new[]
                {
                    new Good(),
                    new Good()
                });

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                cart.AddProduct(collection.ToArray());
                //Assert
                Assert.Equal(map.Count, cart.AmountProducts());
            }

            [Fact]
            public void AddProduct_WhenDataIsNULL_ExpectedExeption()
            {
                //Arrange
                var map = new Dictionary<string, IProduct>();

                IEnumerable<IProduct> collection = null;

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Assert

                Assert.Throws<ArgumentNullException>(() => cart.AddProduct((IProduct[])collection));
            }

            [Fact]
            public void AddProductSingle_WhenDataIsNULL_ExpectedExeption()
            {
                //Arrange
                var map = new Dictionary<string, IProduct>();
                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);

                var cart = new Cart(context.Object);

                //Assert
                Assert.Throws<ArgumentNullException>(() => cart.AddProduct((IProduct)null));
            }
        }

        public class RemoveProductFromCart
        {
            [Fact]
            public void RemoveProductWhenObjectExistsIn()
            {
                //Arrange
                var targetGood = new Good() { Price = 12.2m, Supplier = new Supplier() { Name = "Maxim" } };
                var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_0", targetGood),
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                });
                var expected = map;
                expected.Remove("cart_list_0");

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                cart.Remove(targetGood);
                //Assert
                Assert.Equal(expected, map);
            }

            [Fact]
            public void RemoveProductWhenObjectDoesnotExistsIn()
            {
                //Arrange
                var targetGood = new Good() { Price = 12.2m, Supplier = new Supplier() { Name = "Maxim" } };
                var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                });
                var expected = map;

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                cart.Remove(targetGood);
                //Assert
                Assert.Equal(expected, map);
            }

            [Fact]
            public void RemoveWhenProductIsNull()
            {
                //Arrange
                var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>> { });

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                cart.Remove(null);
                //Assert
                Assert.Equal(map, map);
            }
        }

        public class AllProducts
        {
            [Fact]
            public void GetAllProductsWhenOnesAreNotClear()
            {
                //Arrange
                var expectedList = new[]
                {
                    new Good(),
                    new Good()
                };
                var listObjects = new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                    new KeyValuePair<string, IProduct>("cart_list_2", new Good())
                };

                var map = new Dictionary<string, IProduct>(listObjects);

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(c => c.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                var actual = cart.All();
                //Assert
                Assert.Equal(expectedList, actual);
            }

            [Fact]
            public void GetAllProductsWhenOnesIsEmpty()
            {
                //Arrange
                var expected = new List<IProduct>();
                var map = new Dictionary<string, IProduct>();

                var context = FakeHttpContext.NewFakeHttpContext();
                context.SetupProperty(c => c.Session, FakeHttpContext.FakeSession(map).Object);
                var cart = new Cart(context.Object);
                //Actual
                var actual = cart.All();
                //Assert
                Assert.Equal(expected, actual);
            }
        }
    }

    [NUnit.Framework.TestFixture]
    public class RemoveProductFromCartNUnit
    {
        [NUnit.Framework.Test]
        public void RemoveProductWhenObjectExistsIn()
        {
            //Arrange
            var targetGood = new Good() { Price = 12.2m, Supplier = new Supplier() { Name = "Maxim" } };
            var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_0", targetGood),
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                });
            var expected = map;
            expected.Remove("cart_list_0");

            var context = FakeHttpContext.NewFakeHttpContext();
            context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
            var cart = new Cart(context.Object);
            //Actual
            cart.Remove(targetGood);
            //Assert
            Assert.Equal(expected, map);
        }

        [NUnit.Framework.Test]
        public void RemoveProductWhenObjectDoesnotExistsIn()
        {
            //Arrange
            var targetGood = new Good() { Price = 12.2m, Supplier = new Supplier() { Name = "Maxim" } };
            var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                });
            var expected = map;

            var context = FakeHttpContext.NewFakeHttpContext();
            context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
            var cart = new Cart(context.Object);
            //Actual
            cart.Remove(targetGood);
            //Assert
            NAssert.AreEqual(expected, map);
        }

        [Fact]
        public void RemoveWhenProductIsNull()
        {
            //Arrange
            var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>> { });

            var context = FakeHttpContext.NewFakeHttpContext();
            context.SetupProperty(e => e.Session, FakeHttpContext.FakeSession(map).Object);
            var cart = new Cart(context.Object);
            //Actual
            cart.Remove(null);
            //Assert
            NAssert.AreEqual(map, map);
        }
    }
}
