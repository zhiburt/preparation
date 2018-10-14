﻿using System;
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
        public class Suppliers
        {
            #region GET_SUPPLIERS

            [Fact]
            public async Task GetSuppliersByAddressAndName()
            {
                //Arrange
                var streinger = new Streinger();
                var name = "First Company";
                var address = "Belarus Minsk 12 Surganova 37/2 ";
                //Actual
                var supp = await streinger.Suppliers(name, address);
                //Assert
                Assert.NotNull(supp);
                Assert.Equal(name, supp.Name);
            }

            [Fact]
            public async Task GetSuppliersByAddressAndNameNotExists()
            {
                //Arrange
                var streinger = new Streinger();
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
                var streinger = new Streinger();
                var name = "";
                var address = "";
                //Actual
                var supp = await streinger.Suppliers(name, address);
                //Assert
                Assert.Null(supp);
            }

            [Fact]
            public async Task GetSuppliersByIdWhenIdIsValid()
            {
                //Arrange
                var streinger = new Streinger();
                var name = "First Company";
                //Actual
                var supp = await streinger.Suppliers(1);
                //Assert
                Assert.NotNull(supp);
                Assert.Equal(name, supp.Name);
            }

            [Fact]
            public async Task GetSuppliersByIdWhenIdInValid()
            {
                //Arrange
                var streinger = new Streinger();
                var id = -10;
                //Actual
                var supp = await streinger.Suppliers(id);
                //Assert
                Assert.Null(supp);
            }

            #endregion


            [Fact]
            public async Task AddValidSupplierTest()
            {
                var streinger = new Streinger();
                var supp = new Supplier("Therd Company", "Therd Company", "Belarus Minsk 12 Surganova 37/2 ",
                    "1232103asd7asdasd11128asd", "This is my second company");
                //Actual
                var val = await streinger.AddSupplier(supp);
                //Assert
                Assert.True(val);
                val = await streinger.AddSupplier(supp);
                //Assert
                Assert.False(val);
            }

            [Fact]
            public async Task RemoveSupplierThatExistsInTest()
            {
                var streinger = new Streinger();
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
                var streinger = new Streinger();
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
                var streinger = new Streinger();
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