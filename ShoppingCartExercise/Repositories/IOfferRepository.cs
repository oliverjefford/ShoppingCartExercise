using ShoppingCartExercise.Models.DatabaseModels;

namespace ShoppingCartExercise.Repositories
{
    public interface IOfferRepository
    {
        void Add(Offer offer);
        List<Offer> GetAll();
        void Remove(int id);
    }
}