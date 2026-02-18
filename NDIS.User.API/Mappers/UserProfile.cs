using AutoMapper;
using NDIS.User.API.DTOs;
using AppUser = NDIS.User.API.Domain.User.User;

namespace NDIS.User.API.Mappers
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<SignUpRequestDto, NDIS.User.API.Domain.User.User>();
      CreateMap<NDIS.User.API.Domain.User.User, SignUpRequestDto>();
      CreateMap<AppUser, UserInfoDto>()
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));
        }
  }
}
