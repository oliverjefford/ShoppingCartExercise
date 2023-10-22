using ShoppingCartExercise.Enums;

namespace ShoppingCartExercise.Models.DatabaseModels
{
    public class Offer
    {
        public int Id { get; set; }
        public OfferType OfferType { get; set; }
        public int OfferValue { get; set; }
        public string ProductBarcode { get; set; }
        public virtual Product Product { get; set; }
    }
}
