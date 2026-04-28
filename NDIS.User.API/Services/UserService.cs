using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NDIS.User.API.Common.Enums;
using NDIS.User.API.Domain.User;
using NDIS.User.API.DTOs;
using NDIS.User.API.Exceptions;
using NDIS.User.API.Services;
using NDIS.User.API.UserReposotory;
using System.Security.Claims;
using AppUser = NDIS.User.API.Domain.User.User;

namespace NDIS.User.API.UserService
{
    public class UserService : IUserService
    {
    private readonly IUserRepository _userRepository;
    private readonly UserManager<Domain.User.User> _userManager;
    private readonly IMapper _mapper;
    private readonly RoleManager<Role> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
                           UserManager<Domain.User.User> userManager,
                           IMapper mapper,
                           RoleManager<Role> roleManager,
                           ITokenService tokenService,
                           ILogger<UserService> logger)
    {
      _userRepository = userRepository;
      _userManager = userManager;
      _mapper = mapper;
      _roleManager = roleManager;
      _tokenService = tokenService;
      _logger = logger;
    }

    public async Task<SignUpResponseDto?> RegisterAsync(SignUpRequestDto signUpRequestDto, string roleName)
    {
      var existingUser = await _userManager.FindByEmailAsync(signUpRequestDto.Email);
      if (existingUser != null)
        throw new InvalidOperationException("A user with this email already exists.");

      var user = _mapper.Map<NDIS.User.API.Domain.User.User>(signUpRequestDto);
      var result = await _userManager.CreateAsync(user, signUpRequestDto.Password);
      if (!result.Succeeded)
      {
        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        throw new ApplicationException($"User creation failed: {errors}");
      }

      if (!await _roleManager.RoleExistsAsync(roleName))
        await _roleManager.CreateAsync(new Role { Name = roleName });

      await _userManager.AddToRoleAsync(user, roleName);

      return new SignUpResponseDto { Email = user.Email };

    }
    public async Task<SignInResponseDto> SignInAsync(SignInRequestDto request)
    {        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        var token = await _tokenService.GenerateTokenAsync(user);

        return new SignInResponseDto
        {
            Token = token,
            Email = user.Email,
            UserId = user.Id
        };    
     }
    public async Task<UserInfoDto> GetCurrentUserInfoAsync(string userId)
    {
        _logger.LogInformation($"Looking for user ID: '{userId}'");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }
        var userInfo = _mapper.Map<UserInfoDto>(user);
        return userInfo;
    }
      
    public async Task<UserInfoResponseDto> UpdateUserInfoAsync(string userId, UpdateUserRequestDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        user.UserName = dto.Name;
        user.Email = dto.Email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new ApplicationException("User update failed.");

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Unknown";

        return new UserInfoResponseDto
        {
            UserId = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Role = role
        };
    }

    public async Task<ApiResponse<SignUpResponseDto>> RegisterAsync(SignUpRequestDto dto)
    {
      if (dto.Password != dto.ConfirmPassword)
      {
        return ApiResponse<SignUpResponseDto>.Fail("Passwords do not match.", "PASSWORD_MISMATCH");
      }

      var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

      if (existingUser != null)
      {
        return ApiResponse<SignUpResponseDto>.Fail("Email already exists.", "EMAIL_EXISTS");
      }

      var user = new AppUser
      {
        UserName = dto.Email,
        Email = dto.Email,
        PhoneNumber = dto.PhoneNumber,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      var result = await _userRepository.CreateAsync(user, dto.Password);

      if (!result.Succeeded)
      {
        var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
        return ApiResponse<SignUpResponseDto>.Fail(errorMessage, "CREATE_USER_FAILED");
      }

      var roleResult = await _userRepository.AddToRoleAsync(user, "User");

      if (!roleResult.Succeeded)
      {
        var errorMessage = string.Join("; ", roleResult.Errors.Select(e => e.Description));
        return ApiResponse<SignUpResponseDto>.Fail(errorMessage, "ADD_ROLE_FAILED");
      }

      var token = await _tokenService.GenerateTokenAsync(user);

      var response = new SignUpResponseDto
      {
        UserId = user.Id,
        Email = user.Email!,
        Token = token
      };

      return ApiResponse<SignUpResponseDto>.Success(response, "Register successfully.");
    }
  }
}
