using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Entity
{
    [Serializable]
    public class Cart: OrderBase
    {
        protected internal Cart()
        {
        }

        public Cart(string member)
            : base(member)
        {
        }
    }
}
