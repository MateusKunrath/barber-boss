using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
        CommunicationToDomain();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterUserJson, User>().ForMember(dest => dest.Password, config => config.Ignore());
        CreateMap<RequestBillingJson, Billing>();
    }

    private void EntityToResponse()
    {
        CreateMap<Billing, ResponseRegisteredBillingJson>();
        CreateMap<Billing, ResponseShortBillingJson>();
        CreateMap<Billing, ResponseBillingJson>();

        CreateMap<User, ResponseUserJson>()
            .ForMember(dest => dest.Role, config => config.MapFrom(source => source.Role.RoleToString()));
        CreateMap<User, ResponseUserProfileJson>()
            .ForMember(dest => dest.Role, config => config.MapFrom(source => source.Role.RoleToString()));
    }

    private void CommunicationToDomain()
    {
        CreateMap<RequestGetBillingsJson, GetBillingsFilters>();
    }
}