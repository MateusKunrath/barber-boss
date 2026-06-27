using BarberBoss.Application.UseCases.Billings.GetAll;
using BarberBoss.Communication.Enums;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Billings.GetAll;

public class GetAllBillingsUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var billings = BillingBuilder.Collection();
        
        var useCase = CreateUseCase(billings);
        var request = RequestGetBillingsJsonBuilder.Build();

        var result = await useCase.Execute(request);
        
        result.Should().NotBeNull();
        result.Billings.Should().NotBeNullOrEmpty().And.AllSatisfy(billing =>
        {
            billing.Id.Should().NotBeEmpty();
            billing.BarberName.Should().NotBeNullOrEmpty();
            billing.ClientName.Should().NotBeNullOrEmpty();
            billing.ServiceName.Should().NotBeNullOrEmpty();
            billing.Date.Should().NotBeAfter(DateTime.Today);
            billing.Status.Should().Be(Status.Paid);
            billing.Amount.Should().BeGreaterThan(0);
        });
    }
    
    private GetAllBillingsUseCase CreateUseCase(List<Billing> billings)
    {
        return new GetAllBillingsUseCase(
            new BillingsReadOnlyRepositoryBuilder().GetAllFiltered(billings).Build(),
            MapperBuilder.Build());
    }
}