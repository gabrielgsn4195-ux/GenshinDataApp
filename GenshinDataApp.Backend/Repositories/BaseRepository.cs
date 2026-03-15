using GenshinDataApp.Backend.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GenshinDataApp.Backend.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly GenshinDbContext _context;
    protected abstract string EntityName { get; }

    protected BaseRepository(GenshinDbContext context)
    {
        _context = context;
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var publicIdParam = new SqlParameter("@Id", id);
        var result = await _context.Set<TEntity>()
            .FromSqlRaw($"EXEC SP_S_{EntityName}ById @Id", publicIdParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public virtual async Task<TEntity?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default)
    {
        var publicIdParam = new SqlParameter("@PublicId", publicId);
        var result = await _context.Set<TEntity>()
            .FromSqlRaw($"EXEC SP_S_{EntityName}ByPublicId @PublicId", publicIdParam)
            .AsNoTracking()
            .ToListAsync(ct);
        
        return result.FirstOrDefault();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<TEntity>()
            .FromSqlRaw($"EXEC SP_S_{EntityName}All")
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllActiveAsync(CancellationToken ct = default)
    {
        return await _context.Set<TEntity>()
            .FromSqlRaw($"EXEC SP_S_{EntityName}Active")
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public virtual async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
    {
        throw new NotImplementedException("Must be implemented in derived class");
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        throw new NotImplementedException("Must be implemented in derived class");
    }

    public virtual async Task<bool> DeleteAsync(Guid publicId, CancellationToken ct = default)
    {
        var publicIdParam = new SqlParameter("@PublicId", publicId);
        await _context.Database.ExecuteSqlRawAsync($"EXEC SP_D_{EntityName} @PublicId", publicIdParam, ct);
        return true;
    }

    protected async Task<T?> ExecuteScalarAsync<T>(string storedProcedure, params SqlParameter[] parameters)
    {
        using var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = storedProcedure;
        command.CommandType = CommandType.StoredProcedure;
        
        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }

        await _context.Database.OpenConnectionAsync();
        
        try
        {
            var result = await command.ExecuteScalarAsync();
            return result != null && result != DBNull.Value ? (T)result : default;
        }
        finally
        {
            await _context.Database.CloseConnectionAsync();
        }
    }
}
