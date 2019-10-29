using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Msa.Comm.Lab.Services.Order.ApiClients;
using Refit;

namespace Msa.Comm.Lab.Services.Order
{
    public class Startup
    {

        // Note: IHostingEnvironment has been introduced in .NET Core, 
        // for previous version use IHostingEnvironment
        IHostEnvironment currentEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            currentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Registers and starts Jaeger (see Shared.JaegerServiceCollectionExtensions)
            // Also registers OpenTracing
            services.AddJaeger(currentEnvironment);

            //services.AddMvc
            services.AddControllers();

            services.AddRefitClient<ICatalogApiClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://msa.comm.lab.services.catalog"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
