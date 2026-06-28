using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Users.GetById;

public class GetUserByIdUseCase(IUsersReadOnlyRepository repository, IMapper mapper) : IGetUserByIdUseCase
{
    public async Task<ResponseUserJson> Execute(Guid id)
    {
        var result = await repository.GetById(id);

        return result is null
            ? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND)
            : mapper.Map<ResponseUserJson>(result);
    }
}