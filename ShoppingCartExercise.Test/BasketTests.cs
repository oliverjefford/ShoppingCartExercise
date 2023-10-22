using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;
using ShoppingCartExercise.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using ShoppingCartExercise.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoppingCartExercise.Test
{
    [TestClass]
    public class BasketTests
    {
        [TestMethod]
        public void CanAddItemToBasket()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var products = new List<Product> { product };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket>());
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            basketRepository.AddItemToBasket(1, "Apple");
            // Assertr
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once);
            mockBasketSet.Verify(r => r.Add(It.IsAny<Basket>()), Times.Once);
            Assert.AreEqual(1, mockBasketSet.Object.Count());
            Assert.AreEqual(1, mockBasketSet.Object.First().Quantity);
        }
        [TestMethod]
        public void CanAddDuplicateItemToBasket()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var products = new List<Product> { product };
            var basketItem = new Basket { Id = 1, OfferId = null, ProductBarcode = "Apple", Offer = null, Product = product, Quantity = 1 };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket> { basketItem });
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            basketRepository.AddItemToBasket(1, "Apple");
            // Assert
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once);
            Assert.AreEqual(2, mockBasketSet.Object.First().Quantity);
        }
        [TestMethod]
        public void CanRemoveItemFromBasket()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var products = new List<Product> { product };
            var basketItem = new Basket { Id = 1, OfferId = null, ProductBarcode = "Apple", Offer = null, Product = product, Quantity = 1 };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket> { basketItem });
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            basketRepository.RemoveItemFromBasket(1, "Apple");
            // Assert
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once);
            mockBasketSet.Verify(r => r.Remove(It.IsAny<Basket>()), Times.Once);
            Assert.AreEqual(0, mockBasketSet.Object.Count());
        }
        [TestMethod]
        public void CanRemoveDuplicateItemFromBasket()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var products = new List<Product> { product };
            var basketItem = new Basket { Id = 1, OfferId = null, ProductBarcode = "Apple", Offer = null, Product = product, Quantity = 2 };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket> { basketItem });
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            basketRepository.RemoveItemFromBasket(1, "Apple");
            // Assert
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once);
            Assert.AreEqual(1, mockBasketSet.Object.Count());
            Assert.AreEqual(1, mockBasketSet.Object.First().Quantity);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveNonExistingItemFromBasketThrowsException()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var products = new List<Product> { product };
            var basketItem = new Basket { Id = 1, OfferId = null, ProductBarcode = "Apple", Offer = null, Product = product, Quantity = 1 };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket> { basketItem });
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            basketRepository.RemoveItemFromBasket(1, "Banana");
            // Assert
            // No assert as exception expected
        }
        [TestMethod]
        public void CalculatesCorrectTotalWithOffers()
        {
            // Arrange
            var product1 = new Product { Barcode = "Apple", Price = 50 };
            var product2 = new Product { Barcode = "Grapes", Price = 10 };
            var product3 = new Product { Barcode = "Turnip", Price = 75 };
            var products = new List<Product> { product1, product2, product3 };
            var offer1 = new Offer { Id = 1, OfferType = OfferType.BulkOffer, OfferValue = 120, Quantity = 3 };
            var offer2 = new Offer { Id = 2, OfferType = OfferType.BuyOneGetOneFree };
            var basketItem1 = new Basket { Id = 1, Product = product1, Quantity = 7, Offer = offer1 };
            var basketItem2 = new Basket { Id = 1, Product = product2, Quantity = 1 };
            var basketItem3 = new Basket { Id = 1, Product = product3, Quantity = 2, Offer = offer2 };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket> { basketItem1, basketItem2, basketItem3 });
            var mockOfferSet = CreateMockDbSet(new List<Offer> { offer1, offer2 });
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            int calculatedTotal = basketRepository.CalculateTotal(1);
            // Assert
            Assert.AreEqual(375, calculatedTotal);
        }
        [TestMethod]
        public void CalculatesCorrectTotalWithoutOffers()
        {
            // Arrange
            var product1 = new Product { Barcode = "Apple", Price = 50 };
            var product2 = new Product { Barcode = "Grapes", Price = 10 };
            var product3 = new Product { Barcode = "Banana", Price = 30 };
            var products = new List<Product> { product1, product2, product3 };
            var basketItem1 = new Basket { Id = 1, Product = product1, Quantity = 2 };
            var basketItem2 = new Basket { Id = 1, Product = product2, Quantity = 1 };
            var basketItem3 = new Basket { Id = 1, Product = product3, Quantity = 1 };
            var mockProductSet = CreateMockDbSet(products);
            var mockBasketSet = CreateMockDbSet(new List<Basket> { basketItem1, basketItem2, basketItem3 });
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Baskets).Returns(mockBasketSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<BasketRepository>>();
            var basketRepository = new BasketRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            int calculatedTotal = basketRepository.CalculateTotal(1);
            // Assert
            Assert.AreEqual(140, calculatedTotal);
        }
        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(r => r.Add(It.IsAny<T>())).Callback<T>(data.Add);
            mockSet.Setup(r => r.Remove(It.IsAny<T>())).Callback<T>(r => data.Remove(r));
            return mockSet;
        }
    }
}