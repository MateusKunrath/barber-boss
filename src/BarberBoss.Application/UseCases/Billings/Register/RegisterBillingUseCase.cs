using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Register;

public class RegisterBillingUseCase(
    IBillingsWriteOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRegisterBillingUseCase
{
    public async Task<ResponseRegisteredBillingJson> Execute(RequestBillingJson request)
    {
        Validate(request);
        
        var entity = mapper.Map<Billing>(request);
        
        await repository.Add(entity);
        await unitOfWork.Commit();

        return mapper.Map<ResponseRegisteredBillingJson>(entity);
    }

    private static void Validate(RequestBillingJson request)
    {
        var validator = new BillingValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
            return;

        var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
        
        throw new ErrorOnValidationException(errorMessages);
    }
}