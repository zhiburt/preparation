using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using preparation.Models;
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
            public async Task GetGoodsWhenNameIsValid()
            {
                //Arrange
                var streinger = new Streinger();
                //Actual
                IEnumerable<Good> goods = await streinger.Goods("Бетасерк");
                //Assert
                Assert.NotNull(goods);
            }

            [Fact]
            public async Task GetGoodsWhenNameInValid()
            {
                //Arrange
                var streinger = new Streinger();
                //Actual
                IEnumerable<Good> goods = await streinger.Goods("INVALID_NAME_100_PERCENTS");
                //Assert
                Assert.Null(goods);
            }

            [Fact]
            public async Task GetGoodsWhenNameEmpty()
            {
                //Arrange
                var streinger = new Streinger();
                //Actual
                IEnumerable<Good> goods = await streinger.Goods("");
                //Assert
                Assert.Null(goods);
            }

            #region AddGoods

            [Fact]
            public async Task AddGoodWhenValidParam()
            {
                var streinger = new Streinger();
                decimal price = 13.123m;
                var supp = new Supplier("First Company", "First Company", "Belarus Minsk 12 Surganova 37/2 ",
                    "", "");
                var prep = new Preparation { Name = "Бетасерк" };
                var good = new Good { Price = price, Supplier = supp, Product = prep };
                //Actual
                var val = await streinger.AddGood(good);
                //Assert
                Assert.True(val);
                val = await streinger.AddGood(good);
                //Assert
                Assert.False(val);
            }

            [Fact]
            public async Task AddGoodWithInValidParam()
            {
                var streinger = new Streinger();
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
                var streinger = new Streinger();
                //Actual
                //Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.AddGood(null));
            }

            #endregion

            #region GOOD_REMOVE

            [Fact]
            public async Task RemoveGoodWhenValidParam()
            {
                var streinger = new Streinger();
                var supp = new Supplier("First Company", "First Company", "Belarus Minsk 12 Surganova 37/2 ",
                    "", "");
                var prep = new Preparation { Name = "Бетасерк" };
                var good = new Good { Supplier = supp, Product = prep };
                //Actual
                var val = await streinger.RemoveGood(good);
                //Assert
                Assert.True(val);
                val = await streinger.RemoveGood(good);
                //Assert
                Assert.False(val);
            }

            [Fact]
            public async Task RemoveGoodWithInValidParam()
            {
                var streinger = new Streinger();
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
                var streinger = new Streinger();
                //Actual
                //Assert
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.RemoveGood(null));
            }

            #endregion
        }
    }
}
