using UserRegistration.Domain.Entities;

namespace UserRegistration.Domain.Interfaces.IServices
{
    public interface IFindUsersServices
    {
        Task<IEnumerable<User>> FindUsersByProfile(int idProfile);
        Task<User> FindUserById(Guid id);
        Task<IEnumerable<User>> FindAllUsers();
        Task<User> GetInfosOfLoggedUser(Guid requestUserId);
    }
}
