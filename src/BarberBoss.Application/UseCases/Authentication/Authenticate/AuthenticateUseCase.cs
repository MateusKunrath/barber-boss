using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Exception.ExceptionsBase;
using BarberBoss.Infrastructure.Security.Tokens;

namespace BarberBoss.Application.UseCases.Authentication.Authenticate;

public class AuthenticateUseCase(
    IUsersReadOnlyRepository repository,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator tokenGenerator) : IAuthenticateUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestAuthenticateJson request)
    {
        var user = await repository.GetUserByEmail(request.Email);
        if (user is null)
        {
            throw new InvalidAuthenticationException();
        }

        var passwordMatch = passwordEncrypter.Verify(request.Password, user.Password);
        if (!passwordMatch)
        {
            throw new InvalidAuthenticationException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = tokenGenerator.Generate(user),
        };
    }
}