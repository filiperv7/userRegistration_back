using Moq;
using UserRegistration.Application.Services;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;

public class CreateUserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IProfileRepository> _profileRepositoryMock;
    private readonly CreateUserService _createUserService;

    public CreateUserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _profileRepositoryMock = new Mock<IProfileRepository>();
        _createUserService = new CreateUserService(
            _userRepositoryMock.Object,
            _profileRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateUser_ComCredenciaisAdminValidasECpfQueNaoEstaEmUso_DeveRetornarTrue()
    {
        var user = new User("Novo Usuario", "M", "Cidade", "Pais", "04060722057", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "email@novo.com");
        var adminProfiles = new List<int> { 1 };
        var userProfileId = 2;

        _userRepositoryMock.Setup(repo => repo.CheckIfCpfAlreadyUsed(user.CPF, null)).ReturnsAsync(false);
        _profileRepositoryMock.Setup(repo => repo.GetProfileById(userProfileId)).ReturnsAsync(new Profile { Id = userProfileId });


        bool result = await _createUserService.CreateUser(user, userProfileId, adminProfiles);

        Assert.True(result);
        _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateUser_ComCredenciaisAdminValidasECpfEmUso_DeveRetornarFalse()
    {
        var user = new User("Novo Usuario", "M", "Cidade", "Pais", "04060722057", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "email@novo.com");
        var adminProfiles = new List<int> { 1 };
        var userProfileId = 2;

        _userRepositoryMock.Setup(repo => repo.CheckIfCpfAlreadyUsed(user.CPF, null)).ReturnsAsync(true);


        bool result = await _createUserService.CreateUser(user, userProfileId, adminProfiles);

        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task CreateUser_ComUsuarioNaoAdmin_DeveLancarExcecao()
    {
        var user = new User("Usuario Sem Permissao dos Santos", "F", "Cidade", "Pais", "04060722057", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "sempermissao@email.com");
        var nonAdminProfiles = new List<int> { 2 };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _createUserService.CreateUser(user, 2, nonAdminProfiles));
        _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task CreateUser_ComCpfInvalido_DeveLancarExcecao()
    {
        var invalidCpf = "04082922053";
        var adminProfiles = new List<int> { 1 };

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            var user = new User("Usuario CPF Inválido Pereira", "F", "Cidade", "Pais", invalidCpf, "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "sempermissao@email.com");
            return _createUserService.CreateUser(user, 2, adminProfiles);
        });

        _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task CreateUser_ComSenhaInvalida_DeveLancarExcecao()
    {
        var invalidPassword = "senha";
        var adminProfiles = new List<int> { 1 };
        var userProfileId = 2;

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            var user = new User("Usuario Senha Inválida da Silva", "F", "Cidade", "Pais", "04060722057", invalidPassword, new Profile { Id = userProfileId, Name = "User" }, "sempermissao@email.com");
            return _createUserService.CreateUser(user, userProfileId, adminProfiles);
        });

        _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task CreateUser_ComCpfJaEmUso_DeveRetornarFalse()
    {
        var user = new User("Usuario Duplicado Vasconcelos", "M", "Cidade", "Pais", "04060722057", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "duplicado@email.com");
        var adminProfiles = new List<int> { 1 };
        var userProfileId = 2;

        _userRepositoryMock.Setup(repo => repo.CheckIfCpfAlreadyUsed(user.CPF, null)).ReturnsAsync(true);


        bool result = await _createUserService.CreateUser(user, userProfileId, adminProfiles);


        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Never);
    }
}