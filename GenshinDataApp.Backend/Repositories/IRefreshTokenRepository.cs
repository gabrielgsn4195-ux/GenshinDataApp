using GenshinDataApp.Backend.Entities;

namespace GenshinDataApp.Backend.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
    Task<RefreshToken?> GetActiveTokenByUserIdAsync(int userId, CancellationToken ct = default);
    Task<int> CreateAsync(RefreshToken refreshToken, CancellationToken ct = default);
    Task RevokeByTokenAsync(string token, CancellationToken ct = default);
    Task RevokeAllByUserIdAsync(int userId, CancellationToken ct = default);
}
