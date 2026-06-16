using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public class GetBillingByIdUseCase(IBillingsReadOnlyRepository repository, IMapper mapper) : IGetBillingByIdUseCase
{
    public async Task<ResponseBillingJson> Execute(Guid id)
    {
        var result = await repository.GetById(id);
        
        return result is null
            ? throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND)
            : mapper.Map<ResponseBillingJson>(result);
    }
}