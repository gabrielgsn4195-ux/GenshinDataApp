using GenshinDataApp.Backend.Data;
using GenshinDataApp.Backend.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GenshinDataApp.Backend.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    protected override string EntityName => "User";

    public UserRepository(GenshinDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        
        var emailParam = new SqlParameter("@Email", email);
        var result = await _context.Users
            .FromSqlRaw("EXEC SP_S_UserByEmail @Email", emailParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(username);
        
        var usernameParam = new SqlParameter("@Username", username);
        var result = await _context.Users
            .FromSqlRaw("EXEC SP_S_UserByUsername @Username", usernameParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<User?> GetByUserCodeAsync(string userCode, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(userCode);
        
        var userCodeParam = new SqlParameter("@UserCode", userCode);
        var result = await _context.Users
            .FromSqlRaw("EXEC SP_S_UserByUserCode @UserCode", userCodeParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<User?> GetByAuthProviderAsync(string authProvider, string authProviderId, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(authProvider);
        ArgumentNullException.ThrowIfNull(authProviderId);
        
        var providerParam = new SqlParameter("@AuthProvider", authProvider);
        var providerIdParam = new SqlParameter("@AuthProviderId", authProviderId);
        var result = await _context.Users
            .FromSqlRaw("EXEC SP_S_UserByAuthProvider @AuthProvider, @AuthProviderId", providerParam, providerIdParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        var tokenParam = new SqlParameter("@Token", token);
        var result = await _context.Users
            .FromSqlRaw("EXEC SP_S_UserByEmailVerificationToken @Token", tokenParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        var tokenParam = new SqlParameter("@Token", token);
        var result = await _context.Users
            .FromSqlRaw("EXEC SP_S_UserByPasswordResetToken @Token", tokenParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public async Task<int> CreateAsync(User user, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        var parameters = new[]
        {
            new SqlParameter("@Email", user.Email),
            new SqlParameter("@PasswordHash", (object?)user.PasswordHash ?? DBNull.Value),
            new SqlParameter("@Username", user.Username),
            new SqlParameter("@UserCode", user.UserCode),
            new SqlParameter("@AuthProvider", user.AuthProvider),
            new SqlParameter("@AuthProviderId", (object?)user.AuthProviderId ?? DBNull.Value),
            new SqlParameter("@AvatarPath", (object?)user.AvatarPath ?? DBNull.Value),
            new SqlParameter("@Role", user.Role)
        };

        var publicIdResult = await _context.Database
            .SqlQueryRaw<PublicIdResult>("EXEC SP_I_User @Email, @PasswordHash, @Username, @UserCode, @AuthProvider, @AuthProviderId, @AvatarPath, @Role", parameters)
            .ToListAsync(ct);

        var publicId = publicIdResult.FirstOrDefault()?.PublicId 
            ?? throw new InvalidOperationException("User creation failed - no PublicId returned");

        var createdUser = await GetByPublicIdAsync(publicId, ct);
        return createdUser?.Id ?? throw new InvalidOperationException("User was created but could not be retrieved");
    }

    public async Task<bool> UpdateProfileAsync(Guid publicId, string? username, string? avatarPath, CancellationToken ct = default)
    {
        var parameters = new[]
        {
            new SqlParameter("@PublicId", publicId),
            new SqlParameter("@Username", (object?)username ?? DBNull.Value),
            new SqlParameter("@AvatarPath", (object?)avatarPath ?? DBNull.Value)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_UserProfile @PublicId, @Username, @AvatarPath",
            parameters,
            ct);

        return true;
    }

    public async Task<bool> UpdatePasswordAsync(Guid publicId, string passwordHash, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        
        var parameters = new[]
        {
            new SqlParameter("@PublicId", publicId),
            new SqlParameter("@PasswordHash", passwordHash)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_UserPassword @PublicId, @PasswordHash",
            parameters,
            ct);

        return true;
    }

    public async Task<bool> SetEmailVerificationTokenAsync(Guid publicId, string token, DateTime expiry, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        var parameters = new[]
        {
            new SqlParameter("@PublicId", publicId),
            new SqlParameter("@Token", token),
            new SqlParameter("@Expiry", expiry)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_UserEmailVerificationToken @PublicId, @Token, @Expiry",
            parameters,
            ct);

        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        var tokenParam = new SqlParameter("@Token", token);
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_UserVerifyEmail @Token",
            tokenParam,
            ct);

        return true;
    }

    public async Task<bool> SetPasswordResetTokenAsync(Guid publicId, string token, DateTime expiry, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        
        var parameters = new[]
        {
            new SqlParameter("@PublicId", publicId),
            new SqlParameter("@Token", token),
            new SqlParameter("@Expiry", expiry)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_UserPasswordResetToken @PublicId, @Token, @Expiry",
            parameters,
            ct);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string passwordHash, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(passwordHash);
        
        var parameters = new[]
        {
            new SqlParameter("@Token", token),
            new SqlParameter("@PasswordHash", passwordHash)
        };

        await _context.Database.ExecuteSqlRawAsync(
            "EXEC SP_U_UserResetPassword @Token, @PasswordHash",
            parameters,
            ct);

        return true;
    }
}

internal class PublicIdResult
{
    public Guid PublicId { get; set; }
}
