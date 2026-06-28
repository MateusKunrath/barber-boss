using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Users.Update;

public class UpdateUserUseCase(
    IUsersUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUpdateUserUseCase
{
    public async Task Execute(Guid id, RequestUpdateUserJson request)
    {
        Validate(request);

        var user = await repository.GetById(id);
        if (user is null)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        mapper.Map(request, user);
        repository.Update(user);
        await unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateUserJson request)
    {
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
        {
            return;
        }

        var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}