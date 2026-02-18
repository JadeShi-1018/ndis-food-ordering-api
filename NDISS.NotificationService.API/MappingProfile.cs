using AutoMapper;
using NDISS.NotificationService.API.Domain;
using NDISS.NotificationService.API.DTOs;

namespace NDISS.NotificationService.API
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<NotificationCreateDto, Notification>();
        }
    }
}
