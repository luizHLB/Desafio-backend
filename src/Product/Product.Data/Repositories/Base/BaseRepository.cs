using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO;
using Npgsql;
using Product.Data.Contexts;
using Product.Domain.DTO;
using Product.Domain.Entities.Base;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Repositories;
using System.Data;
using System.Linq.Expressions;

namespace Product.Data.Repositories.Base
{
    public abstract class BaseRepository<T, TT> : IBaseRepository<T, TT> where T : BaseEntity where TT : class
    {
        protected readonly ProductContext _context;

        public BaseRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is PostgresException && !string.IsNullOrEmpty(((PostgresException)e.InnerException).ConstraintName))
                    throw new EntityConstraintException(((PostgresException)e.InnerException).ConstraintName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PagedListDTO<TT>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize)
        {
            var query = Query(expression);
            query = Filter(query);

            var itens = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var currentPage = pageNumber;
            var hasPrevious = currentPage > 1;
            var hasNext = currentPage < totalPages;

            return PagedListDTO<TT>.ToPagedList(Cast(itens), totalCount, totalPages, currentPage, pageSize, hasPrevious, hasNext);
        }

        public abstract List<TT> Cast(List<T> itens);

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> requiredExpression) =>
            _context.Set<T>().AsNoTracking().Where(requiredExpression);

        public virtual IQueryable<T> Filter(IQueryable<T> entity) => entity;

        public virtual async Task Remove(long id)
        {
            var entity = await GetById(id);
            if (entity is null)
                throw new RecordNotFoundException();

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual void Update(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Detached;
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is PostgresException && !string.IsNullOrEmpty(((PostgresException)e.InnerException).ConstraintName))
                    throw new EntityConstraintException(((PostgresException)e.InnerException).ConstraintName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<T> GetById(long id) => await _context.Set<T>().FirstOrDefaultAsync(f => f.Id.Equals(id));
    }
}
