using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using preparation.Models;

namespace preparation.Services.Streinger
{
    public interface IStreinger
    {
        Task<bool> AddGood(Good good);
        Task<bool> AddSupplier(Supplier s);
        Task<IEnumerable<Good>> Goods();
        Task<IEnumerable<Good>> Goods(string prepName);
        Task<IEnumerable<Preparation>> Preparations();
        Task<Preparation> Preparations(int prepId);
        Task<Preparation> Preparations(string prepName);
        Task<bool> RemoveGood(Good good);
        Task<bool> RemoveSupplier(Supplier s);
        Task<IQueryable<Supplier>> Suppliers();
        Task<Supplier> Suppliers(int supplierId);
        Task<Supplier> Suppliers(string suppName, string suppAddress);
        Task<bool> AddPreparationAsync(Preparation preparation);
    }
}