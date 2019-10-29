using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msa.Comm.Lab.Services.Order.ApiClients;
using OpenTracing;

namespace Msa.Comm.Lab.Services.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICatalogApiClient catalogApiClient;
        private readonly ILogger<TestController> log;
        // ##Instr_Baggage
        ITracer tracer;

        public TestController(ICatalogApiClient catalogApiClient, ILogger<TestController> log,
            ITracer tracer)  // ##Instr_Baggage         
        {
            this.catalogApiClient = catalogApiClient;
            this.log = log;
            this.tracer = tracer; // ##Instr_Baggage
        }
     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            #region ##Instr_Baggage

            // Simulates user name 
            string userName = "Joe";
            // Simulates request id
            int requestId = 222;

            // Get active span and add data to baggage
            IScope scope = tracer.ScopeManager.Active;
            if (scope != null)
            {
                scope.Span.SetBaggageItem("username", userName);
                scope.Span.SetBaggageItem("requestid", requestId.ToString());
            }

            #endregion


            var res = await catalogApiClient.GetProductsAsync();

            // ##Instr_Log - new log for the active span via injected ILogger (Microsoft.Extensions.Logging)
            int count = res.Count();
            // Use separate parameters to enable structured logging!
            // This is not string interpolation, the params are matched by order
            // Jaeger attaches the following tag to the log (assuming we have 3 products): ProdCount = 3, 
            // where ProdCount is the key, 3 is the value.
            log.LogInformation("Number of products found: {ProdCount}", count);

            return res;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            return await catalogApiClient.GetProductAsync(id);
            // return $"test: {id}";
        }
    }
}
