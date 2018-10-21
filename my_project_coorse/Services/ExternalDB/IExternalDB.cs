using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace preparation.Services.ExternalDB
{
    public interface IExternalDb
    {
        /// <summary>
        /// Asking foreign Service about p 
        /// </summary>
        /// <param name="p">this is request service</param>
        /// <param name="method">Method HTTP for request</param>
        /// <param name="param">Params Request</param>
        /// <returns>resp once JSON in string</returns>
        Task<string> AskService(string p, HttpMethod m = null, params (string key, string value)[] param);
    }
}
