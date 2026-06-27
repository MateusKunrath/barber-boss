using BarberBoss.Application.UseCases.Authentication.Authenticate;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Authentication.Authenticate;

public class AuthenticateUseCaseTest
{
    [Fact]
    public async Task Authenticate()
    {
        var user = UserBuilder.Build();
        var request = RequestAuthenticateJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);
        request.Email = user.Email;

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ErrorUserNotFound()
    {
        var user = UserBuilder.Build();
        var request = RequestAuthenticateJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidAuthenticationException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public async Task ErrorPasswordDontMatch()
    {
        var user = UserBuilder.Build();
        var request = RequestAuthenticateJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        request.Email = user.Email;

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidAuthenticationException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    private AuthenticateUseCase CreateUseCase(User user, string? password = null)
    {
        return new AuthenticateUseCase(
            new UsersReadOnlyRepositoryBuilder().GetUserByEmail(user).Build(),
            new PasswordEncrypterBuilder().Verify(password).Build(),
            JwtTokenGeneratorBuilder.Build());
    }
}