using AutoMapper;
using NDISS.UserRebate.API.Domain;
using NDISS.UserRebate.API.DTOs;
using NDISS.UserRebate.API.Enums;
using NDISS.UserRebate.API.Repository;

namespace NDISS.UserRebate.API.Service
{
    public class UserRebateService : IUserRebateService
    {
        public readonly IUserRebateRepository _userRebateRepo;
        public readonly ILogger<UserRebateService> _logger;
        public readonly IMapper _mapper;
        public readonly IRebateEventRepository _rebateEventRepo;

        public UserRebateService(
            IUserRebateRepository userRebateRepo,
            IRebateEventRepository rebateEventRepo,
            ILogger<UserRebateService> logger,
            IMapper mapper)
        {
            _userRebateRepo = userRebateRepo;
            _rebateEventRepo = rebateEventRepo;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SaveUserRebateAndEventAsync(string userId, UserRebateCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Start saving UserRebate for UserId: {UserId}", userId);

                var userRebate = await _userRebateRepo.GetUserRebateByUserIdAsync(userId);
                if (userRebate == null)
                {
                    userRebate = _mapper.Map<Domain.UserRebate>(dto);
                    userRebate.UserId = userId;
                    userRebate.UserRebateId = Guid.NewGuid().ToString();
                    userRebate.AccessToken = dto.AccessToken;
                    userRebate.RefreshToken = dto.RefreshToken;

                    await _userRebateRepo.AddUserRebateAsync(userRebate);
                }
                else
                {
                    _mapper.Map(dto, userRebate);
                    userRebate.UpdateAt = DateTime.UtcNow;
                    userRebate.ExpiredAt = dto.VerifiedAt.AddYears(1);
                    userRebate.AccessToken = dto.AccessToken;
                    userRebate.RefreshToken = dto.RefreshToken;
                }

                var rebateEvent = new RebateEvent
                {
                    RebateEventId = Guid.NewGuid().ToString(),
                    RebateEventTypeId = dto.RebateEventTypeId,
                    UserRebateId = userRebate.UserRebateId,
                    SyncStatus = RebateEventSyncStatus.Success,
                    EventTimestamp = DateTime.UtcNow
                };

                await _rebateEventRepo.AddRebateEventAsync(rebateEvent);

                _logger.LogInformation("RebateEvent saved for UserId: {UserId}, EventType: {EventType}", userId, dto.RebateEventTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save UserRebate and RebateEvent for UserId: {UserId}", userId);
                throw; // 保持异常冒泡，交由 Controller 统一处理
            }
        }
    }

}
