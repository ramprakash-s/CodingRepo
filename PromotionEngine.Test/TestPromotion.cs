using NUnit.Framework;
using Moq;
using PromotionEngine.Controllers;
using PromotionEngine.Repository;
using PromotionEngine.Entity;
using PromotionEngine.Model;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Test
{
    public class TestPromotion
    {
        private Mock<IDbManager> _IDbManager;
        private Mock<ILogger> _logger;
        private List<RuleEngine> ruleEngines;
        private int Count = 0;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _IDbManager = new Mock<IDbManager>();
            ruleEngines = new List<RuleEngine>();
            ruleEngines.Add(new RuleEngine() { Id=Guid.NewGuid(),IsActive=true, Title="Promotion1", RuleName="BuyXGetY",ProductName="A", Price=100 });
            ruleEngines.Add(new RuleEngine() { Id = Guid.NewGuid(), IsActive = true, Title = "Promotion2", RuleName = "", ProductName = "B", Price = 60 });
            ruleEngines.Add(new RuleEngine() { Id = Guid.NewGuid(), IsActive = true, Title = "Promotion3", RuleName = "", ProductName = "C", Price = 30 });

        }

        [TestCase("Promotion1")]
        [TestCase("Promotion2")]
        public void Test_Promotion(string title)
        {
            var result = ruleEngines.Where(x => x.Title == title).FirstOrDefault();
            Assert.NotNull(result);
        }
    }

}