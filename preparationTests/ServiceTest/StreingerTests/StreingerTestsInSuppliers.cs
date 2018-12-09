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
        public class Suppliers
        {
            #region GET_SUPPLIERS

            [Fact]
            public async Task GetSuppliersByAddressAndName()
            {
                //Arrange
                var supp = new Supplier()
                {
                    Name = "First Company",
                    Address = "Belarus Minsk 12 Surganova 37/2 "
                };
                var serializeSupp = JsonConvert.SerializeObject(new Dictionary<string, Supplier>()
                {
                    {"data", supp }
                });
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)[]>()))
                    .Returns(Task.FromResult(serializeSupp));
                var streinger = new Streinger(mok.Object);

                //Actual
                var suppResp = await streinger.Suppliers(supp.Name, supp.Address);
                //Assert
                Assert.NotNull(supp);
                Assert.Equal(supp, suppResp);
            }

            [Fact]
            public async Task GetSuppliersByAddressAndNameNotExists()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);

                var name = "INVALID_NAME_100_PERCENTS";
                var address = "INVALID_ADDRESS_100_PERCENTS";
                //Actual
                var supp = await streinger.Suppliers(name, address);
                //Assert
                Assert.Null(supp);
            }

            [Fact]
            public async Task GetSuppliersByAddressAndNameWithEmptyParam()
            {
                //Arrange
                var supp = new Supplier()
                {
                    Name = "First Company",
                    Address = "Belarus Minsk 12 Surganova 37/2 "
                };
                var serializeSupp = JsonConvert.SerializeObject(new Dictionary<string, Supplier>()
                {
                    {"data", supp }
                });

                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)[]>()))
                    .Returns(Task.FromResult(serializeSupp));
                var streinger = new Streinger(mok.Object);

                var name = "";
                var address = "";
                //Actual
                var suppEx = await streinger.Suppliers(name, address);
                //Assert
                Assert.NotNull(suppEx);
            }

            [Fact]
            public async Task GetSuppliersByIdWhenIdIsValid()
            {
                //Arrange
                var supp = new Supplier()
                {
                    Name = "First Company",
                    Address = "Belarus Minsk 12 Surganova 37/2 "
                };
                var serializeSupp = JsonConvert.SerializeObject(new Dictionary<string, Supplier>()
                {
                    {"data", supp }
                });
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)[]>()))
                    .Returns(Task.FromResult(serializeSupp));

                var streinger = new Streinger(mok.Object);

                //Actual
                var suppEx = await streinger.Suppliers(1);
                //Assert
                Assert.NotNull(supp);
                Assert.Equal(supp, suppEx);
            }

            [Fact]
            public async Task GetSuppliersByIdWhenIdInValid()
            {
                //Arrange
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@"hello_world"));

                var streinger = new Streinger(mok.Object);

                var id = -10;
                //Actual
                var supp = await streinger.Suppliers(id);
                //Assert
                Assert.Null(supp);
            }

            #endregion


            //[Fact]
            //public async Task AddValidSupplierTest()
            //{
            //    var mok = new Mock<IExternalDb>();
            //    mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
            //        .Returns(Task.FromResult(@""));
            //    var streinger = new Streinger(mok.Object);
            //    var supp = new Supplier("Therd Company", "Therd Company", "Belarus Minsk 12 Surganova 37/2 ",
            //        "1232103asd7asdasd11128asd", "This is my second company");
            //    //Actual
            //    var val = await streinger.AddSupplier(supp);
            //    //Assert
            //    Assert.True(val);
            //    val = await streinger.AddSupplier(supp);
            //    //Assert
            //    Assert.False(val);
            //}

            [Fact]
            public async Task RemoveSupplierThatExistsInTest()
            {
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<(string, string)[]>()))
                    .Returns(Task.FromResult(@"{status : ""ok""}"));
                var streinger = new Streinger(mok.Object);

                var supp = new Supplier("Therd Company", "Therd Company", "Belarus Minsk 12 Surganova 37/2 ",
                    "1232103asd7asdasd11128asd", "This is my second company");
                //Actual
                var val = await streinger.RemoveSupplier(supp);
                //Assert
                Assert.True(val);
            }

            [Fact]
            public async Task RemoveSupplierThatNotExistsInTest()
            {
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);

                var supp = new Supplier("INVALID_COMPANY_100_PERCENTS", "", "INVALID_ADDRESS_100_PERCENTS",
                    "", "");
                //Actual
                var val = await streinger.RemoveSupplier(supp);
                //Assert
                Assert.False(val);
            }

            [Fact]
            public async Task RemoveSupplierWhenInvalidParametrsTest()
            {
                var mok = new Mock<IExternalDb>();
                mok.Setup(e => e.AskService(It.IsAny<string>(), HttpMethod.Get, null))
                    .Returns(Task.FromResult(@""));
                var streinger = new Streinger(mok.Object);

                var supp = new Supplier("", "", "",
                    "", "");
                //Actual
                var val = await streinger.RemoveSupplier(supp);
                //Assert
                Assert.False(val);
            }
        }
    }
}