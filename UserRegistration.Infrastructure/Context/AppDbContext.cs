using Microsoft.EntityFrameworkCore;
using UserRegistration.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace UserRegistration.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminProfile = new Profile { Id = 1, Name = "Admin" };
            var userProfile = new Profile { Id = 2, Name = "User" };

            // Seed for Profiles
            modelBuilder.Entity<Profile>().HasData(
                adminProfile,
                userProfile
            );

            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, "$enHa32!");

            var creationDate = DateTime.UtcNow;
            modelBuilder.Entity<User>().HasData(
                // Seed for Admin
                new
                {
                    Id = Guid.NewGuid(),
                    CreationDate = creationDate,
                    Name = "Admin Default",
                    Email = "admin@default.com",
                    Gender = "F",
                    PlaceOfBirth = "Gama",
                    Nationality = "Brasileira",
                    CPF = "91649430035",
                    Password = hashedPassword,
                    ProfileId = adminProfile.Id,
                    Excluded = false,
                },
                // Seed for User
                new
                {
                    Id = Guid.NewGuid(),
                    CreationDate = creationDate,
                    Name = "User Default",
                    Email = "user@default.com",
                    Gender = "M",
                    PlaceOfBirth = "Gama",
                    Nationality = "Brasileira",
                    CPF = "74236780070",
                    Password = hashedPassword,
                    ProfileId = userProfile.Id,
                    Excluded = false,
                }
            );
        }
    }
}
