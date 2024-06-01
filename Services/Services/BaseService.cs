﻿using Common;
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
        id.AssertIsNotNull();
        id.AssertIsNotZero();

        return await _set.FindAsync(id) ??
            throw new MediWebClientException(MediWebFeature.CRUD, "Object with given Id doesn't exist.");
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.AssertIsNotNull();

        await _set.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        entity.AssertIsNotNull();

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }


    public virtual async Task<bool> DeleteAsync(T entity)
    {
        entity.AssertIsNotNull();

        _set.Remove(entity);
        var result = await _context.SaveChangesAsync();

        if(result > 0) 
        { 
            return true; 
        }
        return false;
    }

    public virtual async Task<bool> DeleteAsync(long id)
    {
        id.AssertIsNotNull();
        id.AssertIsNotZero();

        var entity = await _set.FindAsync(id) 
            ?? throw new Exception("Cannot delete the object with Id " +  id + "because the object with that Id could not be found.");

        _set.Remove(entity);
        var result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return true;
        }
        return false;
    }

}
