using Microsoft.EntityFrameworkCore;
using PromotionEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.DBRepo
{
    public class ApiContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ApiContext(DbContextOptions options) : base(options)
        {
            LoadProducts();
        }
        private void LoadProducts()
        {
        }

        /// <summary>
        /// Retuns all product details
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            return Products.Local.ToList<Product>();
        }
    }
}
