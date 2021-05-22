using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionEngine.DBRepo;
using PromotionEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ApiContext apiContext;
        private readonly IRuleEngine rule;

        public ProductController(ILogger<ProductController> logger, ApiContext apiContext, IRuleEngine ruleEngine)
        {
            _logger = logger;
            this.apiContext = apiContext;
            this.rule = ruleEngine;
        }

        // GET api/values  
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> Get()
        {
            List<Product> products = apiContext.GetProducts();
            return Ok(products);
        }

        // GET api/values  
        [HttpGet]
        [Route("GetRules")]
        public async Task<IActionResult> GetRules()
        {
            List<RuleEngine> rules = apiContext.GetRules();
            return Ok(rules);
        }
    }
}
