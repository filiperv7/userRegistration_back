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
        private readonly IFindUsersServices _findUsersServices;
        private readonly IEditUserService _editUserService;
        private readonly IMapper _mapper;

        public UserController(
            ILoginService loginService,
            ICreateUserService createUserService,
            IFindUsersServices findUsersServices,
            IEditUserService editUserService,
            IMapper mapper
            )
        {
            _loginService = loginService;
            _createUserService = createUserService;
            _findUsersServices = findUsersServices;
            _editUserService = editUserService;
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

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> FindUserById(Guid id)
        {
            var user = await _findUsersServices.FindUserById(id);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            var userDto = _mapper.Map<UserResponseDto>(user);
            return Ok(userDto);
        }

        [Authorize]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> FindAllUsers()
        {
            var users = await _findUsersServices.FindAllUsers();

            if (!users.Any())
            {
               return NotFound(new { Message = "Nenhum usuário encontrado." });
            }

            var usersDto = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return Ok(usersDto);
        }

        [Authorize]
        [HttpGet]
        [Route("profile/{idProfile}")]
        public async Task<IActionResult> FindUsersByProfile(int idProfile)
        {
            var users = await _findUsersServices.FindUsersByProfile(idProfile);

            if (!users.Any())
            {
                return NotFound(new { Message = "Nenhum usuário encontrado com o perfil especificado." });
            }

            var usersDto = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return Ok(usersDto);
        }

        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDto);

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var profiles = JwtHelper.GetProfilesFromToken(token);
            var requestUserId = JwtHelper.GetClaimIdUserFromToken(token);

            try
            {
                bool updated = await _editUserService.EditUser(user, profiles, Guid.Parse(requestUserId));

                return updated
                    ? Ok(new { Message = "Usuário atualizado com sucesso!" })
                    : BadRequest(new { Message = "Falha na atualização do usuário, CPF já utilizado." });
            }
            catch (UnauthorizedAccessException error)
            {
                return Forbid(error.Message);
            }
            catch (KeyNotFoundException error)
            {
                return NotFound(error.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("infos_of_logged_user")]
        public async Task<IActionResult> GetInfosOfLoggedUser()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var requestUserId = JwtHelper.GetClaimIdUserFromToken(token);

            User loggedUser = await _findUsersServices.GetInfosOfLoggedUser(Guid.Parse(requestUserId));

            return loggedUser != null
                ? Ok(_mapper.Map<UserResponseDto>(loggedUser))
                : NotFound(new { Message = "Usuário não encontrado." });
        }
    }
}
