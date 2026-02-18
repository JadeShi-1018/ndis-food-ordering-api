using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NDISS.NotificationService.API.Data;
using NDISS.NotificationService.API.Domain;
using NDISS.NotificationService.API.DTOs;
using NDISS.NotificationService.API.Enums;
using NDISS.NotificationService.API.Repository;

namespace NDISS.NotificationService.API.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public NotificationService(
            INotificationRepository notificationRepo,
            ILogger<NotificationService> logger,
            IMapper mapper,
            ApplicationDbContext context) //注入 DbContext
        {
            _notificationRepo = notificationRepo;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateNotificationAsync(NotificationCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var notification = _mapper.Map<Notification>(dto);
                notification.NotificationId = Guid.NewGuid().ToString();
                notification.CreatedAt = DateTime.UtcNow;
                await _notificationRepo.AddNotificationAsync(notification);

                var notificationEvent = new NotificationEvent
                {
                    NotificationEventId = Guid.NewGuid().ToString(),
                    NotificationId = notification.NotificationId,
                    NotificationMethod = dto.NotificationMethod,
                    EventStatus = NotificationEventStatus.Pending,
                    EventType = NotificationEventType.Created,
                    SentAt = DateTime.UtcNow,
                    ErrorReason = string.Empty
                };
                await _notificationRepo.AddNotificationEventAsync(notificationEvent);
                await transaction.CommitAsync(); // 都成功才提交

                _logger.LogInformation("Notification and event created successfully. NotificationId: {NotificationId}", notification.NotificationId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to create notification or event.");
                throw;
            }
        }
    }
}
