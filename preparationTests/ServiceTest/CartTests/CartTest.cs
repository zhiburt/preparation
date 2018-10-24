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

namespace preparationTests.ServiceTest.CartTests
{
    public class CartTest
    {
        public class AddProductToCart
        {
            [Fact]
            public void AddProductWhenSessionIsNotClear()
            {
                //Arrange
                var map = new Dictionary<string, IProduct>(new List<KeyValuePair<string, IProduct>>
                {
                    new KeyValuePair<string, IProduct>("cart_list_0", new Good()),
                    new KeyValuePair<string, IProduct>("cart_list_1", new Good()),
                    new KeyValuePair<string, IProduct>("cart_list_2", new Good()),
                });
                

                ICollection<IProduct> collection = new List<IProduct>((IEnumerable<IProduct>) new[]
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
            public void AddProductWhenSessionClear()
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
            public void AddProductWhenDataIsNULL()
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
            public void AddProductSingleWhenDataIsNULL()
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
    }
}
