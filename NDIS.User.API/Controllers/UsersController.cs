using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using NDIS.Shared.Common.Extensions;
using NDIS.User.API.Common.Enums;
using NDIS.User.API.Domain.User;
using NDIS.User.API.DTOs;
using NDIS.User.API.UserService;
using System.Security.Claims;

namespace NDIS.User.API.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;


        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("admin-signup")]
        public async Task<ActionResult<ApiResponse<SignUpResponseDto>>> AdminSignUp([FromBody] SignUpRequestDto dto)
        {
            var result = await _userService.RegisterAsync(dto, RoleNames.Admin.ToString());
            return ApiResponse<SignUpResponseDto>.Success(result, "Admin registered successfully");
        }

        [HttpPost("user-signup")]
        public async Task<ActionResult<ApiResponse<SignUpResponseDto>>> UserSignUp([FromBody] SignUpRequestDto dto)
        {
            var result = await _userService.RegisterAsync(dto, RoleNames.User.ToString());
            return ApiResponse<SignUpResponseDto>.Success(result, "User registered successfully");
        }


        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserInfo(string userId, [FromBody] UpdateUserRequestDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(ApiResponse<UserInfoResponseDto>.Fail("User ID is required", "400"));
                }

                var updatedUser = await _userService.UpdateUserInfoAsync(userId, dto);

                return Ok(ApiResponse<UserInfoResponseDto>.Success(updatedUser, "User profile updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<UserInfoResponseDto>.Fail(ex.Message, "404"));
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ApiResponse<UserInfoResponseDto>.Fail(ex.Message, "400"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating user profile.");
                return StatusCode(500, ApiResponse<UserInfoResponseDto>.Fail("An error occurred while updating user info", "500"));
            }
        }

        [HttpPost("signin")]
        public async Task<ActionResult<ApiResponse<SignInResponseDto>>> SignIn([FromBody] SignInRequestDto request)
        {
            var result = await _userService.SignInAsync(request);
            return ApiResponse<SignInResponseDto>.Success(result, "User signed in successfully");
        }
        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetCurrentUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);         
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<UserInfoDto>.Fail("User ID not found in token.", "401"));
            }
            var userInfo = await _userService.GetCurrentUserInfoAsync(userId);
            return ApiResponse<UserInfoDto>.Success(userInfo, "User profile retrieved successfully");
        } 
  }
}