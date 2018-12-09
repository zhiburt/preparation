using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using NAssert = NUnit.Framework.Assert;
using preparation.Models.Account;
using preparation.ViewModels.Account;
using preparationTests.ServiceTest.TestSerivices.Authefication;
using Xunit;
using Assert = Xunit.Assert;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace preparationTests.Controllers.AccountController
{
    public class AccauntControllerTests
    {
        public class Register
        {
            private static List<User> _users = new List<User>
            {
                new User() { Id = "1", UserName = "User1", Email = "user1@bv.com"},
                new User() { Id = "2", UserName = "User2", Email = "user2@bv.com"},
            };


            [Fact]
            public async Task RegisterWhenDataInvalid_ExpectedNotChangeBD()
            {
                //Arrange
                var userManager = FakeTestingService.MockUserManager<User>(_users);
                userManager.Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());


                var signInManager = FakeTestingService.MockSightInManager<User>(userManager.Object);

                var accaunt = new preparation.Controllers.AccountController(userManager.Object, signInManager.Object);
                var regModel = new UserRegisterViewModel()
                {
                    Address = "test",
                    AgreementConfirm = false,
                    Country = "test",
                    Email = "test",
                    FirstName = "test",
                    Password = "test",
                    Username = "test",
                    Surname = "test",
                    PasswordConfirm = "test"
                };
                string testRedirect = "/";
                //Actual
                var actual = await accaunt.Register(regModel, testRedirect);
                //Assert
                Assert.Equal(2, _users.Count);
                var model = Assert.IsType<ViewResult>(actual);
                Assert.IsAssignableFrom<UserRegisterViewModel>(model.ViewData.Model);
            }

            [Fact]
            public async Task RegisterWhenValidData_ExpectedOKBeheivior()
            {
                //Arrange
                var userManager = FakeTestingService.MockUserManager<User>(_users);
                var signInManager = FakeTestingService.MockSightInManager<User>(userManager.Object);
                
                var accaunt = new preparation.Controllers.AccountController(userManager.Object, signInManager.Object);
                var regModel = new UserRegisterViewModel()
                {
                    Address = "test", AgreementConfirm = false, Country = "test",
                    Email = "test", FirstName = "test", Password = "test", 
                    Username = "test", Surname = "test", PasswordConfirm = "test"
                };
                string testRedirect = "/";
                int expectedCount = 3;
                //Actual
                var actual = await accaunt.Register(regModel, testRedirect);
                //Assert
                Assert.Equal(expectedCount, _users.Count);
            }

        }

        [TestFixture]
        public class Login
        {
            private static List<User> _users = null;

            [SetUp]
            public void SetUp()
            {
                _users = new List<User>
                {
                    new User() { Id = "1", UserName = "User1", Email = "user1@bv.com"},
                    new User() { Id = "2", UserName = "User2", Email = "user2@bv.com"},
                };
            }

            [Test]
            public async Task AutheficationWhenValidData_ExpectedOKModel()
            {
                //Arrange
                var loginModel = new UserLoginViewModel()
                {
                    Email = "user1@bv.com",
                    Password = "",
                    RememberMe = false
                };

                var userManager = FakeTestingService.MockUserManager<User>(_users);
                userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync(_users[0]);
                var signInManager = FakeTestingService.MockSightInManager<User>(userManager.Object);

                signInManager.Setup(m =>
                        m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                    .ReturnsAsync(SignInResult.Success);


                var accaunt = new preparation.Controllers.AccountController(userManager.Object, signInManager.Object);

                string testRedirect = "/";
                //Actual
                var actual = await accaunt.Login(loginModel, testRedirect);
                //Assert
                var actualVuewResult = Assert.IsType<RedirectToActionResult>(actual);
            }


            [Test]
            public async Task AutheficationWhenDataInvalid_ExpectedNotOKModel()
            {
               
            //Arrange
            var loginModel = new UserLoginViewModel()
                {
                    Email = "user1@bv.com",
                    Password = "",
                    RememberMe = false
                };

                var userManager = FakeTestingService.MockUserManager<User>(_users);
                userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                    .ReturnsAsync(null as User);
                var signInManager = FakeTestingService.MockSightInManager<User>(userManager.Object);

                signInManager.Setup(m =>
                        m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                    .ReturnsAsync(SignInResult.Success);


                var accaunt = new preparation.Controllers.AccountController(userManager.Object, signInManager.Object);

                string testRedirect = "/";
                //Actual
                var actual = await accaunt.Login(loginModel, testRedirect);
                //Assert
                var actualVuewResult = actual as ViewResult;
                NAssert.IsInstanceOf(typeof(ViewResult), actual);
                NAssert.IsAssignableFrom(typeof(UserLoginViewModel), actualVuewResult.ViewData.Model);
                NAssert.True(actualVuewResult.ViewData.ModelState.ErrorCount > 0);
            }
        }
    }
}
