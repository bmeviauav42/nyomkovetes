using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Catalog.Data
{
    // Product as stored in the database by EF
    [Table("Product")]
    public class DbProduct
    {
        public DbProduct() { }

        public DbProduct(int productId, string name, decimal unitPrice, int stock)
        {
            ProductId = productId;
            Name = name;
            UnitPrice = unitPrice;
            Stock = stock;
        }

        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
}
