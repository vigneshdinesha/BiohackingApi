using Microsoft.EntityFrameworkCore;
using BiohackingApi.Web.Data;

namespace BiohackingApi.Web.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly BiohackingDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(BiohackingDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T?> UpdateAsync(int id, T entity)
    {
        var existingEntity = await _dbSet.FindAsync(id);
        if (existingEntity == null)
            return null;

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return existingEntity;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}
