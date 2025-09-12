using Microsoft.EntityFrameworkCore;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Infrastructure.Context;

namespace UserRegistration.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetProfileById(int id)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
