using BarberBoss.Application.UseCases.Billings.Reports.Pdf;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Billings.Reports.Pdf;

public class GenerateBillingsReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var billings = BillingBuilder.Collection();
        var useCase = CreateUseCase(billings);
        
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));
        
        result.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task SuccessEmpty()
    {
        var useCase = CreateUseCase([]);
        
        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.Should().BeEmpty();
    }
    
    private GenerateBillingsReportPdfUseCase CreateUseCase(List<Billing> billings)
    {
        return new GenerateBillingsReportPdfUseCase(
            new BillingsReadOnlyRepositoryBuilder().FilterByDate(billings).Build());
    }
}