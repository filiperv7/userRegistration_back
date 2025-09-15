using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

namespace UserRegistration.Application.Services
{
    public class EditUserService : IEditUserService
    {
        private readonly IUserRepository _userRepository;

        public EditUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> EditUser(User user, List<int> requestUserProfiles, Guid requestUserId)
        {
            if (requestUserProfiles == null || (!requestUserProfiles.Contains(1) && user.Id != requestUserId))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para editar este usuário.");
            }

            if (!user.IsValidCPF(user.CPF))
                throw new ArgumentException("CPF inválido.");

            var existingUser = await _userRepository.GetById(user.Id);
            if (existingUser == null) throw new KeyNotFoundException("Usuário não encontrado");

            bool cpfAlreadyUsed = await _userRepository.CheckIfCpfAlreadyUsed(user.CPF, user.Id);
            if (cpfAlreadyUsed == true) return false;

            existingUser.SetName(user.Name);
            existingUser.SetEmail(user.Email);
            existingUser.SetCpf(user.CPF);
            existingUser.SetPlaceOfBirth(user.PlaceOfBirth);
            existingUser.SetNationality(user.Nationality);
            existingUser.UpdateDate = DateTime.UtcNow;

            await _userRepository.Update(existingUser);

            return true;
        }
    }
}
