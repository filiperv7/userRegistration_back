using UserRegistration.Domain.Entities;

namespace UserRegistration.Domain.Interfaces.IServices
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
