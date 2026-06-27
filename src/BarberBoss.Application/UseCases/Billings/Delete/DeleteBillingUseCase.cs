using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Delete;

public class DeleteBillingUseCase(
    IBillingsReadOnlyRepository readOnlyRepository,
    IBillingsWriteOnlyRepository writeOnlyRepository, 
    IUnitOfWork unitOfWork): IDeleteBillingUseCase
{
    public async Task Execute(Guid id)
    {
        var billing = await readOnlyRepository.GetById(id);
        if (billing is null)
        {
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);            
        }

        await writeOnlyRepository.Delete(id);
        await unitOfWork.Commit();
    }
}