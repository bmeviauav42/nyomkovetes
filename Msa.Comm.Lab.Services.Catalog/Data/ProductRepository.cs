using Microsoft.EntityFrameworkCore;
using Msa.Comm.Lab.Services.Catalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Catalog.Data
{
    public class ProductRepository: IProductRepository
    {
        CatalogDBContext dbContext;

        public ProductRepository(CatalogDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product> GetProduct(int productId)
        {
            var p = await dbContext.Products.SingleOrDefaultAsync(p => p.ProductId == productId);
            return p == null ? null : new Product(p.ProductId, p.Name, p.UnitPrice, p.Stock);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await dbContext.Products
                .Select(p => new Product(p.ProductId, p.Name, p.UnitPrice, p.Stock))
                .ToListAsync();
        }
    }
}
