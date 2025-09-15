using Moq;
using UserRegistration.Application.Services;
using UserRegistration.Domain.Entities;
using UserRegistration.Domain.Interfaces.IRepositories;
using UserRegistration.Domain.Interfaces.IServices;

public class EditUserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IEditUserService _editUserService;

    public EditUserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _editUserService = new EditUserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task EditUser_ComPermissoesECpfValidosENaoEmUso_DeveRetornarTrue()
    {
        var adminId = Guid.NewGuid();
        var userToUpdateId = Guid.NewGuid();
        var adminProfiles = new List<int> { 1 };
        var userToUpdate = new User(userToUpdateId, "User to Update", "M", "Cidade", "Pais", "55298110002", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "user@email.com");
        var existingUser = new User(userToUpdateId, "Existing User", "M", "Cidade", "Pais", "55298110002", "SenhaAntiga!1", new Profile { Id = 2, Name = "User" }, "existing@email.com");

        _userRepositoryMock.Setup(repo => repo.GetById(userToUpdateId)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.CheckIfCpfAlreadyUsed(userToUpdate.CPF, userToUpdateId)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _editUserService.EditUser(userToUpdate, adminProfiles, adminId);

        Assert.True(result);
        _userRepositoryMock.Verify(repo => repo.GetById(userToUpdateId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.CheckIfCpfAlreadyUsed(userToUpdate.CPF, userToUpdateId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task EditUser_ComUsuarioNaoAdminEditandoOutroUsuario_DeveLancarUnauthorizedAccessException()
    {
        var nonAdminId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var nonAdminProfiles = new List<int> { 2 };
        var userToUpdate = new User(anotherUserId, "Outro Usuario", "F", "Cidade", "Pais", "55298110002", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "outro@email.com");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _editUserService.EditUser(userToUpdate, nonAdminProfiles, nonAdminId));
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task EditUser_ComCpfInvalido_DeveLancarArgumentException()
    {
        var userId = Guid.NewGuid();
        var adminProfiles = new List<int> { 1 };
        var userProfileId = 2;

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            var userWithInvalidCpf = new User(userId, "Nome", "M", "Cidade", "Pais", "85298110003", "SenhaSegura!1", new Profile { Id = userProfileId, Name = "User" }, "email@novo.com");
            return _editUserService.EditUser(userWithInvalidCpf, adminProfiles, userId);
        });

        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task EditUser_ComUsuarioInexistente_DeveLancarKeyNotFoundException()
    {
        var userId = Guid.NewGuid();
        var adminProfiles = new List<int> { 1 };
        var userToUpdate = new User(userId, "Nome", "M", "Cidade", "Pais", "55298110002", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "email@novo.com");

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _editUserService.EditUser(userToUpdate, adminProfiles, userId));
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task EditUser_ComCpfJaEmUso_DeveRetornarFalse()
    {
        var userId = Guid.NewGuid();
        var adminProfiles = new List<int> { 1 };
        var userToUpdate = new User(userId, "Nome", "M", "Cidade", "Pais", "55298110002", "SenhaSegura!1", new Profile { Id = 2, Name = "User" }, "email@novo.com");
        var existingUser = new User();

        _userRepositoryMock.Setup(repo => repo.GetById(userId)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.CheckIfCpfAlreadyUsed(userToUpdate.CPF, userId)).ReturnsAsync(true);

        var result = await _editUserService.EditUser(userToUpdate, adminProfiles, userId);

        Assert.False(result);
        _userRepositoryMock.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
    }
}
