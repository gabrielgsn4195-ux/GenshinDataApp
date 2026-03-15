namespace GenshinDataApp.Backend.DTOs.User;

public class UserDto
{
    public Guid PublicId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string UserCode { get; set; } = string.Empty;
    public string? AvatarPath { get; set; }
    public bool IsEmailVerified { get; set; }
    public string Role { get; set; } = string.Empty;
    public string AuthProvider { get; set; } = string.Empty;
}
