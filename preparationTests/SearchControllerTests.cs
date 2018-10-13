using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation;
using preparation.Controllers;
using preparation.Models;
using preparation.Services.Streinger;
using Xunit;
using Xunit.Sdk;

namespace preparationTests
{
    public partial class SearchControllerTests
    {
        public class Get
        {
            [Fact]
            public async Task PreparationsWhenServiceResponceValidData()
            {
                //Arrange
                IStreinger streinger = new Streinger();
                var seachController = new SearchController(streinger);
                //Actual
                var resp = await seachController.Index();
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                IEnumerable<Preparation> model =
                    Assert.IsAssignableFrom<IEnumerable<Preparation>>(viewResult.ViewData.Model);
                Assert.Equal(model, await streinger.Preparations());
            }

            [Fact]
            public async Task PreparationsWithSearchStr()
            {
                //Arrange
                IStreinger streinger = new Streinger();
                var seachController = new SearchController(streinger);
                string preparationName = "Эхинацеи экстракт";
                //Actual
                var resp = await seachController.Search(preparationName);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                Preparation model =
                    Assert.IsAssignableFrom<Preparation>(viewResult.ViewData.Model);
                Assert.Equal(model, await streinger.Preparations(preparationName));
            }

            [Fact]
            public async Task PreparationsWhenSearchIsEmptyStr()
            {
                //Arrange
                IStreinger streinger = new Streinger();
                var seachController = new SearchController(streinger);
                string preparationName = "";
                //Actual
                var resp = await seachController.Search(preparationName);
                //Assert
                var viewResult = Assert.IsType<ViewResult>(resp);
                Assert.Null(viewResult.ViewData.Model);
            }
        }
    }
}
