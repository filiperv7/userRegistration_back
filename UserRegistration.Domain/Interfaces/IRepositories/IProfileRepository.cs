using UserRegistration.Domain.Entities;

namespace UserRegistration.Domain.Interfaces.IRepositories
{
    public interface IProfileRepository
    {
        Task<Profile> GetProfileById(int id);
    }
}
