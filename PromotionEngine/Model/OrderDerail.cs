using PromotionEngine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Model
{
    public interface IOrderDetail
    {
        public OrderDetail GetOrderDerails(LineItem lineItem);
    }
    public class OrderDetail : IOrderDetail
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string PromotionTitle { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal SubTotalAmount { get; set; }
        
        public OrderDetail GetOrderDerails(LineItem lineItem)
        {

            return new OrderDetail()
            {
                ProductName = lineItem.Product.ItemName,
                Quantity = lineItem.Quantity,
                Price = lineItem.Product.Price,
                SubTotalAmount = lineItem.Subtotal,
                DiscountAmount = (lineItem.Product.Price * lineItem.Quantity) - lineItem.Subtotal
                //PromotionTitle=lineItem.Discounts.Where(x=>x.Name==lineItem.Product.ItemName).Select(x=>x.)

            };
        }

    }
    public class OrderResponse
    {
        public List<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
        private decimal _grossTotal { get; set; }
        private decimal _totalDiscount { get; set; }
        public decimal GrossTotal { get { _grossTotal = OrderDetail.Select(x => x.SubTotalAmount).Sum(); return _grossTotal; } }
        public decimal TotalDiscount { get { _totalDiscount = OrderDetail.Select(x => x.DiscountAmount).Sum(); return _totalDiscount; } }
    }
}
