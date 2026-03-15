using GenshinDataApp.Backend.DTOs.User;
using GenshinDataApp.Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenshinDataApp.Services.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    // Rutas específicas primero
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken ct)
    {
        try
        {
            // Buscar claim sub (puede estar como "sub" o mapeado a NameIdentifier)
            var publicIdClaim = User.Claims.FirstOrDefault(c => 
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || 
                c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

            if (publicIdClaim == null || !Guid.TryParse(publicIdClaim, out var publicId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var user = await _userRepository.GetByPublicIdAsync(publicId, ct);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = new UserDto
            {
                PublicId = user.PublicId,
                Email = user.Email,
                Username = user.Username,
                UserCode = user.UserCode,
                AvatarPath = user.AvatarPath,
                IsEmailVerified = user.IsEmailVerified,
                Role = user.Role,
                AuthProvider = user.AuthProvider
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current user");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("{publicId:guid}")]
    public async Task<IActionResult> GetUserByPublicId(Guid publicId, CancellationToken ct)
    {
        try
        {
            var user = await _userRepository.GetByPublicIdAsync(publicId, ct);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = new UserDto
            {
                PublicId = user.PublicId,
                Email = user.Email,
                Username = user.Username,
                UserCode = user.UserCode,
                AvatarPath = user.AvatarPath,
                IsEmailVerified = user.IsEmailVerified,
                Role = user.Role,
                AuthProvider = user.AuthProvider
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {PublicId}", publicId);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("GetAllUsers - Starting request");

            var users = await _userRepository.GetAllActiveAsync(ct);
            _logger.LogInformation("GetAllUsers - Retrieved {Count} users from repository", users.Count());

            var userDtos = users.Select(u => new UserDto
            {
                PublicId = u.PublicId,
                Email = u.Email,
                Username = u.Username,
                UserCode = u.UserCode,
                AvatarPath = u.AvatarPath,
                IsEmailVerified = u.IsEmailVerified,
                Role = u.Role,
                AuthProvider = u.AuthProvider
            }).ToList();

            _logger.LogInformation("GetAllUsers - Returning {Count} DTOs", userDtos.Count);
            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
