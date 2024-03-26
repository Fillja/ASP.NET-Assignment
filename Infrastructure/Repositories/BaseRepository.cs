using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) where TEntity : class
{
    private readonly DataContext _context = context;

    public virtual async Task<ResponseResult> CreateAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return ResponseFactory.Ok(entity, "Created Successfully.");
        }
        catch (Exception ex) 
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public virtual async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            IEnumerable<TEntity> list = await _context.Set<TEntity>().ToListAsync();

            if (!list.Any())
                return ResponseFactory.NotFound("List is empty.");
                
            return ResponseFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public virtual async Task<ResponseResult> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);

            if (entity == null)
                return ResponseFactory.NotFound();
                
            return ResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public virtual async Task<ResponseResult> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var exists = await ExistsAsync(predicate);

            if (exists.StatusCode == StatusCode.EXISTS)
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok(entity, "Successfully updated.");
            }

            else if (exists.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound("Could not find and update.");

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public virtual async Task<ResponseResult> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = await GetOneAsync(predicate);

            if(result.StatusCode == StatusCode.OK)
            {
                var entity = (TEntity)result.ContentResult!;
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();

                return ResponseFactory.Ok("Successfully deleted.");
            }

            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();

            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public virtual async Task<ResponseResult> ExistsAsync (Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var result = await _context.Set<TEntity>().AnyAsync(predicate);
            if (result)
                return ResponseFactory.Exists();
            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
