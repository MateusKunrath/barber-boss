using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UsersReadOnlyRepositoryBuilder
{
    private readonly Mock<IUsersReadOnlyRepository> _repository;

    public UsersReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUsersReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(usersReadOnly => usersReadOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UsersReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(usersReadOnly => usersReadOnly.GetUserByEmail(user.Email)).ReturnsAsync(user);
        return this;
    }

    public IUsersReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}