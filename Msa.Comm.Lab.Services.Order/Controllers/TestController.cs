using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msa.Comm.Lab.Services.Order.ApiClients;

namespace Msa.Comm.Lab.Services.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICatalogApiClient catalogApiClient;
        private readonly ILogger<TestController> log;

        public TestController(ICatalogApiClient catalogApiClient, ILogger<TestController> log)
        {
            this.catalogApiClient = catalogApiClient;
            this.log = log;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var res = await catalogApiClient.GetProductsAsync();
            return res;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"test: {id}";
        }
    }
}
