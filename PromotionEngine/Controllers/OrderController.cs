using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionEngine.DBRepo;
using PromotionEngine.Entity;
using PromotionEngine.Model;
using PromotionEngine.Repository;
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
        #region Private
        private readonly ILogger<OrderController> _logger;
        private readonly IDbManager _dbManager;
        private readonly Cart orderBase;
        private Discount discount;
        private IOrderDetail orderDetail;

        private Order ProcessCartToOrder(List<RuleEngine> orderRequest)
        {
            var cart = this.orderBase;
            foreach (var offer in orderRequest)
            {
                // add discounts 
                var promotion = this._dbManager.GetRules().FirstOrDefault(x => x.IsActive && x.Title == offer.Title);
                if (promotion != null)
                {
                    //if (promotion.RuleName == "BuyNoOfItemDiscount")
                    //{
                    //    var p = apiContext.GetProducts().Where(x => x.IsActive && promotion.Value.Contains(x.ItemName)).ToList();
                    //    discount = new BuyNoOfItemDiscount(promotion.Title, p, Convert.ToInt32(promotion.Value), Convert.ToDecimal(promotion.Price));
                    //    this.orderBase.AddDiscount(discount);
                    //}

                    if (promotion.RuleName == "BuyXGetY")
                    {
                        var p = this._dbManager.GetProducts().Where(x => x.IsActive && promotion.Value.Contains(x.ItemName)).ToList();

                        var items = cart.LineItems.Where(x => promotion.Value.Contains(x.Product.ItemName)).ToList();


                        discount = new BuyXGetYFree(promotion.Title, p, Convert.ToInt32(items.Take(1).Select(x => x.Quantity)), Convert.ToInt32(items.Skip(1).Take(1).Select(x => x.Quantity)), Convert.ToDecimal(promotion.Price));
                        this.orderBase.AddDiscount(discount);
                    }
                }
            }

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
        #endregion

        /// <summary>
        /// Order constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="detail"></param>
        /// <param name="dbManager"></param>
        public OrderController(ILogger<OrderController> logger, IOrderDetail detail, IDbManager dbManager)
        {
            _logger = logger;
            this._dbManager = dbManager;
            // create the cart
            this.orderBase = new Cart("Ram");//Convert.ToString(HttpContext.User.Identity.Name));
            orderDetail = detail;
        }

        #region "API"
        // Post api/values  
        [HttpPost]
        [Route("AddToCart")]
        public async Task<IActionResult> AddToCart([Required] List<GenericProduct> genericProducts)
        {
            
            try {

                if (!genericProducts.Any())
                    throw new Exception("Please provide product details");


                foreach (var item in genericProducts)
                {
                    //Validate product quantity
                    if (item.Quantity <= 0)
                        throw new Exception("Product quantity shoud be greater than zero");

                    //Get product from DB
                    var product1 = this._dbManager.GetProducts().FirstOrDefault(x => x.ItemName == item.ProductName && x.IsActive);

                    //Validate producnt
                    if (product1 == null)
                        throw new Exception("Product not found");

                    this.orderBase.AddLineItem(product1, item.Quantity);


                    // add discounts 
                    var promotion = this._dbManager.GetRules().FirstOrDefault(x => x.IsActive && x.Title == item.PromotionTitle);
                    if (promotion != null)
                    {
                        if (promotion.RuleName == "BuyNoOfItemDiscount")
                        {
                            List<Product> products = new List<Product>(); products.Add(product1);
                            discount = new BuyNoOfItemDiscount(item.PromotionTitle, products, Convert.ToInt32(promotion.Value), Convert.ToDecimal(promotion.Price));
                            this.orderBase.AddDiscount(discount);
                        }

                        //    if (promotion.RuleName == "BuyXGetY")
                        //    {
                        //        //var p = apiContext.GetProducts().Where(x => x.IsActive && promotion.Value.Contains(x.ItemName)).ToList();
                        //        //discount = new BuyXGetYFree(item.ProductName,p,promotion. Convert.ToInt32(promotion.Value), Convert.ToDecimal(promotion.Price));
                        //        //this.orderBase.AddDiscount(discount);
                        //    }
                    }
                }

                //process Order
                List<RuleEngine> rules = this._dbManager.GetRules().Where(x => genericProducts.Select(x => x.PromotionTitle).ToList().Contains(x.Title)).ToList();
               var order= ProcessCartToOrder(rules);


                // Generate Response
                List<OrderDetail> orderDerails = new List<OrderDetail>();
                orderBase.LineItems.ToList().ForEach(x => orderDerails.Add(orderDetail.GetOrderDerails(x)));
                OrderResponse orderResponse = new OrderResponse() { OrderDetail = orderDerails };
                
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return BadRequest(ex.Message.ToString());                
            }
        }

        //// GET api/values  
        //[HttpPost]
        //[Route("ProcessCartOrder")]
        //public async Task<IActionResult> ProcessCartOrder(ProcessOrderRequest OrderRequest)
        //{
        //    try
        //    {
        //        var promotion = apiContext.GetRules().FirstOrDefault(x => x.IsActive && OrderRequest.PromotionName.Contains(offer.Title));

        //        return Ok(ProcessCartToOrder(OrderRequest));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message.ToString());
        //        return BadRequest(ex.Message.ToString());
        //    }

        //}

        #endregion
        
    }
}
