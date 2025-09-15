using AutoMapper;
using System.Text.RegularExpressions;
using UserRegistration.Domain.Common;

namespace UserRegistration.Domain.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.UtcNow;
        }

        public User(
            string name,
            string gender,
            string placeOfBirth,
            string nationality,
            string cpf,
            string password,
            Profile profile,
            string email = null)
        {
            if (!IsValidCPF(cpf))
                throw new ArgumentException("CPF inválido.");
            if (!IsValidPassword(password))
                throw new ArgumentException("A senha deve ter mais de 8 caracteres, incluindo letras maiúsculas, minúsculas e caracteres especiais.");

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Gender = gender;
            this.Email = email;
            this.PlaceOfBirth = placeOfBirth;
            this.Nationality = nationality;
            this.CPF = cpf;
            this.Password = password;

            this.Profile = profile;

            this.CreationDate = DateTime.UtcNow;
        }

        public User(
            Guid id,
            string name,
            string gender,
            string placeOfBirth,
            string nationality,
            string cpf,
            string password,
            Profile profile,
            string email = null)
        {
            if (!IsValidCPF(cpf))
                throw new ArgumentException("CPF inválido.");
            if (!IsValidPassword(password))
                throw new ArgumentException("A senha deve ter mais de 8 caracteres, incluindo letras maiúsculas, minúsculas e caracteres especiais.");

            this.Id = id;
            this.Name = name;
            this.Gender = gender;
            this.Email = email;
            this.PlaceOfBirth = placeOfBirth;
            this.Nationality = nationality;
            this.CPF = cpf;
            this.Password = password;

            this.Profile = profile;

            this.CreationDate = DateTime.UtcNow;
        }

        public string Name { get; private set; }

        public string? Gender { get; private set; }
        
        public string? Email { get; private set; }

        public string? PlaceOfBirth { get; private set; } // => Naturalidade

        public string? Nationality { get; private set; }

        public string CPF { get; private set; }
        
        public string Password { get; private set; }

        public Profile Profile { get; private set; }

        public int ProfileId {get; private set;}

        public bool Excluded { get; private set; } = false;

        public DateTime? ExclusionDate { get; set; }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperCase && hasLowerCase && hasSpecialChar;
        }

        public bool IsValidCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (string.IsNullOrEmpty(cpf)) return false;

            if (cpf.Length != 11) return false;

            if (cpf.All(c => c == cpf[0])) return false;

            int[] multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            int rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            string digit = rest.ToString();
            tempCpf = tempCpf + digit;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            rest = sum % 11;
            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit = digit + rest.ToString();

            return cpf.EndsWith(digit);
        }

        public void DeleteUser()
        {
            this.Excluded = true;
            this.ExclusionDate = DateTime.UtcNow;
        }

        public void SetPassword(string password)
        {
            if (!IsValidPassword(password))
                throw new ArgumentException("A senha deve ter mais de 8 caracteres, incluindo letras maiúsculas, minúsculas e caracteres especiais.");

            this.Password = password;
        }


        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetEmail(string email)
        {
            this.Email = email;
        }

        public void SetCpf(string cpf)
        {
            if (!IsValidCPF(cpf))
                throw new ArgumentException("CPF inválido.");

            this.CPF = cpf;
        }

        public void SetPlaceOfBirth(string placeOfBirth)
        {
            this.PlaceOfBirth = placeOfBirth;
        }

        public void SetNationality(string nationality)
        {
            this.Nationality = nationality;
        }

        public void SetProfile(int idProfile)
        {
            this.ProfileId = idProfile;
        }
    }
}
