namespace GenshinDataApp.Backend.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TEntity?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<TEntity>> GetAllActiveAsync(CancellationToken ct = default);
    Task<int> AddAsync(TEntity entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid publicId, CancellationToken ct = default);
}
