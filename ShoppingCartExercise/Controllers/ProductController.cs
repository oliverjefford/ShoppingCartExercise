using Microsoft.AspNetCore.Mvc;
using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;
using ShoppingCartExercise.Repositories;

namespace ShoppingCartExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        public IProductRepository ProductRepository { get; }
        public ILogger<ProductController> Logger { get; }

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            ProductRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("get-products")]
        public IActionResult GetProducts()
        {
            List<Product> allProducts = ProductRepository.GetAll();
            return Ok(allProducts);
        }

        [HttpPost]
        [Route("add-products")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            ProductRepository.Add(product);
            return Ok();
        }

        [HttpDelete]
        [Route("remove-product")]
        public IActionResult Delete(string barcode)
        {
            ProductRepository.RemoveProduct(barcode);
            return Ok();
        }

    }
}