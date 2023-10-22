using Microsoft.AspNetCore.Mvc;
using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;
using ShoppingCartExercise.Models.FormModels;
using ShoppingCartExercise.Repositories;

namespace ShoppingCartExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        public IBasketRepository BasketRepository { get; }
        public ILogger<BasketController> Logger { get; }

        public BasketController(IBasketRepository productRepository, ILogger<BasketController> logger)
        {
            BasketRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("get-basket")]
        public IActionResult GetBasket(int basketId)
        {
            List<Basket> basketItems = BasketRepository.GetBasket(basketId);
            return Ok(basketItems);
        }
        [HttpGet]
        [Route("calculate-total")]
        public IActionResult CalculateTotal(int basketId)
        {
            return Ok($"Your total is: {BasketRepository.CalculateTotal(basketId)}");
        }
        [HttpPost]
        [Route("add-item")]
        public IActionResult AddItemToBasket(int basketId, string barcode)
        {
            BasketRepository.AddItemToBasket(basketId, barcode);
            return Ok();
        }
        [HttpDelete]
        [Route("remove-item")]
        public IActionResult RemoveItemFromBasket(int basketId, string barcode)
        {
            BasketRepository.RemoveItemFromBasket(basketId, barcode);
            return Ok();
        }
    }
}