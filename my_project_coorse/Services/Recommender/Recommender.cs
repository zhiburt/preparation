using preparation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Services.Streinger;

namespace preparation.Services.Recommender
{
    public class Recommender : IRecommender
    {
        private List<string> commentsList = new List<string>()
        {
            "{0} is the best product",
            "I wanna buy all these {0} by {2} for {1} bucks",
            "perfectly",
            "incredebel",
            "It's jumbl delivery but {1} is ok at this price",
        };

        private readonly IStreinger _streinger;

        public Recommender(IStreinger streinger)
        {
            this._streinger = streinger;
        }


        public IEnumerable<string> CommentsTo(IProduct product)
        {
            if (product == null) return null;

            List<string> comments = new List<string>();
            Random rnd = new Random(DateTime.UtcNow.Millisecond);
            int amauntComments = rnd.Next(3, 10);
            for (int i = 0; i < amauntComments; i++)
            {
                comments.Add(RandomComment(product, rnd));
            }

            return comments;
        }

        public IEnumerable<IProduct> RecommendationsTo(IProduct product)
        {
            if (product == null) return null;

            var recommendations = new List<IProduct>();
            Random rnd = new Random();

            var goods = _streinger.Goods().Result;
            foreach (var good in goods)
            {
                int i = rnd.Next(3);
                if (i % 3 == 0)
                {
                    recommendations.Add(good);
                }
            }

            return recommendations;
        }

        #region private

        private string RandomComment(IProduct product, Random rnd)
        {
            string rndComment = commentsList.ElementAt(rnd.Next(commentsList.Count));

            return string.Format(rndComment, product.Product.Name, product.Price, product.Supplier.Name);
        }

        #endregion
    }
}
