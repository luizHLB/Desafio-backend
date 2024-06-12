using Microsoft.EntityFrameworkCore;
using Product.Data.Contexts;
using Product.Domain.DTO;
using Product.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Product.Data.Repositories.Base
{
    public class BaseRepository<T> : IBaseRespository<T> where T : class
    {
        protected readonly ProductContext _context;

        public BaseRepository(ProductContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public async Task<PagedListDTO<T>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            var query = Query(expression);
            query = Filter(query);

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var currentPage = pageNumber;
            var hasPrevious = currentPage > 1;
            var hasNext = currentPage < totalPages;

            return PagedListDTO<T>.ToPagedList(items, totalCount, totalPages, currentPage, pageSize, hasPrevious, hasNext);
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> requiredExpression) =>
            _context.Set<T>().AsNoTracking().Where(requiredExpression);

        public virtual IQueryable<T> Filter(IQueryable<T> entity) => 
            entity;

        public virtual void Remove(T entity) => 
            _context.Set<T>().Remove(entity);

        public virtual void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
            _context.Set<T>().Update(entity);
        }
    }
}
