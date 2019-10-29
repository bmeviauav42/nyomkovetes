using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Catalog.Data
{
    public static class EFServiceCollectionExtensions
    {
        static SqliteConnection connection;

        // Use Sqlite in in memory mode
        // This is just for demo purposes, not for production

        public static IServiceCollection AddnMemSqlite(this IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<CatalogDBContext>(options =>
                {
                    // We have to keep and share use one Sqlite connection for the DbContext instances
                    // as closing the last SqlLite connection would delete the in-mem database.
                    // This approach probably results in transaction conflicts (we don't have any transactions
                    // in the demo however, so this is not an issue). If we had: we could keep one connection
                    // open and create a new connection for each request. But probably that would result in
                    // bad performance.

                    // This is true for the first call only, let's perform initialization then.
                    // This is not thread safe in general. However, as we create the connection at startup
                    // when performing the seed this is not an actual issue.
                    if (connection == null)
                    {
                        var connectionStringBuilder = new SqliteConnectionStringBuilder
                        {
                            DataSource = ":memory:",
                            Mode = SqliteOpenMode.Memory,
                            Cache = SqliteCacheMode.Shared
                        };


                        connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
                        connection.Open();
                        connection.EnableExtensions(true);

                        options.UseSqlite(connection);

                        // Moved to Startup class, Configure method
                        //// This is a bit nasty, but don't really have a better place for it
                        //// We don't have a "truly" typed (generic) options.Options here, work around
                        //// using dynamic
                        //using var dbContext = new CatalogDBContext((dynamic)options.Options);
                        //dbContext.Seed();
                    }
                    else
                        options.UseSqlite(connection);
                });


            return services;
        }

        public static IServiceCollection AddnInMem(this IServiceCollection services)
        {
            services.AddDbContext<CatalogDBContext>(options => options.UseInMemoryDatabase(databaseName: "CatalogDB"));

            return services;
        }
    }
}
