using NUnit.Framework;
using Moq;
using PromotionEngine.Controllers;
using PromotionEngine.Repository;
using PromotionEngine.Entity;
using PromotionEngine.Model;
using Castle.Core.Logging;

namespace PromotionEngine.Test
{
    public class TestProdunctContoller
    {
        private Mock<IDbManager> _IDbManager;
        private Mock<ILogger> _logger;
        private ProductController _ProductController;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _IDbManager = new Mock<IDbManager>();
            _IDbManager.Setup(x => x.GetProducts()).Returns(new System.Collections.Generic.List<Product>());
             _ProductController = new ProductController(null, _IDbManager.Object);
        }

        [Test]
        public void Test_Produncts()
        {
         var result=   _ProductController.Get().Result;
           
            Assert.Pass();
        }

        [Test]
        public void Test_Promotions()
        {
            var result = _ProductController.GetRules();

            Assert.Pass();
        }
    }
}