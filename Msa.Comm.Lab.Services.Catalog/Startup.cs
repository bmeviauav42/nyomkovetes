using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Msa.Comm.Lab.Services.Catalog.Data;
using Msa.Comm.Lab.Services.Catalog.Models;

namespace Msa.Comm.Lab.Services.Catalog
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
            // TODO
            services.AddJaeger(currentEnvironment);

            // The simple "built in" EF Core in memory provider outputs less trace info compared to Sqlite, so
            // use Sqlite configured for in-mem instead
            // services.AddnInMem();
            services.AddnMemSqlite();

            //services.AddMvc();
            services.AddControllers();

            services.AddScoped<IProductRepository, ProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Seed data at startup.
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDBContext>();
                dbContext.Seed();
            }


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
