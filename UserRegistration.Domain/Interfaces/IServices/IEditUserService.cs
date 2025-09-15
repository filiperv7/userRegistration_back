using UserRegistration.Domain.Entities;

namespace UserRegistration.Domain.Interfaces.IServices
{
    public interface IEditUserService
    {
        public Task<bool> EditUser(User user, List<int> requestUserProfiles, Guid requestUserId);
    }
}
