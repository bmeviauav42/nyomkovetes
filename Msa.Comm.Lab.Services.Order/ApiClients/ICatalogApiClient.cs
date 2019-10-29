using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Order.ApiClients
{
    public interface ICatalogApiClient
    {
        [Get("/api/Product")]
        Task<List<Product>> GetProductsAsync();

        [Get("/api/Product/{id}")]
        Task<Product> GetProductAsync(int id);
    }
}
