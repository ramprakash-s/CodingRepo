using Microsoft.EntityFrameworkCore;
using PromotionEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.DBRepo
{
    public class ApiContext : DbContext,IRuleEngine
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<RuleEngine> Rules { get; set; }

        private IRuleEngine rule;

        public ApiContext(DbContextOptions options) : base(options)
        {
            CreateProducts();
            CreateRules();
        }

        private void CreateProducts()
        {
            Product product = new Product()
            {
                SkuId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ItemName = "A",
                Price = 50,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            Products.Add(product);

            product = new Product()
            {
                SkuId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ItemName = "B",
                Price = 30,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            Products.Add(product);

            product = new Product()
            {
                SkuId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ItemName = "C",
                Price = 20,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            Products.Add(product);

            product = new Product()
            {
                SkuId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                ItemName = "D",
                Price = 15,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            Products.Add(product);
        }

        /// <summary>
        /// Retuns all product details
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            return Products.Local.ToList<Product>();
        }

        public List<RuleEngine> GetRules()
        {
            return Rules.Local.ToList<RuleEngine>();
        }

        public void CreateRules()
        {
            RuleEngine rule = new RuleEngine() { Id = Guid.NewGuid(), Title = "Promotion1", IsActive = true, ProductName = "A", RuleName = "BuyNoOfItemDiscount", Value = "3", Price=130 , CreatedAt = DateTime.UtcNow };
            Rules.Add(rule);
            rule = new RuleEngine() { Id = Guid.NewGuid(), Title="Promotion2", IsActive = true, ProductName = "B", RuleName = "BuyNoOfItemDiscount", Value = "3", Price = 50, CreatedAt = DateTime.UtcNow };
            Rules.Add(rule);
            rule = new RuleEngine() { Id = Guid.NewGuid(), Title = "Promotion3", IsActive = true, ProductName = "C", RuleName = "BuyXGetY", Value = "C,D", Price = 30, CreatedAt = DateTime.UtcNow };
            Rules.Add(rule);
            rule = new RuleEngine() { Id = Guid.NewGuid(), Title = "Promotion4", IsActive = false, ProductName = "E", RuleName = "percentageOff", Value = "5", Price = 0, CreatedAt = DateTime.UtcNow };
            Rules.Add(rule);
        }
        
    }
}
