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
    public class OfferTests
    {
        [TestMethod]
        public void CanAddOffer()
        {
            // Arrange
            var product = new Product { Barcode = "Apple", Price = 50 };
            var offer = new Offer { Id = 1, OfferType = OfferType.BulkOffer, OfferValue = 120, Quantity = 3, ProductBarcode = "Apple", Product = product };
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockProductSet = CreateMockDbSet(new List<Product> { product });
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            mockDatabaseContext.Setup(r => r.Products).Returns(mockProductSet.Object);
            var mockLogger = new Mock<ILogger<OfferRepository>>();
            var offerRepository = new OfferRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            offerRepository.Add(offer);
            // Assert
            mockOfferSet.Verify(r => r.Add(It.IsAny<Offer>()), Times.Once);
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once);
            Assert.AreEqual(1, mockOfferSet.Object.Count());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddDuplicateOfferThrowsException()
        {
            // Arrange
            var offer = new Offer { Id = 1, OfferType = OfferType.BulkOffer, OfferValue = 120, Quantity = 3, ProductBarcode = "Apple" };
            var offers = new List<Offer> { offer };
            var mockDbSet = CreateMockDbSet(offers);
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockDbSet.Object);
            var mockLogger = new Mock<ILogger<OfferRepository>>();
            var offerRepository = new OfferRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            offerRepository.Add(offer);
            // Assert
            // No assert as exception expected
        }
        [TestMethod]
        public void CanRemoveOffer()
        {
            // Arrange
            var offers = new List<Offer> { new Offer { Id = 1, OfferType = OfferType.BulkOffer, OfferValue = 120, Quantity = 3, ProductBarcode = "Apple" } };
            var mockOfferSet = CreateMockDbSet(offers);
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<OfferRepository>>();
            var offerRepository = new OfferRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            offerRepository.Remove(1);
            // Assert
            mockOfferSet.Verify(r => r.Remove(It.IsAny<Offer>()), Times.Once);
            mockDatabaseContext.Verify(r => r.SaveChanges(), Times.Once());
            Assert.AreEqual(0, mockOfferSet.Object.Count());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveNonExistingOfferThrowsException()
        {
            // Arrange
            var mockOfferSet = CreateMockDbSet(new List<Offer>());
            var mockDatabaseContext = new Mock<ShoppingCartDatabaseContext>();
            mockDatabaseContext.Setup(r => r.Offers).Returns(mockOfferSet.Object);
            var mockLogger = new Mock<ILogger<OfferRepository>>();
            var offerRepository = new OfferRepository(mockDatabaseContext.Object, mockLogger.Object);
            // Act
            offerRepository.Remove(1);
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