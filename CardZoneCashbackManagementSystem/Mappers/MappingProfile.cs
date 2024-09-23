using AutoMapper;
using CardZoneCashbackManagementSystem.Models;
using CardZoneCashbackManagementSystem.Models.Requests;

namespace CardZoneCashbackManagementSystem.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCardRequest, Card>()
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreateTransactionRequest, Transaction>()
            .ForMember(tx => tx.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}