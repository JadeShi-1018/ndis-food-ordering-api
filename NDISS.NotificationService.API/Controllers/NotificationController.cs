using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDIS.Shared.Common.Models;
using NDISS.NotificationService.API.DTOs;
using NDISS.NotificationService.API.Service;

namespace NDISS.NotificationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateDto dto)
        {
            if (dto == null)
            {
                _logger.LogError("Notification creation failed: DTO is null.");
                return BadRequest(ApiResponse<string>.Fail("Invalid notification data.", "400"));
            }

            await _notificationService.CreateNotificationAsync(dto);
            _logger.LogInformation("Notification created successfully.");

            return Ok(ApiResponse<string>.Success("Notification created successfully."));
        }
    }
}
