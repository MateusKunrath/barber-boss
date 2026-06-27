using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Communication.Enums;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Billings.GetById;

public class GetBillingByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var billing = BillingBuilder.Build();
        var useCase = CreateUseCase(billing);

        var result = await useCase.Execute(billing.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(billing.Id);
        result.BarberName.Should().Be(billing.BarberName);
        result.ClientName.Should().Be(billing.ClientName);
        result.ServiceName.Should().Be(billing.ServiceName);
        result.Date.Should().Be(billing.Date);
        result.Notes.Should().Be(billing.Notes);
        result.Amount.Should().Be(billing.Amount);
        result.PaymentMethod.Should().Be((PaymentMethod)billing.PaymentMethod);
        result.Status.Should().Be((Status)billing.Status);
        result.CreatedAt.Should().Be(billing.CreatedAt);
        result.UpdatedAt.Should().Be(billing.UpdatedAt);
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

    private GetBillingByIdUseCase CreateUseCase(Billing? billing = null)
    {
        return new GetBillingByIdUseCase(
            new BillingsReadOnlyRepositoryBuilder().GetById(billing).Build(),
            MapperBuilder.Build());
    }
}