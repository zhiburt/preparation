using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using Newtonsoft.Json;
using preparation.Models;

namespace preparation.Services.Streinger
{
    public class Streinger : IStreinger
    {
        public Task<IEnumerable<Preparation>> Suppliers(string suppName, string suppAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<Preparation> Preparations(string prepName)
        {
            var strJson = await AskService($"preparations/find/byName?name={prepName}");
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

        /// <summary>
        /// Asking foreign Service about p 
        /// </summary>
        /// <param name="p">this is request service</param>
        /// <param name="method">Method HTTP for request</param>
        /// <returns>resp once JSON in string</returns>
        private async Task<string> AskService(string p, HttpMethod m = null)
        {
            const string baseUrl = "http://127.0.0.1:44321/";
            string url = baseUrl + p;

            HttpResponseMessage r = null;
            HttpMethod method = m ?? HttpMethod.Get;
            using (var client = new HttpClient{ Timeout = TimeSpan.FromSeconds(20), BaseAddress =new Uri( url) })
            using (HttpRequestMessage req = new HttpRequestMessage(method, url))
            {
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
            var obj = JsonConvert.DeserializeObject<Dictionary<string,T>>(json, 
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
