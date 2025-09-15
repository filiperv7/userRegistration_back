using Moq;
using UserRegistration.Application.Services;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

public class DeleteUserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IDeleteUserService _deleteUserService;

    public DeleteUserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _deleteUserService = new DeleteUserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task DeleteUser_ComPermissaoAdminEUsuarioExistente_DeveRetornarTrue()
    {
        var adminProfiles = new List<int> { 1 };
        var userId = Guid.NewGuid();
        var existingUser = new User();

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _deleteUserService.DeleteUser(userId, adminProfiles);

        Assert.True(result);
        _userRepositoryMock.Verify(repo => repo.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(existingUser), Times.Once);
        Assert.True(existingUser.Excluded);
    }

    [Fact]
    public async Task DeleteUser_ComUsuarioNaoAdmin_DeveLancarUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid();
        var nonAdminProfiles = new List<int> { 2 };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _deleteUserService.DeleteUser(userId, nonAdminProfiles));
        _userRepositoryMock.Verify(repo => repo.GetById(It.IsAny<Guid>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUser_ComListaDePerfisNula_DeveLancarUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid();
        List<int> nullProfiles = null;

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _deleteUserService.DeleteUser(userId, nullProfiles));
        _userRepositoryMock.Verify(repo => repo.GetById(It.IsAny<Guid>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteUser_ComUsuarioInexistente_DeveRetornarFalse()
    {
        var adminProfiles = new List<int> { 1 };
        var userId = Guid.NewGuid();

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

        var result = await _deleteUserService.DeleteUser(userId, adminProfiles);

        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }
}
