using AutoMapper;
using NDISS.Service.API.Domain;
using NDISS.Service.API.DTOs;
using NDISS.Service.API.DTOs.Category;
using NDISS.Service.API.DTOs.ProviderService;
using NDISS.Service.API.DTOs.ServiceType;
using NDISS.Service.API.DTOs.SignlePlan;
using NDISS.Service.API.DTOs.SinglePlan;
using NDISS.Service.API.DTOs.WeeklyPlan;
using NDISServiceAPI.DTO.Item;

namespace NDISS.Service.API.Mappers
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

            CreateMap<ProviderService, ProviderServiceResponseDto>()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src => src.ServiceType))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

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
