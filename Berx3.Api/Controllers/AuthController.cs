using Berx3.Api.Request;
using Berx3.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Berx3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
            => _jwtService = jwtService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _jwtService.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }
            
            return Unauthorized(new { Message = "Invalid username or password." });
        }
    }
}
