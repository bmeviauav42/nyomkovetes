using Microsoft.EntityFrameworkCore;
using Msa.Comm.Lab.Services.Catalog.Models;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Catalog.Data
{
    public class ProductRepository: IProductRepository
    {
        CatalogDBContext dbContext;
        // ##Instr_Baggage
        ITracer tracer;

        public ProductRepository(CatalogDBContext dbContext,
            ITracer tracer)  // ##Instr_Baggage  )
        {
            this.dbContext = dbContext;
            this.tracer = tracer; // ##Instr_Baggage
        }

        public async Task<Product> GetProduct(int productId)
        {
            var p = await dbContext.Products.SingleOrDefaultAsync(p => p.ProductId == productId);
            return p == null ? null : new Product(p.ProductId, p.Name, p.UnitPrice, p.Stock);
        }

        public async Task<List<Product>> GetProducts()
        {
            #region ##Instr_Baggage

            // Get active span and add data to baggage
            IScope scope = tracer.ScopeManager.Active;
            if (scope != null)
            {
                var userName = scope.Span.GetBaggageItem("username");
                var requestId = scope.Span.GetBaggageItem("requestid");
                scope.Span.Log(
                            new Dictionary<string, object>() 
                            {
                                {"event", "ProductRepository.GetProducts is executed"},
                                {"username", userName },
                                {"requestId", requestId }
                            }
                    );
            }

            #endregion
            return await dbContext.Products
                .Select(p => new Product(p.ProductId, p.Name, p.UnitPrice, p.Stock))
                .ToListAsync();
        }
    }
}
