namespace UserRegistration.Domain.Interfaces.IServices
{
    public interface IDeleteUserService
    {
        Task<bool> DeleteUser(Guid id, List<int> profiles);
    }
}
