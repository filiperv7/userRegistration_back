using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;
using UserRegistration.Application.Helpers;

namespace UserRegistration.Application.Services
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;

        public CreateUserService(IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        public async Task<bool> CreateUser(User user, int idProfile, List<int> profiles)
        {
            Console.WriteLine(profiles);

            if (profiles == null || !profiles.Contains(1))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para criar um novo usuário.");
            }

            if (!user.IsValidCPF(user.CPF))
                throw new ArgumentException("CPF inválido.");
            if (!user.IsValidPassword(user.Password))
                throw new ArgumentException("A senha deve ter mais de 8 caracteres, incluindo letras maiúsculas, minúsculas e caracteres especiais.");

            bool cpfAlreadyUsed = await _userRepository.CheckIfCpfAlreadyUsed(user.CPF);
            if (cpfAlreadyUsed == true) return false;

            user.SetPassword(PasswordHelper.HashPassword(user.Password));

            user.SetProfile(idProfile);

            await _userRepository.Create(user);

            return true;
        }
    }
}