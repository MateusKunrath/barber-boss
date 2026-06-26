using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Billings.Register;

public class RegisterBillingUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestBillingJsonBuilder.Build();
        var useCase = CreateUseCase();
        
        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ErrorBarberNameEmpty()
    {
        var request = RequestBillingJsonBuilder.Build();
        request.BarberName = string.Empty;
        
        var useCase = CreateUseCase();
        
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.BARBER_NAME_REQUIRED));
    }

    private RegisterBillingUseCase CreateUseCase()
    {
        return new RegisterBillingUseCase(
            BillingsWriteOnlyRepositoryBuilder.Build(),
            UnitOfWorkBuilder.Build(),
            MapperBuilder.Build());
    }
}