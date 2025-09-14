using Moq;
using UserRegistration.Application.Services;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

public class LoginServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginService _loginService;

    public LoginServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loginService = new LoginService(_userRepositoryMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
    {
        var cpf = "91649430035";
        var password = "Adm!n123";
        var profile = new Profile { Id = 1, Name = "Admin" };
        var user = new User("Admin Default", "F", "Gama", "Brasileira", cpf, BCrypt.Net.BCrypt.HashPassword(password), profile, "admin@default.com");

        _userRepositoryMock.Setup(repo => repo.GetUserByCpf(cpf))
           .ReturnsAsync(user);

        _tokenServiceMock.Setup(service => service.GenerateToken(user))
           .Returns("fake-jwt-token");

        var result = await _loginService.Login(cpf, password);

        Assert.NotNull(result);
        Assert.Equal("fake-jwt-token", result);

        _userRepositoryMock.Verify(repo => repo.GetUserByCpf(cpf), Times.Once);
        _tokenServiceMock.Verify(service => service.GenerateToken(user), Times.Once);
    }

    [Fact]
    public async Task Login_ComUsuarioInexistente_DeveRetornarNulo()
    {
        var cpf = "99999999999";
        var password = "senhaInvalida123";

        _userRepositoryMock.Setup(repo => repo.GetUserByCpf(cpf))
           .ReturnsAsync((User)null);

        var result = await _loginService.Login(cpf, password);

        Assert.Null(result);

        _tokenServiceMock.Verify(service => service.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Login_ComSenhaIncorreta_DeveRetornarNulo()
    {
        var cpf = "91649430035";
        var correctPassword = "Adm!n123";
        var wrongPassword = "senhaErrada";
        var profile = new Profile { Id = 1, Name = "Admin" };
        var user = new User("Admin Default", "F", "Gama", "Brasileira", cpf, BCrypt.Net.BCrypt.HashPassword(correctPassword), profile, "admin@default.com");

        _userRepositoryMock.Setup(repo => repo.GetUserByCpf(cpf))
           .ReturnsAsync(user);

        var result = await _loginService.Login(cpf, wrongPassword);

        Assert.Null(result);

        _tokenServiceMock.Verify(service => service.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}