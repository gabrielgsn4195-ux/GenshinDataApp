using GenshinDataApp.Backend.Data;
using GenshinDataApp.Backend.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GenshinDataApp.Backend.Repositories;

public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    protected override string EntityName => "RefreshToken";

    public RefreshTokenRepository(GenshinDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        var tokenParam = new SqlParameter("@Token", token);
        var result = await _context.RefreshTokens
            .FromSqlRaw("EXEC SP_S_RefreshTokenByToken @Token", tokenParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<RefreshToken?> GetActiveTokenByUserIdAsync(int userId, CancellationToken ct = default)
    {
        var userIdParam = new SqlParameter("@UserId", userId);
        var result = await _context.RefreshTokens
            .FromSqlRaw("EXEC SP_S_RefreshTokenActiveByUserId @UserId", userIdParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<int> CreateAsync(RefreshToken refreshToken, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(refreshToken);

        var parameters = new[]
        {
            new SqlParameter("@UserId", refreshToken.UserId),
            new SqlParameter("@Token", refreshToken.Token),
            new SqlParameter("@ExpiresAt", refreshToken.ExpiresAt),
            new SqlParameter("@DeviceFingerprint", (object?)refreshToken.DeviceFingerprint ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<RefreshTokenCreateResult>("EXEC SP_I_RefreshToken @UserId, @Token, @ExpiresAt, @DeviceFingerprint", parameters)
            .ToListAsync(ct);

        var publicId = result.FirstOrDefault()?.PublicId 
            ?? throw new InvalidOperationException("RefreshToken creation failed - no PublicId returned");

        var createdToken = await GetByTokenAsync(refreshToken.Token, ct);
        return createdToken?.Id ?? throw new InvalidOperationException("RefreshToken was created but could not be retrieved");
    }

    public async Task RevokeByTokenAsync(string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);

        var tokenParam = new SqlParameter("@Token", token);
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_RefreshTokenRevoke @Token",
            new[] { tokenParam },
            ct);
    }

    public async Task RevokeAllByUserIdAsync(int userId, CancellationToken ct = default)
    {
        var userIdParam = new SqlParameter("@UserId", userId);
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_RefreshTokenRevokeAllByUserId @UserId",
            new[] { userIdParam },
            ct);
    }
}

internal class RefreshTokenCreateResult
{
    public Guid PublicId { get; set; }
    public string Token { get; set; } = string.Empty;
}
