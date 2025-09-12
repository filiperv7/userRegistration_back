using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace UserRegistration.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByCpf(string cpf)
        {
            return await _context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.CPF == cpf);
        }

        public async Task<IEnumerable<User>> GetUsersByProfile(int idProfile)
        {
            return await _context.Users
            .Include(u => u.Profile)
            .Where(u => u.Profile.Equals(idProfile) && !u.Excluded)
            .ToListAsync();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == id && !u.Excluded);
        }

        public async Task Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckIfCpfAlreadyUsed(string cpf, Guid? userId = null)
        {
            if (userId == null) {
                if (await _context.Users.FirstOrDefaultAsync(u => u.CPF == cpf && !u.Excluded) == null) return false;
                
                return true;
            }

            if (await _context.Users.FirstOrDefaultAsync(u => u.CPF == cpf && !u.Excluded && u.Id != userId) == null) return false;

            return true;
        }
    }
}
