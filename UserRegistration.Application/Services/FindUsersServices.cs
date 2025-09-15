using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

namespace UserRegistration.Application.Services
{
    public class FindUsersServices : IFindUsersServices
    {
        private readonly IUserRepository _userRepository;

        public FindUsersServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> FindUsersByProfile(int idProfile)
        {
            return await _userRepository.GetUsersByProfile(idProfile);
        }

        public async Task<IEnumerable<User>> FindAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User> FindUserById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> GetInfosOfLoggedUser(Guid requestUserId)
        {
            return await _userRepository.GetById(requestUserId);   
        }
    }
}
