using UserRegistration.API.Models;

namespace UserRegistration.API.Models
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Nationality { get; set; }
        public ProfileDto Profile { get; set; }
    }
}
