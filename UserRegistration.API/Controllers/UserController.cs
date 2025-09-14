using UserRegistration.API.Models;
using UserRegistration.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UserRegistration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public UserController(
            ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            string token = await _loginService.Login(loginDto.Cpf, loginDto.Password);

            if (token == null) return Unauthorized(new { Message = "Email ou senha inválidos." });

            return Ok(new { Token = $"Bearer {token}" });
        }
    }
}
