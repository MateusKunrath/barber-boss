using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billings.GetAll;

public class GetAllBillingsUseCase(IBillingsReadOnlyRepository repository, IMapper mapper): IGetAllBillingsUseCase
{
    public async Task<ResponseBillingsJson> Execute(RequestGetBillingsJson request)
    {
        var (result, totalCount) = await repository.GetAllFiltered(mapper.Map<GetBillingsFilters>(request));
        return new ResponseBillingsJson
        {
            Billings = mapper.Map<List<ResponseShortBillingJson>>(result),
            TotalCount = totalCount,
            Page =  request.Page,
            PageSize =  request.PageSize,
        };
    }
}