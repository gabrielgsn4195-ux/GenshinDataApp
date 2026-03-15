namespace GenshinDataApp.Infrastructure.Security;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 15;
    public string Issuer { get; set; } = "GenshinDataApp";
    public string Audience { get; set; } = "GenshinDataApp.Frontend";
}
