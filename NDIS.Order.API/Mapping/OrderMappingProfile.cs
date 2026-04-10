using AutoMapper;
using NDIS.Order.API.Domain;
using NDIS.Order.API.Dtos;
using OrderEntity = NDIS.Order.API.Domain.Entities.Order;

namespace NDIS.Order.API.Mappings
{
  public class OrderMappingProfile : Profile
  {
    public OrderMappingProfile()
    {
      CreateMap<OrderEntity, OrderResponseDto>()
          .ForMember(dest => dest.ProviderName,
              opt => opt.MapFrom(src => src.ProviderServiceName))
          .ForMember(dest => dest.OrderStatus,
              opt => opt.MapFrom(src => src.OrderStatus.ToString()));
    }
  }
}