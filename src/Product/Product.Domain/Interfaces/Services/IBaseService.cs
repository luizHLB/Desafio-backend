using Product.Domain.DTO;
using System.Linq.Expressions;

namespace Product.Domain.Interfaces.Services
{
    public interface IBaseService<T> where T : class
    {
        Task<PagedListDTO<T>> PagedListAsync(Expression<Func<T, bool>> expression,
            int pageNumber,
            int pageSize);

        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
