using BarberBoss.Application.UseCases.Users.Profile;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.AuthenticatedUser;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.Profile;

public class UpdateUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task ErrorNameEmpty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public async Task ErrorEmailAlreadyExists()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
    }

    private UpdateUserProfileUseCase CreateUseCase(User user, string? email = null)
    {
        var readOnlyRepository = new UsersReadOnlyRepositoryBuilder();

        if (!string.IsNullOrWhiteSpace(email))
        {
            readOnlyRepository.ExistActiveUserWithEmail(email);
        }

        return new UpdateUserProfileUseCase(
            UsersUpdateOnlyRepositoryBuilder.Build(user),
            readOnlyRepository.Build(),
            AuthenticatedUserBuilder.Build(user),
            UnitOfWorkBuilder.Build());
    }
}