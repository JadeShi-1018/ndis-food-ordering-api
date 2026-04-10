using AutoMapper;
using NDIS.Service.API.Domain;
using NDIS.Service.API.DTOs;
using NDIS.Service.API.DTOs.Category;
using NDIS.Service.API.DTOs.Menu;
using NDIS.Service.API.DTOs.ProviderService;
using NDIS.Service.API.DTOs.ServiceType;
using NDIS.Service.API.DTOs.SignlePlan;
using NDIS.Service.API.DTOs.SinglePlan;
using NDIS.Service.API.DTOs.WeeklyPlan;
using NDISServiceAPI.DTO.Item;

namespace NDIS.Service.API.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // ServiceType
            CreateMap<ServiceType, ServiceTypeResponseDto>();

            CreateMap<ServiceTypeBaseDto, ServiceType>()
                .ForMember(dest => dest.ServiceTypeId, opt => opt.Ignore());

            // Category
            CreateMap<Category, CategoryResponseDto>();

            CreateMap<CategoryBaseDto, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.ProviderServiceId, opt => opt.Ignore());

            // Items
            CreateMap<CreateItemRequestDto, Item>();
            CreateMap<Item, ItemResponseDto>();

            // ProviderServiceLocation
            CreateMap<ProviderServiceLocation, ProviderServiceLocationDto>();

            CreateMap<ProviderServiceLocationCreateDto, ProviderServiceLocation>()
                .ForMember(dest => dest.ProviderServiceLocationId, opt => opt.Ignore());

            CreateMap<ProviderServiceLocationUpdateDto, ProviderServiceLocation>()
                .ForMember(dest => dest.ProviderServiceLocationId, opt => opt.Ignore());

            // ProviderService
            CreateMap<ProviderServiceBaseDto, ProviderService>()
                .ForMember(dest => dest.ProviderServiceId, opt => opt.Ignore());

            CreateMap<ProviderServiceCreateDto, ProviderService>()
                .ForMember(dest => dest.ProviderServiceId, opt => opt.Ignore());
      CreateMap<ProviderService, ProviderServiceDetailDto>()
            .ForMember(dest => dest.ServiceTypeName,
                opt => opt.MapFrom(src => src.ServiceType != null ? src.ServiceType.ServiceTypeName : null))
            .ForMember(dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories));

      CreateMap<Category, CategorySimpleDto>();

      CreateMap<ProviderService, ProviderServiceResponseDto>();
      CreateMap<ServiceType, ServiceTypeResponseDto>();
      CreateMap<Category, CategoryResponseDto>();

      CreateMap<ProviderService, ProviderServiceListDto>()
    .ForMember(dest => dest.ServiceTypeName,
        opt => opt.MapFrom(src => src.ServiceType.ServiceTypeName))
    .ForMember(dest => dest.CategoryCount,
        opt => opt.MapFrom(src => src.Categories.Count))
    .ForMember(dest => dest.ItemCount,
        opt => opt.MapFrom(src => src.Items.Count));




      CreateMap<ProviderService, ProviderServiceResponseDto>()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

      //Menu
      CreateMap<MenuCreateDto, Menu>()
    .ForMember(dest => dest.MenuId, opt => opt.Ignore());

      CreateMap<MenuUpdateDto, Menu>();

      CreateMap<Menu, MenuResponseDto>();
      // WeeklyPlan
      CreateMap<WeeklyPlanCreateDto, WeeklyPlan>()
                .ForMember(dest => dest.WeeklyPlanId, opt => opt.Ignore());

            CreateMap<WeeklyPlan, WeeklyPlanResponseDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            // SinglePlan
            CreateMap<SinglePlanCreateDto, SinglePlan>()
                .ForMember(dest => dest.SinglePlanId, opt => opt.Ignore());

            CreateMap<SinglePlan, SinglePlanResponseDto>()
                //.ForMember(dest => dest.Menu, opt => opt.MapFrom(src => src.Menu))
                .ForMember(dest => dest.WeeklyPlan, opt => opt.MapFrom(src => src.WeeklyPlan));
                //.ForMember(dest => dest.WeekDay, opt => opt.MapFrom(src => src.WeekDay));
        }
    }
}
