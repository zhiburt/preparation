﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using preparation.Models;
using preparation.Services.ExternalDB;
using preparation.Services.Streinger;
using Xunit;
using Xunit.Sdk;

namespace preparationTests.ServiceTest.StreingerTests
{
    public partial class StreingerTest
    {
        public class Goods
        {
            /// <summary>
            /// get preparations with supplier and price byName
            /// </summary>
            /// <param name="prepName">name preparation</param>
            /// <returns>preparations</returns>
            [Fact]
            public async Task GetGoodsWhenNameIsInvalid()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)[]>()))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);
                //Actual
                IEnumerable<Good> goods = await streinger.Goods("Бетасерк");
                //Assert
                Assert.Null(goods);
            }

            [Fact]
            public async Task GetGoods()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);
                //Actual
                IEnumerable<Good> goods = await streinger.Goods();
                //Assert
                Assert.Null(goods);
            }

            //[Fact]
            //public async Task GetGoodsWhenNameInValid()
            //{
            //    //Arrange
            //    var mok = new Mock<IExternalDb>();
            //    mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
            //        .Returns(Task.FromResult(@""));
            //    var streinger = new Streinger(mok.Object);
            //    //Actual
            //    IEnumerable<Good> goods = await streinger.Goods("INVALID_NAME_100_PERCENTS");
            //    //Assert
            //    Assert.Null(goods);
            //}

            [Fact]
            public async Task GetGoodsWhenNameEmpty()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);
                //Actual
                IEnumerable<Good> goods = await streinger.Goods("");
                //Assert
                Assert.Null(goods);
            }

            #region AddGoods

            //[Fact]
            //public async Task AddGoodWhenValidParam()
            //{
            //    var serializeInfo = JsonConvert.SerializeObject(new Dictionary<string, object>()
            //    {
            //        {"string", new Supplier()}
            //    });
            //    var mok = new Mock<IExternalDb>();
            //    mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Put, new(string, string)[]
            //        {
            //            new Tuple<string, string>("preparation_id"," ").ToValueTuple(),
            //            Tuple.Create("supplier_id", $" ").ToValueTuple(),
            //            Tuple.Create("price", " ").ToValueTuple(),
            //        }))
            //        .Returns(Task.FromResult(@"""status"": ""ok"""));
            //    mok.Setup(e => e.AskService(It.IsAny<string>(), null, new[]
            //        {
            //            Tuple.Create("address", "").ToValueTuple(),
            //            Tuple.Create("name", "").ToValueTuple()
            //        }))
            //        .Returns(Task.FromResult(serializeInfo));



            //    var streinger = new Streinger(mok.Object);

            //    var good = new Good
            //    {
            //        Price = 13.123m,
            //        Supplier = new Supplier(
            //            "First Company",
            //            "First Company",
            //            "Belarus Minsk 12 Surganova 37/2 ",
            //            "",
            //            ""),
            //        Product = new Preparation { Name = "Бетасерк" } };
            ////Actual
            //var val = await streinger.AddGood(good);
            ////Assert
            //Assert.True(val);
            //val = await streinger.AddGood(good);
            ////Assert
            //Assert.False(val);
            //}

        [Fact]
        public async Task AddGoodWithInValidParam()
        {
            var mok = new Mock<IExternalDb>();
            mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                .Returns(Task.FromResult(@""));
            var streinger = new Streinger(mok.Object);
            var prep = new Preparation("INVALID", "", "", "", "Буг", "");
            var good = new Good { Price = 0, Supplier = new Supplier(), Product = prep };
            //Actual
            var val = await streinger.AddGood(good);
            //Assert
            Assert.False(val);
        }

        [Fact]
        public async Task AddGoodWhenParamEmpty()
        {
            //Arrange
            var mok = new Mock<IExternalDb>();
            mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                .Returns(Task.FromResult(@""));
            var streinger = new Streinger(mok.Object);
            //Actual
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.AddGood(null));
        }

        #endregion

        #region GOOD_REMOVE

        //[Fact]
        //public async Task RemoveGoodWhenValidParam()
        //{
        //    var mok = new Mock<IExternalDb>();
        //    mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
        //        .Returns(Task.FromResult(@""));
        //    var streinger = new Streinger(mok.Object);
        //    var supp = new Supplier("First Company", "First Company", "Belarus Minsk 12 Surganova 37/2 ",
        //        "", "");
        //    var prep = new Preparation { Name = "Бетасерк" };
        //    var good = new Good { Supplier = supp, Product = prep };
        //    //Actual
        //    var val = await streinger.RemoveGood(good);
        //    //Assert
        //    Assert.True(val);
        //    val = await streinger.RemoveGood(good);
        //    //Assert
        //    Assert.False(val);
        //}

        [Fact]
        public async Task RemoveGoodWithInValidParam()
        {
            var mok = new Mock<IExternalDb>();
            mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                .Returns(Task.FromResult(@""));
            var streinger = new Streinger(mok.Object);

            var prep = new Preparation("INVALID", "", "", "", "Буг", "");
            var good = new Good { Price = 0, Supplier = null, Product = prep };
            //Actual
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.RemoveGood(good));

        }

        [Fact]
        public async Task RemoveGoodWhenParamEmpty()
        {
            //Arrange
            var mok = new Mock<IExternalDb>();
            mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                .Returns(Task.FromResult(@""));
            var streinger = new Streinger(mok.Object);
            //Actual
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.RemoveGood(null));
        }

        #endregion
    }
}
}
