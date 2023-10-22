using System.ComponentModel.DataAnnotations;

namespace ShoppingCartExercise.Models.DatabaseModels
{
    public class Product
    {
        [Key]
        public string Barcode { get; set; }
        public int Price { get; set; }
    }
}
