using BarberBoss.Application.UseCases.Billings.Update;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Billings.Update;

public class UpdateBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestBillingJsonBuilder.Build();
        var billing = BillingBuilder.Build();
        
        var useCase = CreateUseCase(billing);

        var act = async () => await useCase.Execute(billing.Id, request);

        await act.Should().NotThrowAsync();
        
        billing.BarberName.Should().Be(request.BarberName);
        billing.ClientName.Should().Be(request.ClientName);
        billing.ServiceName.Should().Be(request.ServiceName);
        billing.Notes.Should().Be(request.Notes);
        billing.Amount.Should().Be(request.Amount);
        billing.Status.Should().Be((Status)request.Status);
        billing.PaymentMethod.Should().Be((PaymentMethod)request.PaymentMethod);
        billing.Date.Should().Be(request.Date);
    }

    [Fact]
    public async Task ErrorBarberNameEmpty()
    {
        var billing =  BillingBuilder.Build();
        var request = RequestBillingJsonBuilder.Build();
        request.BarberName = string.Empty;
        var useCase = CreateUseCase(billing);
        
        var act = async () => await useCase.Execute(billing.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BARBER_NAME_REQUIRED));
    }

    [Fact]
    public async Task ErrorBillingNotFound()
    {
        var request = RequestBillingJsonBuilder.Build();
        var useCase = CreateUseCase();
        
        var act = async () => await useCase.Execute(Guid.NewGuid(), request);
        
        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BILLING_NOT_FOUND));
    }
    
    private UpdateBillingUseCase CreateUseCase(Billing? billing = null)
    {
        return new UpdateBillingUseCase(
            new BillingsUpdateOnlyRepositoryBuilder().GetById(billing).Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build());
    }
}