using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Update;

public class UpdateBillingUseCase(
    IBillingsUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IUpdateBillingUseCase
{
    public async Task Execute(Guid id, RequestBillingJson request)
    {
        Validate(request);

        var billing = await repository.GetById(id);
        if (billing is null)
        {
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);
        }

        mapper.Map(request, billing);
        repository.Update(billing);
        await unitOfWork.Commit();
    }

    private static void Validate(RequestBillingJson request)
    {
        var validator = new BillingValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
        {
            return;
        }

        var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorMessages);
    }
}