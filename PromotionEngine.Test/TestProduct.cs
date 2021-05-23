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
    public class TestProduct
    {
        private Mock<IDbManager> _IDbManager;
        private Mock<ILogger> _logger;
        private List<Product> _Products;
        private int Count = 0;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _IDbManager = new Mock<IDbManager>();
            _Products = new List<Product>();
            _Products.Add(new Product() { ItemName = "A", IsActive = true, Price = 150, SkuId = Guid.NewGuid() });
            _Products.Add(new Product() { ItemName = "B", IsActive = true, Price = 100, SkuId = Guid.NewGuid() });
            _Products.Add(new Product() { ItemName = "C", IsActive = true, Price = 50, SkuId = Guid.NewGuid() });
        }

        [TestCase("A")]
        [TestCase("B")]
        public void Test_Produncts(string name)
        {
            var result = _Products.Where(x => x.ItemName == name).FirstOrDefault();
            Assert.NotNull(result);
        }
    }

}