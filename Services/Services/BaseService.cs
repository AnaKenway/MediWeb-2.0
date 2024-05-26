using Common;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class BaseService<T> where T : class
{
    protected readonly MediWebContext _context;
    protected readonly DbSet<T> _set;

    public BaseService(MediWebContext context)
    {
        _context = context;
        _set = _context.Set<T>();
    }

    public virtual async Task<IList<T>> GetAllAsync()
    {
        return await _set.ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(long id)
    {
        return await _set.FindAsync(id) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Object with given Id doesn't exist.");
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }


    public virtual async Task DeleteAsync(T entity)
    {
        _set.Remove(entity);
        await _context.SaveChangesAsync();
    }

}
