using Microsoft.AspNetCore.Mvc;
using Berx3.Api.Models;
using Berx3.Api.Repositories;

namespace Berx3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TShirtsController : ControllerBase
    {
        private readonly IRepository<TShirt> _tshirtRepository;

        public TShirtsController(IRepository<TShirt> tshirtRepository)
        {
            _tshirtRepository = tshirtRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetShirts()
        {
            return Ok(await _tshirtRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTShirtById(int id)
        {
            var tshirt = await _tshirtRepository.GetByIdAsync(id);
            if (tshirt == null)
            {
                return NotFound();
            }
            return Ok(tshirt);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTShirt([FromBody] TShirt tshirt)
        {
            await _tshirtRepository.AddAsync(tshirt);
            return CreatedAtAction(nameof(GetTShirtById), new { id = tshirt.Id }, tshirt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTShirt(int id, [FromBody] TShirt tshirt)
        {
            if (id != tshirt.Id)
            {
                return BadRequest();
            }
            await _tshirtRepository.UpdateAsync(tshirt);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteTShirt(int id)
        {
            await _tshirtRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
