using Microsoft.EntityFrameworkCore;
using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Enums;
using ShoppingCartExercise.Models.DatabaseModels;
using System.Text;

namespace ShoppingCartExercise.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public ShoppingCartDatabaseContext DatabaseContext { get; }
        public ILogger<BasketRepository> Logger { get; }

        public BasketRepository(ShoppingCartDatabaseContext databaseContext, ILogger<BasketRepository> logger)
        {
            DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Basket> GetBaskets()
        {
            return DatabaseContext.Baskets.Include(b => b.Product).Include(b => b.Offer).ToList();
        }
        public List<Basket> GetBasket(int basketId)
        {
            return DatabaseContext.Baskets.Where(bi => bi.Id == basketId).Include(b => b.Product).Include(b => b.Offer).ToList();
        }
        public void AddItemToBasket(int basketId, string barcode)
        {
            Product product = DatabaseContext.Products.FirstOrDefault(p => p.Barcode == barcode);
            if (product == null)
                throw new InvalidOperationException($"Product does not exist for barcode '{barcode}'");
            List<Basket> basketItems = DatabaseContext.Baskets.Where(b => b.Id == basketId).ToList();
            List<Basket> duplicateProducts = basketItems.Where(bi => bi.ProductBarcode == barcode).ToList();
            if (!duplicateProducts.Any())
            {
                Offer offer = DatabaseContext.Offers.FirstOrDefault(o => o.ProductBarcode == barcode);
                Basket basketItem = new Basket
                {
                    Id = basketId,
                    ProductBarcode = barcode,
                    Product = product,
                    OfferId = offer?.Id,
                    Offer = offer
                };
                DatabaseContext.Baskets.Add(basketItem);
            }
            else
                basketItems.First().Quantity++;
            DatabaseContext.SaveChanges();
        }
        public void RemoveItemFromBasket(int basketId, string barcode)
        {
            Product product = DatabaseContext.Products.FirstOrDefault(p => p.Barcode == barcode);
            if (product == null)
                throw new InvalidOperationException($"Product with barcode '{barcode}' does not exist");
            List<Basket> basketItems = DatabaseContext.Baskets.Where(bi => bi.Id == basketId).ToList();
            if (!basketItems.Any(bi => bi.ProductBarcode == barcode))
                throw new InvalidOperationException($"Product with barcode '{barcode}' is not in the basket");
            Basket basketItem = basketItems.First(bi => bi.ProductBarcode == barcode);
            if (basketItem.Quantity > 1)
                basketItem.Quantity--;
            else
                DatabaseContext.Baskets.Remove(basketItem);
            DatabaseContext.SaveChanges();
        }
        public int CalculateTotal(int basketId)
        {
            int total = 0;
            List<Basket> basketItems = GetBasket(basketId);
            if (!basketItems.Any())
                throw new InvalidOperationException($"No basket exists for ID '{basketId}'");
            var basketItemsNoOffers = basketItems.Where(bi => bi.Offer == null).ToList();
            foreach (var item in basketItemsNoOffers)
                total += (item.Product.Price * item.Quantity);
            var basketItemsWithOffers = basketItems.Where(bi => bi.Offer != null).ToList();
            foreach (var item in basketItemsWithOffers)
            {
                switch (item.Offer.OfferType)
                {
                    case OfferType.BuyOneGetOneFree:
                        int freeItemsCount = item.Quantity / 2;
                        total += item.Product.Price * (item.Quantity - freeItemsCount);
                        break;
                    case OfferType.BulkOffer:
                        int remainder = item.Quantity % item.Offer.Quantity;
                        total += remainder * item.Product.Price;
                        int applyableOfferCount = item.Quantity - remainder;
                        total += ((applyableOfferCount / item.Offer.Quantity) * item.Offer.OfferValue);
                        break;
                }
            }
            return total;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
    }
}
