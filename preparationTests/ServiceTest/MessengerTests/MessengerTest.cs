using NUnit.Framework;
using preparation.Services.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using preparationTests.ServiceTest.TestSerivices.Authefication;
using preparation.Models.Account;
using preparation.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using preparation.Models.DbEntity;

namespace preparationTests.ServiceTest.MessengerTests
{
    [TestFixture]
    public class MessengerTest
    {
        public class Send
        {
            public class WhenDataValid
            {
                private UserManager<User> userManager;
                private List<User> users;

                private DbContextOptions<MessengerContext> DammyOptions { get; } = new DbContextOptionsBuilder<MessengerContext>().Options;

                [SetUp]
                public void SetUserManager()
                {
                    users = new List<User>() {
                        new User() { FirstName = "Maxim" },
                        new User() { FirstName = "Evgeniy" }
                    };

                    var mock = FakeTestingService.MockUserManager(users);
                    mock.Setup(ex => ex.Users).Returns(users.AsQueryable());
                    mock.Setup(ex => ex.FindByIdAsync(It.IsAny<string>()))
                        .Returns<string>(s => Task.FromResult(users.Find(u => u.Id == s)));

                    userManager = mock.Object;
                }

                [Test]
                public async Task ResultChange()
                {
                    var context = new Mock<MessengerContext>(DammyOptions);
                    var messenger = new Messenger(userManager, context.Object);

                    await messenger.SendV2(new {Data = "empty", Type = "empty"}, users[0], users[1]);

                    Assert.AreNotEqual(0, users[1].GetMessagesID().Count());
                }
            }
        }
    }
}
