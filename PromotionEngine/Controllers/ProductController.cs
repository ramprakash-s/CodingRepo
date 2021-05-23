using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionEngine.DBRepo;
using PromotionEngine.Entity;
using PromotionEngine.Repository;
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
        #region private
        private readonly ILogger<ProductController> _logger;
        private readonly IDbManager _dbManager;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbManager"></param>
        public ProductController(ILogger<ProductController> logger, IDbManager dbManager)
        {
            _logger = logger;
            this._dbManager = dbManager;
            
        }

        #region api
        // GET api/values  
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Product> products = this._dbManager.GetProducts();
                return Ok(products);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return BadRequest(ex.Message.ToString());
            }
        }

        // GET api/values  
        [HttpGet]
        [Route("GetRules")]
        public async Task<IActionResult> GetRules()
        {
            try
            {
                List<RuleEngine> rules = this._dbManager.GetRules();
                return Ok(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return BadRequest(ex.Message.ToString());
            }
        }
        #endregion
    }
}
