using System.Collections.Generic;
using preparation.Models;

namespace preparation.Services.Recommender
{
    public interface IRecommender
    {
        IEnumerable<string> CommentsTo(IProduct product);
        IEnumerable<IProduct> RecommendationsTo(IProduct product);
    }
}