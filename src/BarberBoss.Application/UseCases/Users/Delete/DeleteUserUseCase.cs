using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Users.Delete;

public class DeleteUserUseCase(
    IUsersReadOnlyRepository usersReadOnlyRepository,
    IUsersWriteOnlyRepository usersWriteOnlyRepository,
    IUnitOfWork unitOfWork) : IDeleteUserUseCase
{
    public async Task Execute(Guid id)
    {
        var user = await usersReadOnlyRepository.GetById(id);
        if (user is null)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        await usersWriteOnlyRepository.Delete(user);
        await unitOfWork.Commit();
    }
}