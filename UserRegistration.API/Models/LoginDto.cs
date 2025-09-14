using System.ComponentModel.DataAnnotations;

namespace UserRegistration.API.Models
{
    public class LoginDto
    {
        [Required]
        public string Cpf { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string Password { get; set; }
    }
}
