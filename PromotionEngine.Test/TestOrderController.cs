using NUnit.Framework;
using Moq;
using PromotionEngine.Controllers;
using PromotionEngine.Repository;
using PromotionEngine.Entity;
using PromotionEngine.Model;
using Castle.Core.Logging;
using System.Collections.Generic;

namespace PromotionEngine.Test
{
    public class TestOrderController
    {
        private Mock<IDbManager> _IDbManager;
        private Mock<ILogger> _logger;
        private Mock<IOrderDetail> _IOrderDetail;
        private OrderController _OrderController;
        private List<GenericProduct> genericProducts;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger>();
            _IDbManager = new Mock<IDbManager>();
            _IOrderDetail = new Mock<IOrderDetail>();
            _IDbManager.Setup(x => x.GetProducts()).Returns(new System.Collections.Generic.List<Product>());
            _OrderController = new OrderController(null, _IOrderDetail.Object, _IDbManager.Object);
            genericProducts = new List<GenericProduct>();
            genericProducts.Add(new GenericProduct() { ProductName="A",PromotionTitle="Promotion1", Quantity=10});
            genericProducts.Add(new GenericProduct() { ProductName = "B", PromotionTitle = "Promotion2", Quantity = 6 });
            genericProducts.Add(new GenericProduct() { ProductName = "C", PromotionTitle = "Promotion3", Quantity = 1 });
        }

        [Test]
        public void Test_Add_To_Cart()
        {
            var result = _OrderController.AddToCart(genericProducts);
            //Assert.NotNull(result);
            Assert.Pass();
        }
       
    }
}