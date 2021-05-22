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
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ApiContext apiContext;

        public ProductController(ILogger<WeatherForecastController> logger, ApiContext apiContext)
        {
            _logger = logger;
            this.apiContext = apiContext;
        }

        // GET api/values  
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Product> products = apiContext.GetProducts();
            return Ok(products);
        }
    }
}
