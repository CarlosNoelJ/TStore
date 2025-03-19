using Microsoft.AspNetCore.Mvc;
using Berx3.Api.Repositories;

namespace Berx3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseRepository _purchaseRepository;

        public PurchaseController(PurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            return Ok(await _purchaseRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return Ok(purchase);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody] PurchaseRequest purchaseRequest)
        {
            var result = await _purchaseRepository.AddPurchaseAsync(
                purchaseRequest.UserId,
                purchaseRequest.TShirtId,
                purchaseRequest.Quantity);

            if (result == "Purchase successful.")
            {
                return Ok(new { message = result });
            }

            return BadRequest(new { message = result });
        }
    }

    public class PurchaseRequest
    {
        public int UserId { get; set; }
        public int TShirtId { get; set; }
        public int Quantity { get; set; }
    }
}