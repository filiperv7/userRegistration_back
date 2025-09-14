using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRegistration.API.Models;
using UserRegistration.Application.Helpers;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IServices;

namespace UserRegistration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ICreateUserService _createUserService;
        private readonly IMapper _mapper;

        public UserController(
            ILoginService loginService,
            ICreateUserService createUserService,
            IMapper mapper
            )
        {
            _loginService = loginService;
            _createUserService = createUserService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            string token = await _loginService.Login(loginDto.Cpf, loginDto.Password);

            if (token == null) return Unauthorized(new { Message = "Email ou senha inválidos." });

            return Ok(new { Token = $"Bearer {token}" });
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDto);

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var profiles = JwtHelper.GetProfilesFromToken(token);

            try
            {
                bool created = await _createUserService.CreateUser(user, userDto.IdProfile, profiles);

                return created
                    ? Ok(new { Message = "Usuário criado com sucesso!" })
                    : BadRequest(new { Message = "CPF já utilizado por outro usuário!" });
            }
            catch (UnauthorizedAccessException error)
            {
                return Forbid(error.Message);
            }
        }
    }
}
