using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Entity
{
    [Keyless]
    [NotMapped]
    [Serializable]
    public abstract class Discount
    {
        protected internal Discount()
        {
        }

        public Discount(string name)
        {
            Name = name;
        }

        public abstract OrderBase ApplyDiscount();
        public virtual OrderBase OrderBase { get; set; }
        public virtual string Name { get; private set; }
    }
    [Serializable]
    public class BuyNoOfItemDiscount : Discount
    {
        protected internal BuyNoOfItemDiscount()
        {
        }

        public BuyNoOfItemDiscount(string name, IList<Product> applicableProducts, int maxCount, decimal fixedPrice)
            : base(name)
        {
            MaxCount = maxCount;
            Name = name;
            FixedPrice = fixedPrice;
            ApplicableProducts = applicableProducts;
        }

        public override OrderBase ApplyDiscount()
        {
            // custom processing
            foreach (LineItem lineItem in OrderBase.LineItems)
            {
                if (ApplicableProducts.Contains(lineItem.Product))
                {
                    if (lineItem.Quantity >= MaxCount)
                    {                        
                        var discountPrice = (lineItem.Quantity / MaxCount) * FixedPrice;
                        var remainPrice = (lineItem.Quantity % MaxCount) * lineItem.Product.Price;

                        lineItem.DiscountAmount = (lineItem.Product.Price * lineItem.Quantity ) -  (discountPrice + remainPrice);
                        lineItem.AddDiscount(this);
                    }
                }
            }
            return OrderBase;
        }

        public virtual int MaxCount { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal FixedPrice { get; set; }
        public virtual IList<Product> ApplicableProducts { get; set; }

    }

    [Serializable]
    public class BuyXGetYFree : Discount
    {
        protected internal BuyXGetYFree()
        {
        }

        public BuyXGetYFree(string name, IList<Product> applicableProducts, int x, int y, decimal fixedPrice)
            : base(name)
        {
            ApplicableProducts = applicableProducts;
            X = x;
            Y = y;
            FixedPrice = fixedPrice;
        }

        public override OrderBase ApplyDiscount()
        {
            decimal price = 0;
            // custom processing
            foreach (LineItem lineItem in OrderBase.LineItems)
            {
                if (ApplicableProducts.Contains(lineItem.Product))
                {
                    
                    if (X > Y)
                    {
                        X = X - Y;
                        price += (X * lineItem.Product.Price) + (Y * FixedPrice);
                    } 
                    else if(Y>X)
                    {
                        Y = Y - X;
                        price += (Y * lineItem.Product.Price) + (X * FixedPrice);
                    }
                    else
                    {
                        price += (X * FixedPrice);
                    }
                    lineItem.DiscountAmount += (lineItem.Product.Price * lineItem.Quantity) - price; // ((lineItem.Quantity / X) * Y) * lineItem.Product.Price;
                    lineItem.AddDiscount(this);
                }
            }
            return OrderBase;
        }

        public virtual IList<Product> ApplicableProducts { get; set; }
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual decimal FixedPrice { get; set; }
    }

    [Serializable]
    public class PercentageOffDiscount : Discount
    {
        protected internal PercentageOffDiscount()
        {
        }

        public PercentageOffDiscount(string name, decimal discountPercentage)
            : base(name)
        {
            DiscountPercentage = discountPercentage;
        }

        public override OrderBase ApplyDiscount()
        {
            // custom processing
            foreach (LineItem lineItem in OrderBase.LineItems)
            {
                lineItem.DiscountAmount = lineItem.Product.Price * DiscountPercentage;
                lineItem.AddDiscount(this);
            }
            return OrderBase;
        }

        public virtual decimal DiscountPercentage { get; set; }
    }

    [Serializable]
    public class SpendMoreThanXGetYDiscount : Discount
    {
        protected internal SpendMoreThanXGetYDiscount()
        {
        }

        public SpendMoreThanXGetYDiscount(string name, decimal threshold, decimal discountPercentage)
            : base(name)
        {
            Threshold = threshold;
            DiscountPercentage = discountPercentage;
        }

        public override OrderBase ApplyDiscount()
        {
            // if the total for the cart/order is more than x apply discount
            if (OrderBase.GrossTotal > Threshold)
            {
                // custom processing
                foreach (LineItem lineItem in OrderBase.LineItems)
                {
                    lineItem.DiscountAmount += lineItem.Product.Price * DiscountPercentage;
                    lineItem.AddDiscount(this);
                }
            }
            return OrderBase;
        }

        public virtual decimal Threshold { get; set; }
        public virtual decimal DiscountPercentage { get; set; }
    }
}
