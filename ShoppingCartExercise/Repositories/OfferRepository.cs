using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;

namespace ShoppingCartExercise.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        public ShoppingCartDatabaseContext DatabaseContext { get; }
        public ILogger<OfferRepository> Logger { get; }

        public OfferRepository(ShoppingCartDatabaseContext databaseContext, ILogger<OfferRepository> logger)
        {
            DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Offer> GetAll()
        {
            return DatabaseContext.Offers.ToList();
        }

        public void Add(Offer offer)
        {
            Offer existingOffer = DatabaseContext.Offers.FirstOrDefault(o => o.OfferType == offer.OfferType
                                                                          && o.OfferValue == offer.OfferValue
                                                                          && o.ProductBarcode == offer.ProductBarcode);
            if (existingOffer != null)
                throw new InvalidOperationException($"Offer for product '{offer.ProductBarcode}' already exists");
            Product product = DatabaseContext.Products.FirstOrDefault(p => p.Barcode == offer.ProductBarcode);
            if (product == null)
                throw new InvalidOperationException($"Product does not exist for barcode '{offer.ProductBarcode}'");
            offer.Product = product;
            DatabaseContext.Offers.Add(offer);
            DatabaseContext.SaveChanges();
        }

        public void Remove(int id)
        {
            Offer existingOffer = DatabaseContext.Offers.FirstOrDefault(o => o.Id == id);
            if (existingOffer == null)
                throw new InvalidOperationException($"Offer does not exist for ID '{id}'");
            DatabaseContext.Offers.Remove(existingOffer);
            DatabaseContext.SaveChanges();
        }
    }
}
