using Moq;
using UserRegistration.Application.Services;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;

public class FindUsersServicesTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly FindUsersServices _findUsersServices;

    public FindUsersServicesTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _findUsersServices = new FindUsersServices(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task FindUsersByProfile_ComPerfilExistente_DeveRetornarListaDeUsuarios()
    {
        var idProfile = 1;
        var users = new List<User> { new User(), new User() };
        _userRepositoryMock.Setup(repo => repo.GetUsersByProfile(idProfile)).ReturnsAsync(users);

        var result = await _findUsersServices.FindUsersByProfile(idProfile);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _userRepositoryMock.Verify(repo => repo.GetUsersByProfile(idProfile), Times.Once);
    }

    [Fact]
    public async Task FindUsersByProfile_ComPerfilNaoExistente_DeveRetornarListaVazia()
    {
        var idProfile = 99;
        _userRepositoryMock.Setup(repo => repo.GetUsersByProfile(idProfile)).ReturnsAsync(Enumerable.Empty<User>());

        var result = await _findUsersServices.FindUsersByProfile(idProfile);

        Assert.NotNull(result);
        Assert.Empty(result);
        _userRepositoryMock.Verify(repo => repo.GetUsersByProfile(idProfile), Times.Once);
    }

    [Fact]
    public async Task FindAllUsers_ComUsuariosExistentes_DeveRetornarListaDeUsuarios()
    {
        var users = new List<User> { new User(), new User() };
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

        var result = await _findUsersServices.FindAllUsers();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _userRepositoryMock.Verify(repo => repo.GetAllUsers(), Times.Once);
    }

    [Fact]
    public async Task FindAllUsers_ComNenhumUsuario_DeveRetornarListaVazia()
    {
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(Enumerable.Empty<User>());

        var result = await _findUsersServices.FindAllUsers();

        Assert.NotNull(result);
        Assert.Empty(result);
        _userRepositoryMock.Verify(repo => repo.GetAllUsers(), Times.Once);
    }

    [Fact]
    public async Task FindUserById_ComIdExistente_DeveRetornarUsuario()
    {
        var userId = Guid.NewGuid();
        var user = new User();
        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);

        var result = await _findUsersServices.FindUserById(userId);

        Assert.NotNull(result);
        _userRepositoryMock.Verify(repo => repo.GetById(userId), Times.Once);
    }

    [Fact]
    public async Task FindUserById_ComIdNaoExistente_DeveRetornarNulo()
    {
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

        var result = await _findUsersServices.FindUserById(userId);

        Assert.Null(result);
        _userRepositoryMock.Verify(repo => repo.GetById(userId), Times.Once);
    }

    [Fact]
    public async Task GetInfosOfLoggedUser_ComIdDoUsuarioLogadoExistente_DeveRetornarUsuario()
    {
        var requestUserId = Guid.NewGuid();
        var user = new User();
        _userRepositoryMock.Setup(repo => repo.GetById(requestUserId)).ReturnsAsync(user);

        var result = await _findUsersServices.GetInfosOfLoggedUser(requestUserId);

        Assert.NotNull(result);
        _userRepositoryMock.Verify(repo => repo.GetById(requestUserId), Times.Once);
    }

    [Fact]
    public async Task GetInfosOfLoggedUser_ComIdDoUsuarioLogadoNaoExistente_DeveRetornarNulo()
    {
        var requestUserId = Guid.NewGuid();
        _userRepositoryMock.Setup(repo => repo.GetById(requestUserId)).ReturnsAsync((User)null);

        var result = await _findUsersServices.GetInfosOfLoggedUser(requestUserId);

        Assert.Null(result);
        _userRepositoryMock.Verify(repo => repo.GetById(requestUserId), Times.Once);
    }
}
