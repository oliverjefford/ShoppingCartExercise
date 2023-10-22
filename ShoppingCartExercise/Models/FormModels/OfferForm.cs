using ShoppingCartExercise.Enums;

namespace ShoppingCartExercise.Models.FormModels
{
    public class OfferForm
    {
        public OfferType OfferType { get; set; }
        public int OfferValue { get; set; }
        public string ProductBarcode { get; set; }
    }
}
