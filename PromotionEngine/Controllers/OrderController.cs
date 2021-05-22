using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionEngine.DBRepo;
using PromotionEngine.Entity;
using PromotionEngine.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ApiContext apiContext;
        private readonly IRuleEngine rule;
        private readonly Cart orderBase;
        private Discount discount;

        public OrderController(ILogger<OrderController> logger, ApiContext apiContext, IRuleEngine ruleEngine)
        {
            _logger = logger;
            this.apiContext = apiContext;
            this.rule = ruleEngine;
            // create the cart
            this.orderBase = new Cart("Ram");
        }

        // Post api/values  
        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart([Required] List<GenericProduct> genericProducts)
        {
            try { 
            foreach(var item in genericProducts)
            {
                //Validate product quantity
                if(item.Quantity<=0)
                    throw new Exception("Product quantity shoud be greater than zero");

                //Get product from DB
                var product1 = apiContext.GetProducts().FirstOrDefault(x => x.ItemName == item.ProductName && x.IsActive);

                //Validate producnt
                if (product1 == null)
                    throw new Exception("Product not found");

                this.orderBase.AddLineItem(product1, item.Quantity);


                // add discounts 
                var promotion = apiContext.GetRules().FirstOrDefault(x => x.IsActive && x.Title == item.PromotionTitle);
                if(promotion!=null)
                {
                    if(promotion.RuleName== "BuyNoOfItemDiscount")
                    {
                        discount = new BuyNoOfItemDiscount(item.ProductName, Convert.ToInt32(promotion.Value), Convert.ToDecimal(promotion.Price));
                        this.orderBase.AddDiscount(discount);
                    }

                    if (promotion.RuleName == "BuyXGetY")
                    {
                        //var p = apiContext.GetProducts().Where(x => x.IsActive && promotion.Value.Contains(x.ItemName)).ToList();
                        //discount = new BuyXGetYFree(item.ProductName,p,promotion. Convert.ToInt32(promotion.Value), Convert.ToDecimal(promotion.Price));
                        //this.orderBase.AddDiscount(discount);
                    }
                }
            }

               // ProcessCartToOrder(this.orderBase);

            return Ok(this.orderBase.LineItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
                _logger.LogError(ex.Message.ToString());
            }
        }

        private static Order ProcessCartToOrder(Cart cart)
        {
            Order order = new Order(cart.Member);
            foreach (LineItem lineItem in cart.LineItems)
            {
                order.AddLineItem(lineItem.Product, lineItem.Quantity);
                foreach (Discount discount in lineItem.Discounts)
                {
                    order.AddDiscount(discount);
                }
            }
            return order;
        }

        // GET api/values  
        [HttpGet]
        [Route("Rules")]
        public async Task<IActionResult> GetRules()
        {
            List<RuleEngine> rules = apiContext.GetRules();
            return Ok(rules);
        }
    }
}
