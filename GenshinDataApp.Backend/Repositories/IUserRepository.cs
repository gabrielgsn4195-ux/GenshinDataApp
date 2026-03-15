using GenshinDataApp.Backend.Entities;

namespace GenshinDataApp.Backend.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<User?> GetByUserCodeAsync(string userCode, CancellationToken ct = default);
    Task<User?> GetByAuthProviderAsync(string authProvider, string authProviderId, CancellationToken ct = default);
    Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken ct = default);
    Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct = default);
    Task<int> CreateAsync(User user, CancellationToken ct = default);
    Task<bool> UpdateProfileAsync(Guid publicId, string? username, string? avatarPath, CancellationToken ct = default);
    Task<bool> UpdatePasswordAsync(Guid publicId, string passwordHash, CancellationToken ct = default);
    Task<bool> SetEmailVerificationTokenAsync(Guid publicId, string token, DateTime expiry, CancellationToken ct = default);
    Task<bool> VerifyEmailAsync(string token, CancellationToken ct = default);
    Task<bool> SetPasswordResetTokenAsync(Guid publicId, string token, DateTime expiry, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(string token, string passwordHash, CancellationToken ct = default);
}
