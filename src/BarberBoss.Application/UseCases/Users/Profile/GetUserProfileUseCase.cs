using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Services.AuthenticatedUser;

namespace BarberBoss.Application.UseCases.Users.Profile;

public class GetUserProfileUseCase(IAuthenticatedUser authenticatedUser, IMapper mapper) : IGetUserProfileUseCase
{
    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await authenticatedUser.Get();
        return mapper.Map<ResponseUserProfileJson>(user);
    }
}