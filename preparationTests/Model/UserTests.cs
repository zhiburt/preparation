using preparation.Models.Account;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace preparationTests.Model
{
    [NUnit.Framework.TestFixture]
    public class UserTests

    {
        public class AddMessagesID
        {
            [Test]
            public void WhenDataValid()
            {
                //arrange
                string newId = "it's ID";
                var user = new User();
                var expected = new string[] { newId };
                //acctual
                user.AddMessagesID(newId);
                //assert
                Assert.AreEqual(expected, user.GetMessagesID());
                user.AddMessagesID(newId);
                expected = expected.Append(newId).ToArray();
                Assert.AreEqual(expected, user.GetMessagesID());
            }

            [Test]
            public void WhenDataInvalid()
            {
                //arrange
                string newId = "it's ID";
                var user = new User();
                var expected = new string[] { newId };
                //acctual
                user.AddMessagesID(newId);
                //assert
                Assert.AreEqual(expected, user.GetMessagesID());
                user.AddMessagesID(newId);
                expected = expected.Append(newId).ToArray();
                Assert.AreEqual(expected, user.GetMessagesID());
            }
        }
    }
}
