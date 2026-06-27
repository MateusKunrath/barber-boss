using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Authentication.Authenticate;

public interface IAuthenticateUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestAuthenticateJson request);
}