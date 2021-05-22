using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Entity
{
    public interface IProduct
    {

    }
    public class Product:IProduct
    {
        [Key]
        public Guid SkuId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsActive { get; set; }

        private string name;
        private int quantity;
        public Product()
        {

        }
        public Product(string name, int quantity)
        {
            this.name = name;
            this.quantity = quantity;
        }
    }
}
