using Product.Domain.DTO;
using Product.Domain.Interfaces.Services;
using System.Linq.Expressions;

namespace Product.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T, TT> : IBaseRepositoryUserHandler where T : class where TT : class
    {
        Task<PagedListDTO<TT>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);
        IQueryable<T> Query(Expression<Func<T, bool>> requiredExpression);
        IQueryable<T> Filter(IQueryable<T> entity);
        Task Add(T entity);
        Task Remove(long id);
        Task Remove(T entity);
        void Update(T entity);
        Task<T> GetById(long id);
    }
}
