using Berx3.Api.Models;
using Berx3.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Berx3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UsersController(IRepository<User> userRepository)
            => _userRepository = userRepository;

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id}, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null) return BadRequest("User data is requiered.");
            if ( id != user.Id) return BadRequest("ID mismatch");

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return NotFound();

            await _userRepository.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            await _userRepository.SoftDeleteAsync(id);
            return NoContent();
        }
    }
}
