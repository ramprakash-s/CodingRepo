using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngine.Entity
{
    public interface IRuleEngine
    {
        void CreateRules();
        List<RuleEngine> GetRules();

    }
    public class RuleEngine 
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string RuleName { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductName { get; set; }
        public bool IsActive { get; set; }

        public List<RuleEngine> rules { get; set; }

        public RuleEngine()
        {
           
        }
       
    }
}
