using System.ComponentModel.DataAnnotations;

namespace ShoppingCartExercise.Models.DatabaseModels
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        [Key]
        public string ProductBarcode { get; set; }
        public int Quantity { get; set; }
        public int? OfferId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Offer? Offer { get; set; }
    }
}
