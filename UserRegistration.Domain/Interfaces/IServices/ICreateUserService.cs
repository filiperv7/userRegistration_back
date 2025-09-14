using UserRegistration.Domain.Entities;

namespace UserRegistration.Domain.Interfaces.IServices
{
    public interface ICreateUserService
    {
        Task<bool> CreateUser(User user, int idProfile, List<int> profiles);
    }
}
