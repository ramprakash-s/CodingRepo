using Microsoft.Extensions.Logging;
using PromotionEngine.DBRepo;
using PromotionEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Repository
{
    public interface IDbManager
    {
        public List<Product> GetProducts();
        public List<RuleEngine> GetRules();
    }
    public class DbManager:IDbManager
    {
        private readonly ILogger<DbManager> _logger;
        private readonly ApiContext _apiContext;
        public DbManager(ILogger<DbManager> logger, ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        public List<Product> GetProducts()
        {
           return this._apiContext.GetProducts();
        }

        public List<RuleEngine> GetRules()
        {
            return this._apiContext.GetRules();
        }
    }
}
