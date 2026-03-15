using System.Security.Claims;

namespace GenshinDataApp.Infrastructure.Security;

public interface IJwtHelper
{
    string GenerateAccessToken(Guid publicId, string email, string username, string role);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
