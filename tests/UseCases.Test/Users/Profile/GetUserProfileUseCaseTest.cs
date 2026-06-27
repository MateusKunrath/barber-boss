using BarberBoss.Application.UseCases.Users.Profile;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.AuthenticatedUser;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using FluentAssertions;

namespace UseCases.Test.Users.Profile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
        result.Role.Should().Be(user.Role.ToString());
    }

    private GetUserProfileUseCase CreateUseCase(User user)
    {
        return new GetUserProfileUseCase(
            AuthenticatedUserBuilder.Build(user),
            MapperBuilder.Build());
    }
}