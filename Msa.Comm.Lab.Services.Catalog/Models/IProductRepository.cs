using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Catalog.Models
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProduct(int productId);
    }
}
