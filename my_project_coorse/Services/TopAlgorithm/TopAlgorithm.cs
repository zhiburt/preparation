using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models;

namespace preparation.Services.TopAlgorithm
{
    public class TopAlgorithm : ITopAlgorithm
    {
        private enum PriorityCompany
        {
            High,
            Partner
        }

        public virtual IEnumerable<IEnumerable<IProduct>> Top(IEnumerable<IEnumerable<IProduct>> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            var responce = new List<IProduct>();

            foreach (var product in products)
            {
                foreach (var good in product)
                {
                    if (IsPartnerProduct(good))
                        responce.Add(good);
                }
            }

            return new [] { responce.AsEnumerable() };
        }

        private bool IsPartnerProduct(IProduct product)
        {
            var partners = new string[]
            {
                "maxim",
                "zhiburt"
            };

            foreach (var partner in partners)
            {
                if (product.Supplier.Company.ToLower().Contains(partner))
                    return true;
            }

            return false;
        }
    }
}
