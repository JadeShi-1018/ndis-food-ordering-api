using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NDIS.Shared.Common.Models;
using NDISS.UserRebate.API.DTOs;
using NDISS.UserRebate.API.Service;

namespace NDISS.UserRebate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRebateController : ControllerBase
    {
        private readonly ILogger<UserRebateController> _logger;
        private readonly IUserRebateService _userRebateService;

        public UserRebateController(IUserRebateService userRebateService, ILogger<UserRebateController> logger)
        {
            _userRebateService = userRebateService;
            _logger = logger;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] UserRebateCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access attempt to rebate verify endpoint.");
                return Unauthorized(ApiResponse<string>.Fail("Unauthorized", "401"));
            }

            await _userRebateService.SaveUserRebateAndEventAsync(userId, dto);

            _logger.LogInformation("UserRebate and RebateEvent saved successfully for UserId: {UserId}", userId);
            return Ok(ApiResponse<string>.Success("Rebate saved and event logged."));
        }
    }
}