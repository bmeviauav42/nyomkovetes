using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Msa.Comm.Lab.Services.Catalog.Data
{
    public class CatalogDBContext : DbContext
    {
        public CatalogDBContext(DbContextOptions<CatalogDBContext> options)
            : base(options)
        {
        }

        public DbSet<DbProduct> Products { get; set; }

        public void Seed()
        {
            Database.OpenConnection();
            Database.EnsureCreated();
            // Database.Migrate(); // EnsureCreated is mutually exclusive with migrate

            Products.Add(new DbProduct(1, "Sör", 10, 250));
            Products.Add(new DbProduct(2, "Bor", 5, 890));
            Products.Add(new DbProduct(3, "Csoki", 15, 200));

            SaveChanges();
        }
    }

    


}
