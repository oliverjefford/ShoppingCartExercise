using Microsoft.Extensions.Logging;
using Moq;
using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;
using ShoppingCartExercise.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCartExercise.Test
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void CanAddProduct()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            var mockProductSet = CreateMockDbSet(new List<Product>());
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            var productRepository = new ProductRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            productRepository.Add(product);
            // Assert
            mockProductSet.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once);
            Assert.AreEqual(1, mockProductSet.Object.Count());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDuplicateProductThrowsException()
        {
            // Arrange
            var product = new Product { Barcode = "Banana", Price = 30 };
            var products = new List<Product> { product };
            var mockDbSet = CreateMockDbSet(products);
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Products).Returns(mockDbSet.Object);
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            var productRepository = new ProductRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            productRepository.Add(product);
            // Assert
            // No assert as exception expected
        }
        [TestMethod]
        public void CanRemoveProduct()
        {
            // Arrange
            var products = new List<Product> { new Product { Barcode = "Apple", Price = 50 } };
            var mockProductSet = CreateMockDbSet(products);
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            var productRepository = new ProductRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            productRepository.RemoveProduct("Apple");
            // Assert
            mockProductSet.Verify(r => r.Remove(It.IsAny<Product>()), Times.Once);
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once());
            Assert.AreEqual(0, mockProductSet.Object.Count());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveNonExistingProductThrowsException()
        {
            // Arrange
            var products = new List<Product> { new Product { Barcode = "Banana", Price = 30 } };
            var mockProductSet = CreateMockDbSet(products);
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            var mockLogger = new Mock<ILogger<ProductRepository>>();
            var productRepository = new ProductRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            productRepository.RemoveProduct("Chocolate");
            // Assert
            // No assert as exception expected
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