using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Msa.Comm.Lab.Services.Catalog.Models;
using OpenTracing;

namespace Msa.Comm.Lab.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductRepository productRepository;
        // ##Instr_Log
        private readonly ITracer tracer;

        public ProductController(IProductRepository productRepository, ITracer tracer)  // ##Instr_Log)
        {
            this.productRepository = productRepository;
            this.tracer = tracer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var rand = new Random();
            if (rand.Next() % 5 == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            else
            {
               
                return await productRepository.GetProducts();

            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var rand = new Random();

            // ##Instr_CreateSpan
            // Simulate trying to get item from cache
            // The "GetProductFromCache" is the "operation name" attribute of the span 
            using (IScope scope = tracer.BuildSpan("GetProductFromCache").StartActive(finishSpanOnDispose: true))
            {
                try
                {
                    // Attach tag to span
                    scope.Span.SetTag("ProdId", id);

                    // Simulate cache lookup
                    await Task.Delay(rand.Next(80));

                    // Simulate error for product with ID = 1
                    if (id == 1)
                        throw new Exception("There was an error retrieving item from the product cache");

                    // Simulate cache hit for product with ID = 2
                    if (id == 2)
                    {
                        // New log to the current span (simple event)
                        scope.Span.Log("Item found in cache");

                        // New log with any key-value pairs (the first "event" key is for the standard text description)
                        //scope.Span.Log(
                        //    new Dictionary<string, object>() {
                        //        {"event", "Item found in cache"},
                        //        {"ProdId", id }
                        //    }
                        //    );

                        // Simulate returning found item
                        return new Product(id, "ProdFromCache", 200, 100);
                    }
                    else
                        scope.Span.Log("Item not found in cache");

                }
                catch (Exception ex)
                {
                    // Add the standard "Error" tag to the new span
                    OpenTracing.Tag.Tags.Error.Set(scope.Span, true);
                }

                // No need to call scope.Span.Finish() as we've set finishSpanOnDispose:true in StartActive.
            }

            var product = await productRepository.GetProduct(id);
            if (product == null)
                return NotFound();
            else
                return  product;
        }

    }
}
