using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;

namespace ShoppingCartExercise.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public ShoppingCartDatabaseContext DatabaseContext { get; }
        public ILogger<ProductRepository> Logger { get; }

        public ProductRepository(ShoppingCartDatabaseContext databaseContext, ILogger<ProductRepository> logger) {
            DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Product> GetAll()
        {
            return DatabaseContext.Products.ToList();
        }

        public void Add(Product product)
        {
            Product existingProduct = DatabaseContext.Products.FirstOrDefault(p => p.Barcode.ToLower() == product.Barcode.ToLower());
            if (existingProduct != null)
                throw new InvalidOperationException($"Product with barcode '{product.Barcode}' already exists");
            DatabaseContext.Products.Add(product);
            DatabaseContext.SaveChanges();
        }

        public void RemoveProduct(string barcode)
        {
            Product existingProduct = DatabaseContext.Products.FirstOrDefault(p => p.Barcode.ToLower() == barcode.ToLower());
            if (existingProduct == null)
                throw new InvalidOperationException($"No product found for barcode '{barcode}'");
            DatabaseContext.Products.Remove(existingProduct);
            DatabaseContext.SaveChanges();
        }
    }
}
