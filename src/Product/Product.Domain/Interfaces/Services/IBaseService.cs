using Product.Domain.DTO;
using System.Linq.Expressions;

namespace Product.Domain.Interfaces.Services
{
    public interface IBaseService<T, TT> where T : class where TT : class
    {
        Task<PagedListDTO<TT>> PagedListAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        Task Add(T entity);
        Task Remove(long id);
        Task Update(T entity);
        Task<T> GetById(long id);
    }
}
