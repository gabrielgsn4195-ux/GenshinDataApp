using GenshinDataApp.Backend.DTOs.Auth;
using GenshinDataApp.Backend.Entities;
using GenshinDataApp.Backend.Repositories;
using GenshinDataApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace GenshinDataApp.Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtHelper _jwtHelper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtHelper jwtHelper,
        IPasswordHasher passwordHasher,
        ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtHelper = jwtHelper;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || 
                string.IsNullOrWhiteSpace(request.Password) || 
                string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest(new { message = "Email, password, and username are required" });
            }

            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email, ct);
            if (existingUser != null)
            {
                return Conflict(new { message = "User with this email already exists" });
            }

            // Generate unique UserCode
            // Multiple users can have the same Username as long as UserCode differs (like Discord discriminators)
            var userCode = GenerateUserCode();
            var maxAttempts = 100;
            var attempts = 0;

            while (attempts < maxAttempts)
            {
                // Check if Username + UserCode combination exists
                var combinedCheck = $"{request.Username}#{userCode}";
                // For now, just generate unique codes. The SP will validate uniqueness on insert.
                var codeExists = await _userRepository.GetByUserCodeAsync(userCode, ct);
                if (codeExists == null)
                {
                    break;
                }
                userCode = GenerateUserCode();
                attempts++;
            }

            if (attempts >= maxAttempts)
            {
                return StatusCode(500, new { message = "Could not generate unique UserCode" });
            }

            // Hash password
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // Create user
            var user = new User
            {
                Email = request.Email.ToLowerInvariant(),
                PasswordHash = passwordHash,
                Username = request.Username,
                UserCode = userCode,
                AuthProvider = "local",
                IsEmailVerified = false,
                Role = "User",
                IsActive = true
            };

            int userId;
            try
            {
                userId = await _userRepository.CreateAsync(user, ct);
            }
            catch (Exception ex) when (ex.Message.Contains("Username and UserCode combination already exists"))
            {
                _logger.LogWarning(ex, "Username+UserCode collision for {Username}#{UserCode}", request.Username, userCode);
                return Conflict(new { message = "Username and code combination already exists. Please try again." });
            }

            // Get created user
            var createdUser = await _userRepository.GetByIdAsync(userId, ct);
            if (createdUser == null)
            {
                return StatusCode(500, new { message = "User created but could not be retrieved" });
            }

            // Generate tokens
            var accessToken = _jwtHelper.GenerateAccessToken(
                createdUser.PublicId,
                createdUser.Email,
                createdUser.Username,
                createdUser.Role
            );

            var refreshToken = _jwtHelper.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = createdUser.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                DeviceFingerprint = HttpContext.Connection.RemoteIpAddress?.ToString(),
                IsActive = true
            };

            await _refreshTokenRepository.CreateAsync(refreshTokenEntity, ct);

            var response = new AuthResponse
            {
                PublicId = createdUser.PublicId,
                Email = createdUser.Email,
                Username = createdUser.Username,
                UserCode = createdUser.UserCode,
                Role = createdUser.Role,
                IsEmailVerified = createdUser.IsEmailVerified,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            _logger.LogInformation("User {Email} registered successfully with PublicId {PublicId}", createdUser.Email, createdUser.PublicId);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            // Find user
            var user = await _userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), ct);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Account is deactivated" });
            }

            // Verify password
            if (user.PasswordHash == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Generate tokens
            var accessToken = _jwtHelper.GenerateAccessToken(
                user.PublicId,
                user.Email,
                user.Username,
                user.Role
            );

            var refreshToken = _jwtHelper.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                DeviceFingerprint = HttpContext.Connection.RemoteIpAddress?.ToString(),
                IsActive = true
            };

            await _refreshTokenRepository.CreateAsync(refreshTokenEntity, ct);

            var response = new AuthResponse
            {
                PublicId = user.PublicId,
                Email = user.Email,
                Username = user.Username,
                UserCode = user.UserCode,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            _logger.LogInformation("User {Email} logged in successfully", user.Email);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.AccessToken) || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new { message = "AccessToken and RefreshToken are required" });
            }

            // Validate expired access token
            var principal = _jwtHelper.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return Unauthorized(new { message = "Invalid access token" });
            }

            // Buscar claim sub (puede estar como "sub" o mapeado a NameIdentifier)
            var publicIdClaim = principal.Claims.FirstOrDefault(c => 
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || 
                c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (publicIdClaim == null || !Guid.TryParse(publicIdClaim, out var publicId))
            {
                return Unauthorized(new { message = "Invalid token claims" });
            }

            // Find user
            var user = await _userRepository.GetByPublicIdAsync(publicId, ct);
            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { message = "User not found or inactive" });
            }

            // Validate refresh token
            var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, ct);
            if (storedRefreshToken == null || 
                storedRefreshToken.UserId != user.Id || 
                !storedRefreshToken.IsActive || 
                storedRefreshToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            // Revoke old refresh token
            await _refreshTokenRepository.RevokeByTokenAsync(request.RefreshToken, ct);

            // Generate new tokens
            var newAccessToken = _jwtHelper.GenerateAccessToken(
                user.PublicId,
                user.Email,
                user.Username,
                user.Role
            );

            var newRefreshToken = _jwtHelper.GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                DeviceFingerprint = HttpContext.Connection.RemoteIpAddress?.ToString(),
                IsActive = true
            };

            await _refreshTokenRepository.CreateAsync(newRefreshTokenEntity, ct);

            var response = new AuthResponse
            {
                PublicId = user.PublicId,
                Email = user.Email,
                Username = user.Username,
                UserCode = user.UserCode,
                Role = user.Role,
                IsEmailVerified = user.IsEmailVerified,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            _logger.LogInformation("User {Email} refreshed token successfully", user.Email);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, new { message = "An error occurred during token refresh" });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                await _refreshTokenRepository.RevokeByTokenAsync(request.RefreshToken, ct);
            }

            _logger.LogInformation("User logged out successfully");

            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, new { message = "An error occurred during logout" });
        }
    }

    private static string GenerateUserCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new char[8];
        
        for (int i = 0; i < random.Length; i++)
        {
            random[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
        }
        
        return new string(random);
    }
}
