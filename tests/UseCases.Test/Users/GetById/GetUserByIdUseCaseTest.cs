using BarberBoss.Application.UseCases.Users.GetById;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Extensions;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users.GetById;

public class GetUserByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(user.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
        result.Role.Should().Be(user.Role.RoleToString());
        result.CreatedAt.Should().Be(user.CreatedAt);
        result.UpdatedAt.Should().Be(user.UpdatedAt);
    }

    [Fact]
    public async Task ErrorUserNotFound()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid());

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }

    private GetUserByIdUseCase CreateUseCase(User? user = null)
    {
        return new GetUserByIdUseCase(
            new UsersReadOnlyRepositoryBuilder().GetById(user).Build(),
            MapperBuilder.Build());
    }
}