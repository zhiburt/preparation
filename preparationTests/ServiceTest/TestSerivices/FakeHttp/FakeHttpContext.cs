using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Formatters.Json.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using Newtonsoft.Json;
using preparation.Models;
using preparationTests.ServiceTest.TestSerivices.MockExtensions;

namespace preparationTests.ServiceTest.TestSerivices.FakeHttp
{
    public class FakeHttpContext
    {
        public static Mock<HttpContext> NewFakeHttpContext()
        {
            Mock<HttpContext> httpContext = new Mock<HttpContext>();

            return httpContext;
        }

        public static Mock<ISession> FakeSession()
        {
            var session = new Mock<ISession>();

            return session;
        }

        public static Mock<ISession> FakeSession(IDictionary<string, IProduct> products)
        {
            var session = new Mock<ISession>();
            SetupIterator(products);

            session.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback( () => products.Add(GetKeyStr(), new Good()));

            session.Setup(_ => _.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>( (a,b) => products.Add(GetKeyStr(), new Good()));

            session.Setup(_ => _.Remove(It.IsAny<string>()))
                .Callback<string>((key) => products.Remove(key));

            var o = UTF8Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Good()));
            session.Setup(_ => _.TryGetValue(It.IsAny<string>(), out o))
                .Returns(true);

            session.Setup(p => p.Keys)
                .Returns(products.Keys);

            return session;
        }

        private static int iterator = 0;
        private static void SetupIterator(IDictionary<string, IProduct> products)
        {
            iterator = products.Keys.Count > 0 ? products.Keys.Select(k => int.Parse(k.Split('_')[2])).Max() : 0;
            iterator++;
        }

        private static string GetKeyStr()
        {
            return $"cart_list_{iterator++}";
        }

        
    }
}
