using System.ComponentModel.DataAnnotations;

namespace UserRegistration.API.Models
{
    public class UserUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve conter exatamente 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas dígitos numéricos.")]
        public string Cpf { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "O Nome deve conter pelo menos 3 caracteres.")]
        public string Name { get; set; }

        [StringLength(1, MinimumLength = 1, ErrorMessage = "O Sexo deve conter somente 1 caractere.")]
        [AllowedValues(["F", "M"], ErrorMessage = "O valores aceitos são F para Feminino ou M para Masculino")]
        public string? Gender { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string? Email { get; set; }

        [MinLength(5, ErrorMessage = "A Naturalidade deve conter pelo menos 8 caracteres.")]
        public string? PlaceOfBirth { get; set; }


        [MinLength(5, ErrorMessage = "A Naturalidade deve conter pelo menos 8 caracteres.")]
        public string? Nationality { get; set; }
    }
}
