using Microsoft.AspNetCore.Mvc;
using ShoppingCartExercise.DatabaseContext;
using ShoppingCartExercise.Models.DatabaseModels;
using ShoppingCartExercise.Models.FormModels;
using ShoppingCartExercise.Repositories;

namespace ShoppingCartExercise.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfferController : ControllerBase
    {
        public IOfferRepository OfferRepository { get; }
        public ILogger<OfferController> Logger { get; }

        public OfferController(IOfferRepository productRepository, ILogger<OfferController> logger)
        {
            OfferRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(Name = "[controller]/GetAll")]
        public IActionResult GetOffers()
        {
            List<Offer> allOffers = OfferRepository.GetAll();
            return Ok(allOffers);
        }

        [HttpPost(Name = "[controller]/Create")]
        public IActionResult AddOffer([FromBody] OfferForm offerForm)
        {
            Offer offer = new Offer()
            {
                OfferType = offerForm.OfferType,
                OfferValue = offerForm.OfferValue,
                ProductBarcode = offerForm.ProductBarcode
            };
            OfferRepository.Add(offer);
            return Ok();
        }

        [HttpDelete(Name = "[controller]/Remove")]
        public IActionResult Delete(int offerId)
        {
            OfferRepository.Remove(offerId);
            return Ok();
        }

    }
}