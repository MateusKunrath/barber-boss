using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billings.GetAll;

public class GetAllBillingsUseCase(IBillingsReadOnlyRepository repository, IMapper mapper): IGetAllBillingsUseCase
{
    public async Task<ResponseBillingsJson> Execute()
    {
        var result = await repository.GetAll();
        return new ResponseBillingsJson
        {
            Billings = mapper.Map<List<ResponseShortBillingJson>>(result)
        };
    }
}