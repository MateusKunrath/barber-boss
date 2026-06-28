using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UsersUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUsersUpdateOnlyRepository> _repository;

    public UsersUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUsersUpdateOnlyRepository>();
    }

    public UsersUpdateOnlyRepositoryBuilder GetById(User? user)
    {
        if (user is not null)
        {
            _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        }

        return this;
    }

    public IUsersUpdateOnlyRepository Build()
    {
        return _repository.Object;
    }
}