using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using preparation.Models;

namespace preparation.Services.Cart
{
    public class Cart : ICart
    {
        int iteratorSesssonName;
        private HttpContext _context;

        public Cart(HttpContext context)
        {
            this._context = context;
            FillIterator();
        }

        //[ActivatorUtilitiesConstructor]
        public Cart()
        {
        }


        public HttpContext Context
        {
            private get { return Context; }
            set
            {
                _context = value;
                FillIterator();
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

        public virtual IEnumerable<IProduct> GetAll()
        {
            string json = "";
            foreach (var cartKey in CartKeys())
            {
                json = _context.Session.GetString(cartKey);
                yield return JsonConvert.DeserializeObject<Good>(json);
            }
        }

        public virtual IEnumerable<IProduct> All()
        {
            string json = "";
            var list = new List<IProduct>();
            foreach (var cartKey in CartKeys())
            {
                json = _context.Session.GetString(cartKey);
                list.Add(JsonConvert.DeserializeObject<Good>(json));
            }

            return list.AsEnumerable();
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
            var keys = CartKeys();
            foreach (var cartKey in keys)
            {
                json = _context.Session.GetString(cartKey);
                var jbj = JsonConvert.DeserializeObject<Good>(json);
                if (jbj.Equals(product))
                {
                    _context.Session.Remove(cartKey);
                    return;
                }
            }

        }

        public int AmountProducts()
        {
            return All().Count();
        }

        private IEnumerable<string> CartKeys()
        {
            Mutex m = new Mutex();
            string pattern = @"^cart_list_\d+";
            Regex regex = new Regex(pattern);
            m.WaitOne();
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
            m.ReleaseMutex();
        }

        private void FillIterator()
        {
            var keys = CartKeys();
            foreach (var key in keys)
            {
                iteratorSesssonName++;
            }
        }

    }
}
