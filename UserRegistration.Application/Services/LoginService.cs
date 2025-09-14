using UserRegistration.Application.Helpers;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

namespace UserRegistration.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<string> Login(string cpf, string password)
        {
            User user = await _userRepository.GetUserByCpf(cpf);

            if (user == null || !PasswordHelper.VerifyPassword(password, user.Password))
            {
                return null;
            }

            return _tokenService.GenerateToken(user);
        }
    }
}
