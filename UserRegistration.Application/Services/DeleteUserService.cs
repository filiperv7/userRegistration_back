using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

namespace UserRegistration.Application.Services
{
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> DeleteUser(Guid id, List<int> profiles)
        {
            if (profiles == null || !profiles.Contains(1))
            {
                throw new UnauthorizedAccessException("Você não tem permissão para deletar este usuário.");
            }

            User user = await _userRepository.GetById(id);
            if (user == null) return false;

            user.DeleteUser();

            await _userRepository.Update(user);

            return true;
        }
    }
}
