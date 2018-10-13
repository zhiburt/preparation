using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using preparation.Services.Streinger;
using Xunit;

namespace preparationTests.ServiceTest
{
    public class StreingerTest
    {   
        public class Preparations
        {
            [Fact]
            public async Task GetAllPreparationsTestIsNotNull()
            {
                //Arrange
                var streinger = new Streinger();
                //Actual
                var all = await streinger.Preparations();
                //Assert
                Assert.NotNull(all);
            }

            [Fact]
            public async Task GetPreparationsByName()
            {
                //Arrange
                var streinger = new Streinger();
                string namePrep = "Длянос";
                //Actual
                var preparation = await streinger.Preparations(namePrep);
                //Assert
                Assert.NotNull(preparation);
                Assert.Equal("Ксилометазолин", preparation.ActiveIngredient);
            }

            [Fact]
            public async Task GetPreparationsByNameWhenNameInvalid()
            {
                //Arrange
                var streinger = new Streinger();
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
                var streinger = new Streinger();
                string namePrep = "";
                //Actual
                var preparation = await streinger.Preparations(namePrep);
                //Assert
                Assert.Null(preparation);
            }
        }
    }
}
