using System.ComponentModel.DataAnnotations;

namespace UserRegistration.API.Models
{
    public class UserCreationDto
    {
        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve conter exatamente 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas dígitos numéricos.")]
        public string Cpf { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "A Nome deve ter pelo menos 3 caracteres.")]
        public string Name { get; set; }

        [StringLength(1, MinimumLength = 1, ErrorMessage = "O Sexo deve conter somente 1 caractere.")]
        [AllowedValues(["F", "M"], ErrorMessage = "O valores aceitos são F para Feminino, M para Masculino")]
        public string? Gender { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string? Email { get; set; }

        [MinLength(5, ErrorMessage = "A Naturalidade deve conter pelo menos 8 caracteres.")]
        public string? PlaceOfBirth { get; set; }


        [MinLength(5, ErrorMessage = "A Naturalidade deve conter pelo menos 8 caracteres.")]
        public string? Nationality { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string Password { get; set; }

        [Required]
        [AllowedValues([1,2], ErrorMessage = "O valores aceitos são 1 para usuário admin ou 2 para usuário comum")]
        public int IdProfile { get; set; }
    }
}
