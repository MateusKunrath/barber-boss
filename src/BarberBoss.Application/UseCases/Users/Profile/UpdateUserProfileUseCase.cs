using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Services.AuthenticatedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.Profile;

public class UpdateUserProfileUseCase(
    IUsersUpdateOnlyRepository usersUpdateOnlyRepository,
    IUsersReadOnlyRepository usersReadOnlyRepository,
    IAuthenticatedUser authenticatedUser,
    IUnitOfWork unitOfWork) : IUpdateUserProfileUseCase
{
    public async Task Execute(RequestUpdateUserProfileJson request)
    {
        var loggedUser = await authenticatedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await usersUpdateOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        usersUpdateOnlyRepository.Update(user);
        await unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserProfileJson request, string currentEmail)
    {
        var validator = new UpdateUserProfileValidator();

        var result = await validator.ValidateAsync(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExists = await usersReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_EXISTS));
            }
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}