﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using preparation.Models;

namespace preparation.Services.Streinger
{

    //TODO streinger need constructor DB +- for MOK better testing
    public class Streinger : IStreinger
    {
        public async Task<Supplier> Suppliers(string suppName, string suppAddress)
        {
            var strJson = await AskService($"suppliers/find/byAddressAndName", null, new []
            {
                ("address", suppAddress),
                ("name", suppName)
            });

            return Deserialize<Supplier>(strJson);
        }


        public async Task<Supplier> Suppliers(int supplierId)
        {
            var strJson = await AskService($"suppliers/find/byId", HttpMethod.Get, new []
            {
                ("id", supplierId.ToString())
            });
            var supplier = Deserialize<Supplier>(strJson);

            return supplier;
        }

        public async Task<bool> AddSupplier(Supplier s)
        {
            var strJson =
                await AskService(
                    $"suppliers/new", HttpMethod.Put, new []
                    {
                        ("name", s.Name),
                        ("company", s.Company),
                        ("address", s.Address),
                        ("description", s.Description),
                        ("geolocation", s.Geolocation)
                    });

            return !CheckErrorInJSON(strJson);
        }

        public async Task<bool> RemoveSupplier(Supplier s)
        {
            var strJson =
                await AskService(
                    $"suppliers/delete", HttpMethod.Delete, new[]
                    {
                        ("name", s.Name),
                        ("company", s.Company),
                        ("address", s.Address),
                        ("description", s.Description),
                        ("geolocation", s.Geolocation)
                    });

            return !CheckErrorInJSON(strJson);
        }

        public async Task<Preparation> Preparations(string prepName)
        {
            var strJson = await AskService($"preparations/find/byName", HttpMethod.Get, new[]
            {
                ("name", prepName)
            });
            var preparation = Deserialize<Preparation>(strJson);

            return preparation;

        }


        private async Task<Preparation> Preparations(int prepId)
        {
            var strJson = await AskService($"preparations/find/byId?id={prepId}");
            var preparation = Deserialize<Preparation>(strJson);

            return preparation;
        }


        /// <summary>
        /// Get All preparations from somewhere :)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Preparation>> Preparations()
        {
            var strJson = await AskService("preparations");
            var preparations = Deserialize<IEnumerable<Preparation>>(strJson);

            return preparations;
        }

        public async Task<IEnumerable<Good>> Goods(string prepName)
        {
            var prep = await Preparations(prepName);
            if (prep == null)
            {
                return null;
            }

            var strJson = await AskService($"goods/preparations", HttpMethod.Get, new []
            {
                ("preparation_id", prep.Id.ToString())
            });
            var goods = await GetGoods(strJson);

            return goods;
        }


        public async Task<bool> AddGood(Good good)
        {
            if (good == null || (good.Supplier == null || good.Product == null )) throw new ArgumentNullException(nameof(good));

            var prep = await Preparations(good.Product.Name);
            var supp = await Suppliers(good.Supplier.Name, good.Supplier.Address);
            if (prep == null || supp == null)
            {
                return false;
            }

            var strJson = await AskService($"goods/new", HttpMethod.Put, new[]
            {
                ("preparation_id", prep.Id.ToString()),
                ("supplier_id", supp.Id.ToString()),
                ("price", good.Price.ToString(CultureInfo.InvariantCulture))
            });

            return !CheckErrorInJSON(strJson);
        }

        public async Task<bool> RemoveGood(Good good)
        {
            if (good == null || (good.Supplier == null || good.Product == null)) throw new ArgumentNullException(nameof(good));

            var prep = await Preparations(good.Product.Name);
            var supp = await Suppliers(good.Supplier.Name, good.Supplier.Address);
            if (prep == null || supp == null)
            {
                return false;
            }

            var strJson = await AskService($"goods/delete", HttpMethod.Delete, new[]
            {
                ("preparation_id", prep.Id.ToString()),
                ("supplier_id", supp.Id.ToString()),
                ("price", good.Price.ToString(CultureInfo.InvariantCulture))
            });

            return !CheckErrorInJSON(strJson);
        }

        /// <summary>
        /// Asking foreign Service about p 
        /// </summary>
        /// <param name="p">this is request service</param>
        /// <param name="method">Method HTTP for request</param>
        /// <param name="param">Params Request</param>
        /// <returns>resp once JSON in string</returns>
        private async Task<string> AskService(string p, HttpMethod m = null, params (string key, string value)[] param)
        {

            const string baseUrl = "http://127.0.0.1:44321/";
            string url = p + "?";
            if (m == HttpMethod.Get || m== HttpMethod.Delete || m == null)
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach ((string key, string value) valueTuple in param)
                {
                    query.Add(valueTuple.key, valueTuple.value);
                }

                url += query.ToString();
            }

            HttpResponseMessage r = null;
            HttpMethod method = m ?? HttpMethod.Get;
            using (var client = new HttpClient {Timeout = TimeSpan.FromSeconds(20), BaseAddress = new Uri(baseUrl) })
            using (HttpRequestMessage req = new HttpRequestMessage(method, url))
            {
                if (method == HttpMethod.Put || method == HttpMethod.Post)
                {
                    var bodyParams = param?.Select((v) => new KeyValuePair<string, string>(v.key, v.value));
                    req.Content = new FormUrlEncodedContent(bodyParams);
                }
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                r = await client.SendAsync(req);

            }

            var resp = await r.Content.ReadAsStringAsync();
            Debug.WriteLine(resp);
            return resp;

        }

        private T Deserialize<T>(string json)
            where T : class
        {
            Debug.WriteLine(json);

            if (CheckErrorInJSON(json))
            {
                return null;
            }

            const string responceKey = "data";
            var obj = JsonConvert.DeserializeObject<Dictionary<string, T>>(json,
                settings: new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

            return obj.ContainsKey(responceKey) ? obj[responceKey] : null;
        }

        private bool CheckErrorInJSON(string json)
        {
            const string errorKey = "error";
            var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(json,
                settings: new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

            return map.ContainsKey(errorKey);
        }


        private async Task<IEnumerable<Good>> GetGoods(string json)
        {
            var resp = Deserialize<IEnumerable<GoodsServiceResp>>(json);
            if (resp == null)
                return null;

            var goods = new List<Good>(resp.Count());
            foreach (var goodsServiceResp in resp)
            {
                Supplier supplier = await Suppliers(goodsServiceResp.SupplierId);
                Preparation prep = await Preparations(goodsServiceResp.PreparationId);
                goods.Add( new Good { Price = goodsServiceResp.Price, Product = prep, Supplier = supplier } );
            }

            return goods;
        }

        private class GoodsServiceResp
        {
            public int PreparationId { get; set; }
            public int SupplierId { get; set; }
            public decimal Price { get; set; }
        }

        /*
*   string serviceUrl = @"preparations";
string url = "http://127.0.0.1:44321/preparations";

var cnt = HttpClientFactory.Create();
await cnt.GetStringAsync(url);

using (var client = new HttpClient
   { Timeout = TimeSpan.FromSeconds(20) })
using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get,
   url))
{

   client.DefaultRequestHeaders.Accept.Clear();
   client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

   try
   {


   var r = await client.SendAsync(req);
   var resp = await r.Content.ReadAsStringAsync();
   var preparations =
       JsonConvert.DeserializeObject<IEnumerable<Preparation>>(resp);
       return preparations;

   }
   catch (Exception ex)
   {
       Console.WriteLine((ex.Message));
   }

   return null;

*/

    }


}