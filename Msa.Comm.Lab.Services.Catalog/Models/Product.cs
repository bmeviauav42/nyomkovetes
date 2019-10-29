using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msa.Comm.Lab.Services.Catalog.Models
{
    // The domain product class
    public class Product
    {
        public Product(int productId, string name, decimal unitPrice, int stock)
        {
            ProductId = productId;
            Name = name;
            UnitPrice = unitPrice;
            Stock = stock;
        }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
}
