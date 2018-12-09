using System;
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
        public class Preparations
        {
            [Fact]
            public async Task GetAllPreparationsTestIsNotNull()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);

                //Actual
                var all = await streinger.Preparations();
                //Assert
                Assert.Null(all);
            }

            [Fact]
            public async Task GetPreparationsByName()
            {
                //Arrange
                var expected = new Preparation(){Name = "Hello world", ActiveIngredient = "Limon"};
                var result = JsonConvert.SerializeObject(new Dictionary<string, IGood>()
                {
                    {"data", expected}
                });

                var mok = new Mock<IExternalDb>();
                (string, string) p = ("name", "");
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, new[]
                    {
                        p
                    })).
                    Returns(Task.FromResult(result));

                var streinger = new Streinger(mok.Object);

                //Actual
                var preparation = await streinger.Preparations("");
                //Assert
                Assert.NotNull(preparation);
                Assert.Equal(expected, preparation);
            }

            [Fact]
            public async Task GetPreparationsByNameWhenNameInvalid()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);

                string namePrep = "INVALID_NAME_100_PERCENTS";
                //Actual
                var preparation = await streinger.Preparations(namePrep);
                //Assert
                Assert.Null(preparation);
            }

            [Fact]
            public async Task GetPreparationsByNameWhenNameIsCleanStr()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);


                string namePrep = "";
                //Actual
                var preparation = await streinger.Preparations(namePrep);
                //Assert
                Assert.Null(preparation);
            }
        }

        //    public class Suppliers
        //    {
        //        #region GET_SUPPLIERS

        //        [Fact]
        //        public async Task SuppliersByAddressAndName()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            var name = "First Company";
        //            var address = "Belarus Minsk 12 Surganova 37/2 ";
        //            //Actual
        //            var supp = await streinger.Suppliers(name, address);
        //            //Assert
        //            Assert.NotNull(supp);
        //            Assert.Equal(name, supp.Name);
        //        }

        //        [Fact]
        //        public async Task SuppliersByAddressAndNameNotExists()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            var name = "INVALID_NAME_100_PERCENTS";
        //            var address = "INVALID_ADDRESS_100_PERCENTS";
        //            //Actual
        //            var supp = await streinger.Suppliers(name, address);
        //            //Assert
        //            Assert.Null(supp);
        //        }

        //        [Fact]
        //        public async Task SuppliersByAddressAndNameWithEmptyParam()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            var name = "";
        //            var address = "";
        //            //Actual
        //            var supp = await streinger.Suppliers(name, address);
        //            //Assert
        //            Assert.Null(supp);
        //        }

        //        [Fact]
        //        public async Task SuppliersByIdWhenIdIsValid()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            var name = "First Company";
        //            //Actual
        //            var supp = await streinger.Suppliers(1);
        //            //Assert
        //            Assert.NotNull(supp);
        //            Assert.Equal(name, supp.Name);
        //        }

        //        [Fact]
        //        public async Task SuppliersByIdWhenIdInValid()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            var id = -10;
        //            //Actual
        //            var supp = await streinger.Suppliers(id);
        //            //Assert
        //            Assert.Null(supp);
        //        }

        //        #endregion


        //        [Fact]
        //        public async Task AddValidSupplierTest()
        //        {
        //            var streinger = new Streinger();
        //            var supp = new Supplier("Therd Company", "Therd Company", "Belarus Minsk 12 Surganova 37/2 ",
        //                "1232103asd7asdasd11128asd", "This is my second company");
        //            //Actual
        //            var val = await streinger.AddSupplier(supp);
        //            //Assert
        //            Assert.True(val);
        //            val = await streinger.AddSupplier(supp);
        //            //Assert
        //            Assert.False(val);
        //        }

        //        [Fact]
        //        public async Task RemoveSupplierThatExistsInTest()
        //        {
        //            var streinger = new Streinger();
        //            var supp = new Supplier("Therd Company", "Therd Company", "Belarus Minsk 12 Surganova 37/2 ",
        //                "1232103asd7asdasd11128asd", "This is my second company");
        //            //Actual
        //            var val = await streinger.RemoveSupplier(supp);
        //            //Assert
        //            Assert.True(val);
        //        }

        //        [Fact]
        //        public async Task RemoveSupplierThatNotExistsInTest()
        //        {
        //            var streinger = new Streinger();
        //            var supp = new Supplier("INVALID_COMPANY_100_PERCENTS", "", "INVALID_ADDRESS_100_PERCENTS",
        //                "", "");
        //            //Actual
        //            var val = await streinger.RemoveSupplier(supp);
        //            //Assert
        //            Assert.False(val);
        //        }

        //        [Fact]
        //        public async Task RemoveSupplierWhenInvalidParametrsTest()
        //        {
        //            var streinger = new Streinger();
        //            var supp = new Supplier("", "", "",
        //                "", "");
        //            //Actual
        //            var val = await streinger.RemoveSupplier(supp);
        //            //Assert
        //            Assert.False(val);
        //        }
        //    }

        //    public class Goods
        //    {
        //        /// <summary>
        //        /// get preparations with supplier and price byName
        //        /// </summary>
        //        /// <param name="prepName">name preparation</param>
        //        /// <returns>preparations</returns>
        //        [Fact]
        //        public async Task GetGoodsWhenNameIsValid()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            //Actual
        //            IEnumerable<Good> goods = await streinger.Goods("Бетасерк");
        //            //Assert
        //            Assert.NotNull(goods);
        //        }

        //        [Fact]
        //        public async Task GetGoodsWhenNameInValid()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            //Actual
        //            IEnumerable<Good> goods = await streinger.Goods("INVALID_NAME_100_PERCENTS");
        //            //Assert
        //            Assert.Null(goods);
        //        }

        //        [Fact]
        //        public async Task GetGoodsWhenNameEmpty()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            //Actual
        //            IEnumerable<Good> goods = await streinger.Goods("");
        //            //Assert
        //            Assert.Null(goods);
        //        }

        //        #region AddGoods

        //        [Fact]
        //        public async Task AddGoodWhenValidParam()
        //        {
        //            var streinger = new Streinger();
        //            decimal price = 13.123m;
        //            var supp = new Supplier("First Company", "First Company", "Belarus Minsk 12 Surganova 37/2 ",
        //                "", "");
        //            var prep = new Preparation{Name = "Бетасерк" };
        //            var good = new Good {Price = price, Supplier = supp, Product = prep};
        //            //Actual
        //            var val = await streinger.AddGood(good);
        //            //Assert
        //            Assert.True(val);
        //            val = await streinger.AddGood(good);
        //            //Assert
        //            Assert.False(val);
        //        }

        //        [Fact]
        //        public async Task AddGoodWithInValidParam()
        //        {
        //            var streinger = new Streinger();
        //            var prep = new Preparation("INVALID", "", "", "", "Буг", "");
        //            var good = new Good { Price = 0, Supplier = new Supplier(), Product = prep };
        //            //Actual
        //            var val = await streinger.AddGood(good);
        //            //Assert
        //            Assert.False(val);
        //        }

        //        [Fact]
        //        public async Task AddGoodWhenParamEmpty()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            //Actual
        //            //Assert
        //            await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.AddGood(null));
        //        }

        //        #endregion

        //        #region GOOD_REMOVE

        //        [Fact]
        //        public async Task RemoveGoodWhenValidParam()
        //        {
        //            var streinger = new Streinger();
        //            var supp = new Supplier("First Company", "First Company", "Belarus Minsk 12 Surganova 37/2 ",
        //                "", "");
        //            var prep = new Preparation { Name = "Бетасерк" };
        //            var good = new Good { Supplier = supp, Product = prep };
        //            //Actual
        //            var val = await streinger.RemoveGood(good);
        //            //Assert
        //            Assert.True(val);
        //            val = await streinger.RemoveGood(good);
        //            //Assert
        //            Assert.False(val);
        //        }

        //        [Fact]
        //        public async Task RemoveGoodWithInValidParam()
        //        {
        //            var streinger = new Streinger();
        //            var prep = new Preparation("INVALID", "", "", "", "Буг", "");
        //            var good = new Good { Price = 0, Supplier = null, Product = prep };
        //            //Actual
        //            //Assert
        //            await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.RemoveGood(good));

        //        }

        //        [Fact]
        //        public async Task RemoveGoodWhenParamEmpty()
        //        {
        //            //Arrange
        //            var streinger = new Streinger();
        //            //Actual
        //            //Assert
        //            await Assert.ThrowsAsync<ArgumentNullException>(async () => await streinger.RemoveGood(null));
        //        }

        //        #endregion
        //    }
        //}
    }
}
