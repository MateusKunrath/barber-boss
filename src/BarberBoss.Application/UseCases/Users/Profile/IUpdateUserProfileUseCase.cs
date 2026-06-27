using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Users.Profile;

public interface IUpdateUserProfileUseCase
{
    Task Execute(RequestUpdateUserProfileJson request);
}