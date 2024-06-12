using Product.Domain.DTO;
using System.Linq.Expressions;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IBaseRespository<T> where T : class
    {
        Task<PagedListDTO<T>> PagedListAsync(Expression<Func<T, bool>> expression,
            int pageNumber,
            int pageSize,
            params Expression<Func<T, object>>[] includes);

        IQueryable<T> Query(Expression<Func<T, bool>> requiredExpression);
        IQueryable<T> Filter(IQueryable<T> entity);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
