using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(user.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ErrorUserNotFound()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid());

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }

    private DeleteUserUseCase CreateUseCase(User? user = null)
    {
        return new DeleteUserUseCase(
            new UsersReadOnlyRepositoryBuilder().GetById(user).Build(),
            UsersWriteOnlyRepositoryBuilder.Build(),
            UnitOfWorkBuilder.Build());
    }
}