using NDIS.Order.API.Dtos;

namespace NDIS.Order.API.ServiceClient
{
  public interface IUserServiceClient
  {
    Task<UserProfileDto?> GetUserByIdAsync(string token);
  
}
}
