using ShoppingCartExercise.Models.DatabaseModels;

namespace ShoppingCartExercise.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        List<Product> GetAll();
        void RemoveProduct(string barcode);
    }
}