using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Msa.Comm.Lab.Services.Catalog.Models;

namespace Msa.Comm.Lab.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
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
            var product = productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return await product;
            }
        }
    }
}
