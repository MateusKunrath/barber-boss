using BarberBoss.Application.UseCases.Billings.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Billings.Delete;

public class DeleteBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var billing = BillingBuilder.Build();
        
        var useCase = CreateUseCase(billing);

        var act = async () => await useCase.Execute(billing.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ErrorBillingNotFound()
    {
        var useCase = CreateUseCase();
        
        var act = async () => await useCase.Execute(Guid.NewGuid());

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BILLING_NOT_FOUND));
    }
    
    private DeleteBillingUseCase CreateUseCase(Billing? billing = null)
    {
        return new DeleteBillingUseCase(
            new BillingsReadOnlyRepositoryBuilder().GetById(billing).Build(),
            BillingsWriteOnlyRepositoryBuilder.Build(),
            UnitOfWorkBuilder.Build());
    }
}