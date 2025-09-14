namespace UserRegistration.Domain.Interfaces.IServices
{
    public interface ILoginService
    {
        Task<string> Login(string cpf, string password);
    }
}
