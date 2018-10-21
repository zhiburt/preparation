using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using preparation.Models.Account;
using preparation.ViewModels.Account;
using preparationTests.ServiceTest.TestSerivices.Authefication;
using Xunit;
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
            public async Task RegisterWhenValidData()
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

            [Fact]
            public async Task AutheficationWhenValidData()
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
        }
    }
}
