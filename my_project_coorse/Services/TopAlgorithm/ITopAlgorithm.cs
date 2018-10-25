using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models;

namespace preparation.Services.TopAlgorithm
{
    public interface ITopAlgorithm
    {
        IEnumerable<IEnumerable<IProduct>> Top(IEnumerable<IEnumerable<IProduct>> products);
    }
}
