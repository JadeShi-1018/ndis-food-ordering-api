using AutoMapper;
using NDISS.UserRebate.API.DTOs;

namespace NDISS.UserRebate.API
{
    public class MappingProfile : Profile   
    {
        public MappingProfile()
        {

            CreateMap<UserRebateCreateDto, Domain.UserRebate>()
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ExpiredAt, opt => opt.MapFrom(src => src.VerifiedAt.AddYears(1)))//根据业务改变
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => 1))  // 1 表示 Verified 状态（你可用枚举替代）
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UserRebateId, opt => opt.Ignore());

        }
    }
}
