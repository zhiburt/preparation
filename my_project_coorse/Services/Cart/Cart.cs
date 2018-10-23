using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using preparation.Models;

namespace preparation.Services.Cart
{
    public class Cart
    {
        private readonly HttpContext _context;
        int iteratorSesssonName;

        public Cart(HttpContext context)
        {
            _context = context;

            foreach (var key in CartKeys())
            {
                iteratorSesssonName++;
            }
        }

        public virtual void AddProduct(IProduct product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            var prodSerializeObject = JsonConvert.SerializeObject(product);
            _context.Session.SetString($"cart_list_{iteratorSesssonName}", prodSerializeObject);
            iteratorSesssonName++;
        }

        public virtual void AddProduct(IProduct[] expectedList)
        {
            if (expectedList == null) throw new ArgumentNullException(nameof(expectedList));

            foreach (var product in expectedList)
            {
                AddProduct(product);
            }
        }

        public virtual IEnumerable<IProduct> All()
        {
            string json = "";
            foreach (var cartKey in CartKeys())
            {
                json = _context.Session.GetString(cartKey);
               yield return JsonConvert.DeserializeObject<Good>(json);
            }
        }

        public virtual decimal TotalPrice()
        {
            decimal total = 0.0m;
            foreach (var product in All())
            {
                total += product.Price;
            }

            return total;
        }

        public virtual void PopLast()
        {
            var lastSessionKey = CartKeys()?.Select(k => int.Parse(k.Split('_')[2])).Max() ?? 0;
            _context.Session.Remove("cart_list_" + lastSessionKey);
        }

        public virtual void Remove(IProduct product)
        {
            string json = "";
            foreach (var cartKey in CartKeys())
            {
                json = _context.Session.GetString(cartKey);
                var jbj = JsonConvert.DeserializeObject<IProduct>(json);
                if (jbj.Equals(product))
                    _context.Session.Remove(cartKey);
            }

        }

        public int AmountProducts()
        {
            return All().Count();
        }

        private IEnumerable<string> CartKeys()
        {
            string pattern = @"^cart_list_\d+";
            Regex regex = new Regex(pattern);

            //LinkedList<string> resp = new LinkedList<string>();
            foreach (var key in _context.Session.Keys)
            {
                var match = regex.IsMatch(key);
                
                if (match)
                {
                    yield return key;
                    //resp.AddLast(new LinkedListNode<string>(key));
                }
            }
        }
    }
}
