using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using preparation.Services.ExternalDB;

namespace preparation.Services.Streinger
{
    public class ExternalDb : IExternalDb
    {
        /// <summary>
        /// Asking foreign Service about p 
        /// </summary>
        /// <param name="p">this is request service</param>
        /// <param name="method">Method HTTP for request</param>
        /// <param name="param">Params Request</param>
        /// <returns>resp once JSON in string</returns>
        public async Task<string> AskService(string p, HttpMethod m = null, params (string key, string value)[] param)
        {

            const string baseUrl = "http://127.0.0.1:44321/";
            string url = p + "?";
            if ( (m == HttpMethod.Get || m == HttpMethod.Delete || m == null) &&
                 param != null)
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
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(20), BaseAddress = new Uri(baseUrl) })
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
    }
}