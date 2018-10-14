using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using preparation.Models;

namespace preparation.Services.Streinger
{
    public interface IStreinger
    {
        /// <summary>
        /// Return Preparations By Supplier Name and Supplier Address
        /// </summary>
        /// <param name="suppName">Supplier Name</param>
        /// <param name="suppAddress">Supplier Address</param>
        /// <returns>returns Preparations</returns>
        Task<Supplier> Suppliers(string suppName, string suppAddress);

        Task<Preparation> Preparations(string prepName);
        Task<IEnumerable<Preparation>> Preparations();
    }
}