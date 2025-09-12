using UserRegistration.Domain.Entities;

namespace UserRegistration.Domain.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByCpf(string cpf);
        Task<IEnumerable<User>> GetUsersByProfile(int idProfile);
        Task<User> GetById(Guid id);
        Task Create(User user);
        Task Update(User user);
        Task Delete(Guid id);
        Task<bool> CheckIfCpfAlreadyUsed(string cpf, Guid? userId = null);
    }
}
