using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Delete;

public class DeleteBillingUseCase(IBillingsWriteOnlyRepository repository, IUnitOfWork unitOfWork)
    : IDeleteBillingUseCase
{
    public async Task Execute(Guid id)
    {
        var result = await repository.Delete(id);
        if (!result)
            throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);

        await unitOfWork.Commit();
    }
}